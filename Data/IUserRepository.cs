using System;
using System.Collections.Generic;
using Avanage.SorterFeelLite.UI.Models;
using Microsoft.AspNetCore.Identity;

namespace Avanage.SorterFeelLite.UI.Data
{
    public interface IUserRepository : IDisposable
    {
        void UpdateRoles(string userId, IList<string> roles);
        IEnumerable<IdentityRole> GetRoles(string id);
        IEnumerable<ApplicationUser> GetUsers();
        void UpdateUser(ApplicationUser user);
    }
}