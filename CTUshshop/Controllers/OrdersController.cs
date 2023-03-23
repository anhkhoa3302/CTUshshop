using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CTUshshop.Data;
using CTUshshop.Models;
using Microsoft.AspNetCore.Identity;
using CTUshshop.Areas.Identity.Data;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace CTUshshop.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly shshopContext _context;

        public OrdersController(shshopContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        // Key lưu chuỗi json của Cart
        public const string CARTKEY = "cart";
        // GET: Orders
        public async Task<IActionResult> Index()
        {
            ViewBag.Categories = await _context.Category.ToListAsync();


            List<CartItem> cart = GetCartItems();

            if (cart.Count == 0) return Redirect("/Products");
            
            ViewBag.CartItem = cart;


            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            Order order = new Order();
            order.FirstName = user.FirstName;
            order.MiddleName = user.MiddleName;
            order.LastName = user.LastName;
            order.Email = user.Email;
            order.PhoneNumber = user.PhoneNumber;
            order.UserId = user.Id;

            var province = await _context.Province.FindAsync(user.ProvinceId);
            var district = await _context.District.FindAsync(user.DistrictId);
            var ward = await _context.Ward.FindAsync(user.WardId);

            order.Address = user.Address +", "
                + ward.Type +' '
                + ward.Name + ", "
                + district.Type + ' ' 
                + district.Name + ", "
                + province.Type + ' ' 
                + province.Name;

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,FirstName,MiddleName,LastName,Email,PhoneNumber,Address,Tax,Shipping,ShippingVia,Total,GrandTotal,Status,CreatedAt,UpdatedAt")] Order order, string password)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var verigyResult = _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, password);

            if (verigyResult == PasswordVerificationResult.Success) // either verigyResult equal to SuccessRehashNeeded or Success
            {
                if (ModelState.IsValid)
                {
                    _context.Add(order);
                    await _context.SaveChangesAsync();
                    List<CartItem> listItem = GetCartItems();
                    foreach(var item in listItem)
                    {
                        OrderItem orderItem = new OrderItem();
                        orderItem.OrderId = order.Id;
                        orderItem.ProductId = item.Product.Id;
                        orderItem.Quantity = item.Quantity;
                        orderItem.TotalPrice = item.Quantity * item.Product.Price;
                        _context.Add(orderItem);
                    }
                    await _context.SaveChangesAsync();
                    ClearCart();
                    return Redirect("/Identity/Account/Manage/UserOrders");
                }
            }
            
            return RedirectToAction(nameof(Index));
        }
        
        [Authorize(Roles = "Admin")]
        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", order.UserId);
            return View(order);
        }

        [Authorize(Roles = "Admin")]
        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,UserId,FirstName,MiddleName,LastName,Email,PhoneNumber,Address,Tax,Shipping,ShippingVia,Total,GrandTotal,Status,CreatedAt,UpdatedAt")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
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
            ViewData["UserId"] = new SelectList(_context.AspNetUsers, "Id", "Id", order.UserId);
            return View(order);
        }

        [Authorize(Roles = "Admin")]
        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
        [Authorize(Roles = "Admin")]
        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var order = await _context.Order.FindAsync(id);
            _context.Order.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(long id)
        {
            return _context.Order.Any(e => e.Id == id);
        }



        // Lấy cart từ Session (danh sách CartItem)
        List<CartItem> GetCartItems()
        {
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            var session = HttpContext.Session;
            string jsoncart = session.GetString(CARTKEY);
            if (jsoncart != null)
            {
                return JsonConvert.DeserializeObject<List<CartItem>>(jsoncart);
            }
            return new List<CartItem>();
        }

        // Xóa cart khỏi session
        void ClearCart()
        {
            var session = HttpContext.Session;
            session.Remove(CARTKEY);
        }
    }
}
