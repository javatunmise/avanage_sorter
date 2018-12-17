using Avanage.SorterFeelLite.UI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Avanage.SorterFeelLite.UI.Data
{
    public class EfUserRepository : IUserRepository, IDisposable
    {
        private ApplicationDbContext context;

        public EfUserRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<IdentityRole> GetRoles(string id)
        {
            return (from userRole in context.UserRoles.Where(u => u.UserId == id)
                    join role in context.Roles on userRole.RoleId equals role.Id
                    select role
                    ).ToList();
        }

        public IEnumerable<ApplicationUser> GetUsers()
        {
            return context.Users
                .OrderBy(u => u.UserName)
                //.OrderByDescending(u => u.IsActive)
                .ToList();
        }

        public void UpdateRoles(string userId, IList<string> roleIds)
        {
            var existingRoles = context.UserRoles.Where(u => u.UserId == userId)
                .Select(r => r.RoleId);
            var deletedRoles = existingRoles.Except(roleIds);
            var addedRoles = roleIds.Except(existingRoles);

            foreach (var deleted in deletedRoles)
            {
                context.UserRoles.Remove(CreateUserRole(userId, deleted));
            }

            Save();

            foreach (var role in addedRoles)
            {
                context.UserRoles.Add(CreateUserRole(userId, role));
            }

            Save();
        }

        private void Save()
        {
            context.SaveChanges();
        }

        private static IdentityUserRole<string> CreateUserRole(string userId, string deleted)
        {
            return new IdentityUserRole<string> { UserId = userId, RoleId = deleted };
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public void UpdateUser(ApplicationUser user)
        {
            context.Entry(user).State = EntityState.Modified;
            context.SaveChanges();
        }
    }
}
