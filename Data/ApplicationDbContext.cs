using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Avanage.SorterFeelLite.UI.Models;
using Microsoft.AspNetCore.Identity;

namespace Avanage.SorterFeelLite.UI.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public List<UserRoleView> GetUserRoles()
        {
            var users = from user in Users
                        join userRole in UserRoles on user.Id equals userRole.UserId into userRoles
                        from userRole in userRoles.DefaultIfEmpty()
                        join role in Roles on userRole.RoleId equals role.Id into roleNames
                        from g in roleNames.DefaultIfEmpty()
                        select new UserRoleView
                        { UserId = user.Id, UserName = user.UserName, Roles = roleNames.ToList() };

            return users.ToList();
        }

        public void AddUserToRoles(string userId, IList<string> roles)
        {
            foreach (var role in roles)
                UserRoles.Add(new IdentityUserRole<string> { UserId = userId, RoleId = role });

            SaveChanges();
        }
    }
}
