using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CTUshshop.Areas.Identity.Data;
using CTUshshop.Data;
using CTUshshop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CTUshshop.Areas.Identity.Pages.Account.Manage
{
    public class UserOrdersDetailsModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly shshopContext _context;

        public UserOrdersDetailsModel(UserManager<ApplicationUser> userManager, shshopContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public string Username { get; set; }
        public string UserId { get; set; }
        public Order Order { get; set; }
        public List<OrderItem> OrderItems { get; set; }

        private async Task LoadAsync(long id)
        {
            var order = await _context.Order.FindAsync(id);
            var orderItems = await _context.OrderItem.Include(n=>n.Product).Include(m=>m.Product.ProductImg).Include(j=>j.Product.User).Where(p => p.OrderId == order.Id).ToListAsync();

            Order = order;
            OrderItems = orderItems;
        }
        public async Task<IActionResult> OnGetAsync(long id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            Username = user.UserName;
            UserId = user.Id;
            await LoadAsync(id);

            return Page();
        }
    }
}
