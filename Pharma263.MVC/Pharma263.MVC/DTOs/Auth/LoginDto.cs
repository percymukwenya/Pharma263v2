using System.ComponentModel.DataAnnotations;

namespace Pharma263.MVC.DTOs.Auth
{
    public class LoginDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
