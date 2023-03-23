using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    public partial class UserProductsModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly shshopContext _context;

        public UserProductsModel(
            UserManager<ApplicationUser> userManager,
            shshopContext context
            )
        {
            _userManager = userManager;
            _context = context;
        }
        public string Username { get; set; }
        public string UserId { get; set; }

        public Pager pager { get; set; }

        public List<Product> ProductList { get; set; }


        [TempData]
        public string StatusMessage { get; set; }
        private async Task LoadAsync(ApplicationUser user, int pg)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var userId = await _userManager.GetUserIdAsync(user);

            Username = userName;
            UserId = userId;

            var shshopdbContext = await _context.Product.Where(m => m.UserId == UserId).Include(p=>p.ProductImg).ToListAsync();
            //const int pageSize = 4;
            //if (pg < 1)
            //{
            //    pg = 1;
            //}
            //int recsCount = shshopdbContext.Count();

            //pager = new Pager(recsCount, pg, pageSize);

            //ViewData["Pager"] = pager;

            //int recSkip = (pg - 1) * pageSize;

            //var userProducts = shshopdbContext.Skip(recSkip).Take(pager.PageSize);


            //ProductList = userProducts.ToList();
            ProductList = shshopdbContext.ToList();
        }

        public async Task<IActionResult> OnGetAsync(int pg=1)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user,pg);

            
            return Page();
        }
    }
}
