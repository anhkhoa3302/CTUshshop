using CTUshshop.Areas.Identity.Data;
using CTUshshop.Data;
using CTUshshop.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CTUshshop.Controllers
{
    public class ShoppingCartController : Controller
    {


        private readonly UserManager<ApplicationUser> _userManager;
        private readonly shshopContext _context;

        public ShoppingCartController(shshopContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            ViewBag.Categories =  _context.Category.ToList();
            return View(GetCartItems());
        }

        // Key lưu chuỗi json của Cart
        public const string CARTKEY = "cart";

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

        // Lưu Cart (Danh sách CartItem) vào session
        void SaveCartSession(List<CartItem> ls)
        {

            var session = HttpContext.Session;
            string jsoncart = JsonConvert.SerializeObject(ls);
            session.SetString(CARTKEY, jsoncart);
        }

        /// Thêm sản phẩm vào cart
        [HttpPost]
        public JsonResult AddToCart(long id)
        {

            List<ProductImg> productImg = _context.ProductImg.Where(n => n.ProductId == id).ToList();

            var product = _context.Product
                .Where(p => p.Id == id)
                .FirstOrDefault();

            string userId = product.UserId;
            var user = _context.AspNetUsers.Find(userId);
            string username = user.UserName;
            // Xử lý đưa vào Cart ...
            var cart = GetCartItems();
            var cartitem = cart.Find(p => p.Product.Id == id);
            if (cartitem != null)
            {
                // Đã tồn tại, tăng thêm 1
                if(cartitem.Quantity < product.Quantity)
                    cartitem.Quantity++;
            }
            else
            {
                //  Thêm mới
                cart.Add(new CartItem() { Quantity = 1, Product = product, UserName = username, ProductImg = productImg });
            }

            // Lưu cart vào Session
            SaveCartSession(cart);
            var ItemAmount = cart.Count;
            // Chuyển đến trang hiện thị Cart
            return Json(new { ItemAmount });
        }

        [HttpPost]
        public IActionResult AddToCart2(int quantity, long id)
        {

            List<ProductImg> productImg = _context.ProductImg.Where(n => n.ProductId == id).ToList();

            var product = _context.Product
                .Where(p => p.Id == id)
                .FirstOrDefault();

            string userId = product.UserId;
            var user = _context.AspNetUsers.Find(userId);
            string username = user.UserName;
            // Xử lý đưa vào Cart ...
            var cart = GetCartItems();
            var cartitem = cart.Find(p => p.Product.Id == id);

            int addQuantity = 0;
            if (quantity >= product.Quantity)
            {
                addQuantity = (int)product.Quantity;
            }
            else
            {
                addQuantity = quantity;
            }


            if (cartitem != null)
            {
                // Đã tồn tại, tăng thêm 1
                if (quantity >= product.Quantity)
                    cartitem.Quantity = (int)product.Quantity;
                else
                    cartitem.Quantity = quantity;
            }
            else
            {
                //  Thêm mới
                cart.Add(new CartItem() { Quantity = addQuantity, Product = product, UserName = username, ProductImg = productImg });
            }

            // Lưu cart vào Session
            SaveCartSession(cart);
            var ItemAmount = cart.Count;
            // Chuyển đến trang hiện thị Cart
            return RedirectToAction(nameof(Index));
        }

        [Route("/updatecart", Name = "updatecart")]
        [HttpPost]
        public IActionResult UpdateCart([FromForm] long productid, [FromForm] int quantity)
        {
            var product = _context.Product
            .Where(p => p.Id == productid)
            .FirstOrDefault();
            // Cập nhật Cart thay đổi số lượng quantity ...
            var cart = GetCartItems();
            var cartitem = cart.Find(p => p.Product.Id == productid);
            if (cartitem != null)
            {
                if (quantity >= product.Quantity)
                    cartitem.Quantity = (int)product.Quantity;
                else
                    cartitem.Quantity = quantity;
            }
            SaveCartSession(cart);
            // Trả về mã thành công (không có nội dung gì - chỉ để Ajax gọi)
            return Ok();
        }

        /// xóa item trong cart
        [Route("/removecart/{productid:int}", Name = "removecart")]
        public IActionResult RemoveCart([FromRoute] int productid)
        {
            var cart = GetCartItems();
            var cartitem = cart.Find(p => p.Product.Id == productid);
            if (cartitem != null)
            {
                // Đã tồn tại, tăng thêm 1
                cart.Remove(cartitem);
            }

            SaveCartSession(cart);
            return RedirectToAction(nameof(Index));
        }
    }
}
