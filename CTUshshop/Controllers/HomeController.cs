
using CTUshshop.Data;
using CTUshshop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CTUshshop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly shshopContext _context;

        public HomeController(ILogger<HomeController> logger, shshopContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var shshopContext = _context.Product.Include(p => p.User).Include(n=>n.ProductImg);
            ViewBag.Categories = await _context.Category.ToListAsync();
            return View(await shshopContext.ToListAsync());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



        //[HttpPost]
        //public JsonResult GetDistrict(string DistrictId)
        //{
        //    Repository repo = new Repository();
        //}
    }
}
