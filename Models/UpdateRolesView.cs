using Microsoft.AspNetCore.Identity;
using System.Collections;
using System.Collections.Generic;

namespace Avanage.SorterFeelLite.UI.Controllers
{
    public class UpdateRolesView
    {
        public UpdateRolesView(string userId, IList<IdentityRole> roles, IList<string> userRoles)
        {
            this.Roles = roles;
            this.UserRoles = userRoles;
            this.UserId = userId;
        }

        public IList<IdentityRole> Roles { get; }
        public IList<string> UserRoles { get; }
        public string UserId { get; }
    }
}