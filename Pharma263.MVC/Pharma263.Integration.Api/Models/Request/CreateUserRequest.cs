using System.Collections.Generic;

namespace Pharma263.Integration.Api.Models.Request
{
    public class CreateUserRequest
    {
        public string UserName { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
