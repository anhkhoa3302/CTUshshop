using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CTUshshop.Data;
using CTUshshop.Models;
using Microsoft.AspNetCore.Authorization;

namespace CTUshshop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly shshopContext _context;

        public UsersController(shshopContext context)
        {
            _context = context;
        }

        // GET: Admin/Users
        public async Task<IActionResult> Index()
        {
            var shshopContext = _context.AspNetUsers.Include(a => a.District).Include(a => a.Province).Include(a => a.Ward);
            return View(await shshopContext.ToListAsync());
        }

        // GET: Admin/Users/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aspNetUsers = await _context.AspNetUsers
                .Include(a => a.District)
                .Include(a => a.Province)
                .Include(a => a.Ward)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aspNetUsers == null)
            {
                return NotFound();
            }

            return View(aspNetUsers);
        }

        // GET: Admin/Users/Create
        public IActionResult Create()
        {
            ViewData["DistrictId"] = new SelectList(_context.District, "Id", "Name");
            ViewData["ProvinceId"] = new SelectList(_context.Province, "Id", "Name");
            ViewData["WardId"] = new SelectList(_context.Ward, "Id", "Name");
            return View();
        }

        // POST: Admin/Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount,FirstName,MiddleName,LastName,Address,ProvinceId,DistrictId,WardId,ProfilePic,CreatedAt,Status")] AspNetUsers aspNetUsers)
        {
            if (ModelState.IsValid)
            {
                _context.Add(aspNetUsers);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DistrictId"] = new SelectList(_context.District, "Id", "Name", aspNetUsers.DistrictId);
            ViewData["ProvinceId"] = new SelectList(_context.Province, "Id", "Name", aspNetUsers.ProvinceId);
            ViewData["WardId"] = new SelectList(_context.Ward, "Id", "Name", aspNetUsers.WardId);
            return View(aspNetUsers);
        }

        // GET: Admin/Users/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aspNetUsers = await _context.AspNetUsers.FindAsync(id);
            if (aspNetUsers == null)
            {
                return NotFound();
            }
            ViewData["DistrictId"] = new SelectList(_context.District, "Id", "Name", aspNetUsers.DistrictId);
            ViewData["ProvinceId"] = new SelectList(_context.Province, "Id", "Name", aspNetUsers.ProvinceId);
            ViewData["WardId"] = new SelectList(_context.Ward, "Id", "Name", aspNetUsers.WardId);
            return View(aspNetUsers);
        }

        // POST: Admin/Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount,FirstName,MiddleName,LastName,Address,ProvinceId,DistrictId,WardId,ProfilePic,CreatedAt,Status")] AspNetUsers aspNetUsers)
        {
            if (id != aspNetUsers.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aspNetUsers);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AspNetUsersExists(aspNetUsers.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DistrictId"] = new SelectList(_context.District, "Id", "Name", aspNetUsers.DistrictId);
            ViewData["ProvinceId"] = new SelectList(_context.Province, "Id", "Name", aspNetUsers.ProvinceId);
            ViewData["WardId"] = new SelectList(_context.Ward, "Id", "Name", aspNetUsers.WardId);
            return View(aspNetUsers);
        }

        // GET: Admin/Users/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aspNetUsers = await _context.AspNetUsers
                .Include(a => a.District)
                .Include(a => a.Province)
                .Include(a => a.Ward)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aspNetUsers == null)
            {
                return NotFound();
            }

            return View(aspNetUsers);
        }

        // POST: Admin/Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var aspNetUsers = await _context.AspNetUsers.FindAsync(id);
            _context.AspNetUsers.Remove(aspNetUsers);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AspNetUsersExists(string id)
        {
            return _context.AspNetUsers.Any(e => e.Id == id);
        }
    }
}
