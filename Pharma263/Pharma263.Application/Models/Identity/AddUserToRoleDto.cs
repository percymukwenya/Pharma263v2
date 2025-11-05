using System.ComponentModel.DataAnnotations;

namespace Pharma263.Application.Models.Identity
{
    public class AddUserToRoleDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Role name is required")]
        [StringLength(50, ErrorMessage = "Role name cannot exceed 50 characters")]
        public string RoleName { get; set; }
    }
}
