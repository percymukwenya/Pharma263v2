using System.ComponentModel.DataAnnotations;

namespace Pharma263.Integration.Api.Models.Request
{
    public class CreateSupplierRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Address { get; set; }
        public string Notes { get; set; }
        public string MCAZLicence { get; set; }
        public string BusinessPartnerNumber { get; set; }
        public string VATNumber { get; set; }
    }
}
