using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Avanage.SorterFeelLite.UI.Models.UsersViewModels
{
    public class AddUserViewModel
    {
        public AddUserViewModel()
        {
            Username = "";
            Email = "";
            PhoneNumber = "";
        }

        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
    }
}
