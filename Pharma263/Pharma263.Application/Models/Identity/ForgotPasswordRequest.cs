using System.ComponentModel.DataAnnotations;

namespace Pharma263.Application.Models.Identity
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
