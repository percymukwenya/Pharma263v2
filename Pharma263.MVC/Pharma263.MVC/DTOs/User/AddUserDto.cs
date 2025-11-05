using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pharma263.MVC.DTOs.User
{
    public class AddUserDto
    {
        [Required]
        public string UserName { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }

        public string Phone { get; set; }
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }

        public IEnumerable<SelectListItem> Roles { get; set; }
    }
}
