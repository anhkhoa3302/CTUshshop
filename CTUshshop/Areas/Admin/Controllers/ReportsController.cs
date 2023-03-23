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
    public class ReportsController : Controller
    {
        private readonly shshopContext _context;

        public ReportsController(shshopContext context)
        {
            _context = context;
        }

        // GET: Admin/Reports
        public async Task<IActionResult> Index()
        {
            var shshopContext = _context.Report.Include(r => r.ReportCategory).Include(r => r.User);
            return View(await shshopContext.ToListAsync());
        }

        // GET: Admin/Reports/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var report = await _context.Report
                .Include(r => r.ReportCategory)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (report == null)
            {
                return NotFound();
            }

            return View(report);
        }

        // GET: Admin/Reports/Create
        public IActionResult Create()
        {
            ViewData["ReportCategoryId"] = new SelectList(_context.ReportCategory, "Id", "Title");
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            return View();
        }

        // POST: Admin/Reports/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,ToUserId,ReportCategoryId,Summary,Content,CreatedAt")] Report report)
        {
            if (ModelState.IsValid)
            {
                _context.Add(report);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ReportCategoryId"] = new SelectList(_context.ReportCategory, "Id", "Title", report.ReportCategoryId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", report.UserId);
            return View(report);
        }

        // GET: Admin/Reports/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var report = await _context.Report.FindAsync(id);
            if (report == null)
            {
                return NotFound();
            }
            ViewData["ReportCategoryId"] = new SelectList(_context.ReportCategory, "Id", "Title", report.ReportCategoryId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", report.UserId);
            return View(report);
        }

        // POST: Admin/Reports/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,UserId,ToUserId,ReportCategoryId,Summary,Content,CreatedAt")] Report report)
        {
            if (id != report.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(report);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReportExists(report.Id))
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
            ViewData["ReportCategoryId"] = new SelectList(_context.ReportCategory, "Id", "Title", report.ReportCategoryId);
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", report.UserId);
            return View(report);
        }

        // GET: Admin/Reports/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var report = await _context.Report
                .Include(r => r.ReportCategory)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (report == null)
            {
                return NotFound();
            }

            return View(report);
        }

        // POST: Admin/Reports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var report = await _context.Report.FindAsync(id);
            _context.Report.Remove(report);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReportExists(long id)
        {
            return _context.Report.Any(e => e.Id == id);
        }
    }
}
