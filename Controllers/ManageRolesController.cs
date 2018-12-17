using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avanage.SorterFeelLite.UI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Avanage.SorterFeelLite.UI.Controllers
{
    public class ManageRolesController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public ManageRolesController(RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        //List all roles
        public IActionResult Index()
        {
            var roles = roleManager.Roles.OrderBy(o => o.Name).ToList();
            return View(roles);
        }

        //List users in a role
        public async Task<IActionResult> Users(string roleName = "")
        {
            var userList = await userManager.GetUsersInRoleAsync(roleName);
            return View("RoleUsers", userList.Select(u => u.UserName));
        }
    }
}