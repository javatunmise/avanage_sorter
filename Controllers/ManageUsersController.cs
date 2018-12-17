using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avanage.SorterFeelLite.UI.Data;
using Avanage.SorterFeelLite.UI.Models;
using Avanage.SorterFeelLite.UI.Models.UsersViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Avanage.SorterFeelLite.UI.Controllers
{
    public class ManageUsersController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IUserRepository userRepository;
        private readonly IPasswordHasher<ApplicationUser> passwordHasher;

        public ManageUsersController(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, 
            IUserRepository userRepository,
            IPasswordHasher<ApplicationUser> passwordHasher)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.userRepository = userRepository;
            this.passwordHasher = passwordHasher;
        }

        public IActionResult Index()
        {
            var users = new List<UserRoleView>();
            using (userRepository)
            {
                users = userRepository.GetUsers()
                   .Select(u => new UserRoleView
                   {
                       UserId = u.Id,
                       UserName = u.UserName,
                       IsActive = u.IsActive,
                       Roles = userRepository.GetRoles(u.Id).ToList()
                   }).ToList();
            }

            return View(users);
        }

        public IActionResult Add()
        {
            var model = new AddUserViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddUserViewModel model)
        {
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = model.Username ?? "",
                Email = model.Email ?? "",
                NormalizedEmail = model.Email?.ToUpper(),
                PhoneNumber = "",
                SecurityStamp = Guid.NewGuid().ToString(),
                NormalizedUserName = model.Username?.ToUpper(),
            };

            var hashedPassword = passwordHasher.HashPassword(user, "password");
            user.PasswordHash = hashedPassword;

            var result = await userManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                ViewBag.PageInfo = new PageInfo("", PageInfo.ERROR, errors);
                return View(model);
            }

            ViewBag.PageInfo = new PageInfo("User added!", PageInfo.INFO);

            return View();
        }

        public async Task<IActionResult> Edit(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            ViewBag.UserId = userId;

            return View(new AddUserViewModel
            {
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Username = user.UserName,
                IsActive = user.IsActive
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string userId, AddUserViewModel model)
        {
            var user = await userManager.FindByIdAsync(userId);
            user.IsActive = model.IsActive;

            model.Email = user.Email;
            model.PhoneNumber = user.PhoneNumber;
            model.Username = user.UserName;

            //try
            //{
            using (userRepository)
            {
                userRepository.UpdateUser(user);
            }
            //}
            //catch (Exception e)
            //{
            //    var errors = new[] { e.Message };
            //    ViewBag.PageInfo = new PageInfo("", PageInfo.ERROR, errors);
            //    return View(model);
            //}

            ViewBag.PageInfo = new PageInfo("User updated!", PageInfo.INFO);

            return View(model);
        }

        public IActionResult Delete(AddUserViewModel model)
        {
            return View();
        }

        public IActionResult UpdateStatus(string userId, bool isActive)
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> UpdateRoles(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            var updateRolesView = new UpdateRolesView(userId, roleManager.Roles.ToList(), await userManager.GetRolesAsync(user));
            return View(updateRolesView);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRoles(string userId, string[] roleIds)
        {
            var user = await userManager.FindByIdAsync(userId);
            var updateRolesView = new UpdateRolesView(userId, roleManager.Roles.ToList(), await userManager.GetRolesAsync(user));
            if (roleIds.Any())
            {
                using (userRepository)
                {
                    userRepository.UpdateRoles(userId, roleIds);
                }
            }

            ViewBag.PageInfo = new PageInfo("Roles has been added successfully.", PageInfo.INFO);

            return View(updateRolesView);
        }
    }
}