using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using CTUshshop.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using CTUshshop.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CTUshshop.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly shshopContext _context;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            shshopContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [StringLength(100,ErrorMessage ="Bla bla")]
            [Display(Name = "UserName")]
            public string UserName { get; set; }


            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [Display(Name = "Address")]
            [StringLength(200)]
            public string Address { get; set; }

            [Display(Name = "province")]
            public long ProvinceId { get; set; }
            [Display(Name = "district")]
            public long DistrictId { get; set; }
            [Display(Name = "ward")]
            public long WardId { get; set; }

            [Required]
            [Display(Name = "First Name")]
            [StringLength(10)]
            public string FirstName { get; set; }
            [Required]
            [Display(Name = "Middle Name")]
            [StringLength(10)]
            public string MiddleName { get; set; }
            [Required]
            [Display(Name = "Last Name")]
            [StringLength(15)]
            public string LastName { get; set; }


        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                Response.Redirect("/");
            }
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            var district = _context.District.ToList();
            var province = _context.Province.ToList();
            var ward = _context.Ward.ToList();

            ViewData["ProvinceId"] = new SelectList(province.ToList(), "Id", "Name");
            //ViewData["DistrictId"] = new SelectList(district.ToList(), "Id", "Name");
            //ViewData["WardId"] = new SelectList(ward.ToList(), "Id", "Name");
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { 
                    UserName = Input.UserName, 
                    Email = Input.Email,
                    Address = Input.Address,
                    ProvinceId = Input.ProvinceId,
                    DistrictId = Input.DistrictId,
                    WardId = Input.WardId,
                    FirstName = Input.FirstName,
                    MiddleName = Input.MiddleName,
                    LastName = Input.LastName,
                    EmailConfirmed = true
                };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                    _logger.LogInformation("User created a new account with password.");


                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                    var district = _context.District.ToList();
                    var province = _context.Province.ToList();
                    var ward = _context.Ward.ToList();

                    ViewData["DistrictId"] = new SelectList(district.ToList(), "Id", "Name");
                    ViewData["ProvinceId"] = new SelectList(province.ToList(), "Id", "Name");
                    ViewData["WardId"] = new SelectList(ward.ToList(), "Id", "Name");
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
