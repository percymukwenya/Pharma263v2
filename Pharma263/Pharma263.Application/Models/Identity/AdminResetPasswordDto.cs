using System.ComponentModel.DataAnnotations;

namespace Pharma263.Application.Models.Identity
{
    public class AdminResetPasswordDto
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}
