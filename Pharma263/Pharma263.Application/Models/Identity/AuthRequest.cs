using System.ComponentModel.DataAnnotations;

namespace Pharma263.Application.Models.Identity
{
    public class AuthRequest
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
