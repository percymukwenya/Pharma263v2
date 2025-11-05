using System.ComponentModel.DataAnnotations;

namespace Pharma263.MVC.DTOs.Auth
{
    public class RegisterDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
