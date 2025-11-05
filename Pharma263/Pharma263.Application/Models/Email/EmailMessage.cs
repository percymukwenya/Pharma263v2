using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Pharma263.Application.Models.Email
{
    public class EmailMessage
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<IFormFile> Attachments { get; set; }
    }
}
