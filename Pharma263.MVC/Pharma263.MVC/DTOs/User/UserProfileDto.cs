using System.ComponentModel.DataAnnotations;

namespace Pharma263.MVC.DTOs.User
{
    public class UserProfileDto
    {
        public string Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Firstname { get; set; }

        [Required]
        public string Lastname { get; set; }

        public string Phone { get; set; }
    }
}
