using CTUshshop.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CTUshshop.Controllers
{
    public class ImagesController : Controller
    {
        private readonly shshopContext _context;

        public ImagesController(shshopContext context)
        {
            _context = context;
        }


    }
}
