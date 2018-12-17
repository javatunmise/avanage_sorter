using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Avanage.SorterFeelLite.UI.Data
{
    public class UserRoleView
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public bool IsActive { get; set; }

        public List<IdentityRole> Roles { get; set; }
    }
}