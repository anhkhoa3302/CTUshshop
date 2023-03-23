using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CTUshshop.Areas.Identity.Data;
using CTUshshop.Data;
using CTUshshop.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CTUshshop.Areas.Identity.Pages.Account.Manage
{
    public class EditUserProductsModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly shshopContext _context;
        private IWebHostEnvironment _env;

        public EditUserProductsModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            shshopContext context,
            IWebHostEnvironment env
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _env = env;
        }

        public string UserId { get; set; }
        public string Username { get; set; }

        public byte Status { get; set; }
        public List<Category> categoryList { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public List<ProductImg> Images { get; set; }

        [TempData]
        public string StatusMessage { get; set; }


        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            public long Id { get; set; }

            [Required]
            [StringLength(256)]
            [Display(Name = "Title")]
            public string Title { get; set; }

            [Required]
            [Display(Name = "Summary")]
            public string Summary { get; set; }

            [Required]
            [Display(Name = "Content")]
            public string Content { get; set; }

            [Required]
            [Display(Name = "Price")]
            public double Price { get; set; }

            [Required]
            [Display(Name = "Quantity")]
            public double Quantity { get; set; }

            [Required]
            [Display(Name = "Unit")]
            [StringLength(20)]
            public string Unit { get; set; }

            public bool isHided { get; set; }

            public List<long> Categories { get; set; }
            public List<ImgDelete> DeleteImgList { get; set; }
            public List<IFormFile> Files { get; set; }

            public int tagchanged { get; set; }
        }

        private async Task LoadAsync(long id)
        {
            var product = await _context.Product.FindAsync(id);
            Images = await _context.ProductImg.Where(s => s.ProductId == id).ToListAsync();
            Input = new InputModel
            {
                Id = id,
                Title = product.Title,
                Summary = product.Summary,
                Content = product.Content,
                Price = product.Price,
                Quantity = (double)product.Quantity,
                Unit = product.Unit
            };
            if(product.Status == 1)
            {
                Input.isHided = false;
            }
            else if(product.Status == 0)
            {
                Input.isHided = true;
            }

            categoryList = await _context.ProductCategory.Include(x => x.Category).Where(s => s.ProductId == id).Select(cat => cat.Category).ToListAsync();

            var categories = await _context.Category.ToListAsync();

            var sortCats = categories.Except(categoryList);

            ViewData["Categories"] = new SelectList(sortCats.ToList(),"Id", "Title");
            


        }
        public async Task<IActionResult> OnGetAsync(long id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }



            await LoadAsync(id);
            Username = user.UserName;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IEnumerable<IFormFile> files)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if(Input.tagchanged == 1)
            {
                bool tagged = false;
                for (int n = 0; n < 5; n++)
                {
                    if (Input.Categories[n] != -1)
                    {
                        bool valid = false;
                        var t = await _context.Category.FindAsync(Input.Categories[n]);
                        if (t != null) valid = true;
                        if (!valid)
                        {
                            StatusMessage = "Error: Tag invalid";
                            return RedirectToPage("./EditUserProducts", new { id = Input.Id });
                        }
                        tagged = true;
                    }

                }

                if (!tagged)
                {
                    StatusMessage = "Error : Product need at least 1 tag!";
                    return RedirectToPage("./EditUserProducts", new { id = Input.Id });
                }

                var pcL = await _context.ProductCategory.Where(s => s.ProductId == Input.Id).ToListAsync();
                foreach(var item in pcL)
                {
                    _context.ProductCategory.Remove(item);
                }
                

                for (int n = 0; n < 5; n++)
                {
                    if (Input.Categories[n] == -1)
                    {
                        continue;
                    }
                    ProductCategory productCategory = new ProductCategory();
                    productCategory.ProductId = Input.Id;
                    productCategory.CategoryId = Input.Categories[n];
                    _context.ProductCategory.Add(productCategory);
                }
            }




            int i = 0;
            var userDir = Path.Combine(_env.WebRootPath, "files/" + user.UserName + "/images");

            int currentProductImg = _context.ProductImg.Where(s => s.ProductId == Input.Id).Count();
            //bool limit = false;
            if (files != null)
            {
                string[] pathList = new string[20];
                string[] filenameList = new string[20];
                string[] extList = { ".png", ".jpeg", ".jpg" };
                
                foreach(var file in files)
                {
                    if (currentProductImg >= 20)
                    {
                        StatusMessage = "Cannot upload more than 20 images for 1 product";
                        break; 
                    }
                    if (file.Length > 5242880)
                    {
                        StatusMessage = "Error: file too big (only allow 5Mb or lower)";
                        return Page();
                    }
                    var imgName = Guid.NewGuid().ToString() + file.FileName;
                    var path = Path.Combine(userDir, imgName);
                    var ext = Path.GetExtension(path).ToLower();
                    bool valid = false;
                    foreach (var item in extList)
                    {
                        if (ext.Equals(item))
                            valid = true;

                    }
                    if (valid)
                    {
                        pathList[i] = path;
                        filenameList[i] = imgName;
                    }
                    else
                    {
                        StatusMessage = "Error: png, jpeg or jpg only";
                        return Page();
                    }
                    if (!Directory.Exists(userDir))
                    {
                        Directory.CreateDirectory(userDir);
                    }
                    i++;
                    currentProductImg++;
                }
                int y=0;
                foreach (var file in files)
                {
                    if (y >= i)
                    {
                        StatusMessage = "Edit successfully! Product can only has 20 images maxium";
                        break;
                    }
                    var path = pathList[y];
                    using (FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write))
                    {
                        file.CopyTo(stream);
                    }
                    ProductImg img = new ProductImg();
                    img.ProductId = Input.Id;
                    img.Img = filenameList[y];
                    _context.ProductImg.Add(img);
                    //await _context.SaveChangesAsync();
                    y++;
                }

                

            }



            var updateProduct = await _context.Product.FirstOrDefaultAsync(s => s.Id == Input.Id);
            
            updateProduct.Title = Input.Title;
            updateProduct.Summary = Input.Summary;
            updateProduct.Content = Input.Content;
            updateProduct.Price = Input.Price;
            updateProduct.Quantity = Input.Quantity;
            updateProduct.Unit = Input.Unit;
            updateProduct.UpdatedAt = DateTime.Now;
            if(Input.isHided)
            {
                updateProduct.Status = 0;
            }
            else
            {
                updateProduct.Status = 1;
            }


            if(await TryUpdateModelAsync<Product>(
                updateProduct,
                "",
                s=>s.Title,
                s=>s.Summary,
                s=>s.Content,
                s => s.Price,
                s => s.Quantity,
                s => s.Unit,
                s => s.Status,
                s => s.UpdatedAt
                ))
            {
                foreach (var item in Input.DeleteImgList)
                {
                    if (item.isSelected)
                    {
                        FileInfo file = new FileInfo(Path.Combine(userDir, item.Img));
                        if (file.Exists)
                        {
                            if(currentProductImg<=6)
                            {
                                StatusMessage = "Error : can't delete more ! Product need at least 6 images!";
                                break;
                            }
                            file.Delete();
                            

                        }
                        ProductImg productImg = await _context.ProductImg.FindAsync(item.Id);
                        _context.ProductImg.Remove(productImg);
                        currentProductImg--;

                    }
                }
                try
                {
                    await _context.SaveChangesAsync();
                    StatusMessage = "Edit successfully";
                    return RedirectToPage("./EditUserProducts", new { id = Input.Id });
                }
                catch(DbUpdateException ex)
                {
                    return NotFound(ex);
                }
            }
            else
            {
                return NotFound("ModelStat invalid!");
            }


        }
    }
}
