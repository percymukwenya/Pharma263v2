using System.Collections.Generic;

namespace Pharma263.Integration.Api.Models.Response
{
    public class UserListResponse
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
