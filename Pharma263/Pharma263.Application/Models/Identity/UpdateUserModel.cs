using Microsoft.AspNetCore.Http;

namespace Pharma263.Application.Models.Identity
{
    public class UpdateUserModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public IFormFile ProfilePhoto { get; set; }
    }
}
