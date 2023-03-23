using CTUshshop.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CTUshshop.Controllers
{
    public class AddressController : Controller
    {
        private readonly shshopContext _context;

        public AddressController(shshopContext context)
        {
            _context = context;
        }

        [HttpPost]
        public JsonResult GetDistrict(string ProvinceId)
        {
            var districts = _context.District.Where(z => z.ProvinceId == Convert.ToInt64(ProvinceId));
            foreach (var district in districts)
            {
                district.Name = district.Type + ' ' + district.Name;
            }
            return Json(new SelectList(districts.ToList(), "Id", "Name"));
        }

        [HttpPost]
        public JsonResult GetWard(string DistrictId)
        {
            var wards = _context.Ward.Where(z => z.DistrictId == Convert.ToInt64(DistrictId));
            foreach (var ward in wards)
            {
                ward.Name = ward.Type + ' ' + ward.Name;
            }
            return Json(new SelectList(wards.ToList(), "Id", "Name"));
        }
    }
}
