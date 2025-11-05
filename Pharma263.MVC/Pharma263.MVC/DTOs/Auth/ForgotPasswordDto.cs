using System.ComponentModel.DataAnnotations;

namespace Pharma263.MVC.DTOs.Auth
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
