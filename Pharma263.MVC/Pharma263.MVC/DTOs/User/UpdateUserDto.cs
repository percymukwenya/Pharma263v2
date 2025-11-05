using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Pharma263.MVC.DTOs.User
{
    public class UpdateUserDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public IEnumerable<SelectListItem> Roles { get; set; }
    }
}
