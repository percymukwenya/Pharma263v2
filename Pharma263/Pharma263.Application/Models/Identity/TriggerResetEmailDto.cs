using System.ComponentModel.DataAnnotations;

namespace Pharma263.Application.Models.Identity
{
    public class TriggerResetEmailDto
    {
        [Required]
        public string UserId { get; set; }
    }
}
