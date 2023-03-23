using CTUshshop.Areas.Admin.Models;
using CTUshshop.Areas.Identity.Data;
using CTUshshop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CTUshshop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles="Admin")]
    public class AdministratorController : Controller
    {

        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public AdministratorController(RoleManager<IdentityRole> roleManager,
                                        UserManager<ApplicationUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRole model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.RoleName
                };

                IdentityResult result = await roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return Redirect("/Admin/Administrator/ListRole");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }
            else
            {
                ViewBag.Error = "Model invalid";
            }

            return View(model);

        }

        [HttpGet]

        public IActionResult ListRole()
        {
            var roles = roleManager.Roles;
            return View(roles);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if(role == null)
            {
                ViewBag.ErrorMessage = $"Role with id = {id} cannot be found";
                return View();
            }
            var model = new EditRole
            {
                Id = role.Id,
                RoleName = role.Name
            };

            foreach (var user in userManager.Users)
            {
                if(await userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }

            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRole model)
        {
            var role = await roleManager.FindByIdAsync(model.Id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with id = {model.Id} cannot be found";
                return View();
            }
            else
            {
                role.Name = model.RoleName;
                var result = await roleManager.UpdateAsync(role);

                if(result.Succeeded)
                {
                    return Redirect("/Admin/Administrator/ListRole");
                }
                else
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
                return View(model);
            }
            

        }

        [HttpGet]
        public async Task<IActionResult> EditUserRole(string roleId)
        {
            ViewBag.roleId = roleId;

            var role = await roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with id = {roleId} cannot be found";
                return View();
            }
            ViewBag.roleName = role.Name;

            var model = new List<UserRole>();

            foreach(var user in userManager.Users)
            {
                var userRole = new UserRole
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };
                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRole.IsSelected = true;
                }
                else
                {
                    userRole.IsSelected = false;
                }

                model.Add(userRole);

            }
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> EditUserRole(List<UserRole> model, string roleId)
        {
            var url = "/Admin/Administrator/EditRole?id=" + roleId;
            var role = await roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with id = {roleId} cannot be found";
                return View();
            }
            for(int i = 0;i<model.Count;i++)
            {
                var user =  await userManager.FindByIdAsync(model[i].UserId);

                IdentityResult result = null;

                if(model[i].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }
                else if(!model[i].IsSelected && await userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

                if(result.Succeeded)
                {
                    if (i < (model.Count - 1))
                        continue;
                    else
                    {
                        
                        return Redirect(url);
                    }
                        
                }

            }
            return Redirect(url);
        }
    }
}

