using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Pharma263.Integration.Api.Models.Request
{
    public class CreateCustomerRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string PhysicalAddress { get; set; }
        public string DeliveryAddress { get; set; }
        public string MCAZLicence { get; set; }
        public string HPALicense { get; set; }
        public string VATNumber { get; set; }
        public int CustomerTypeId { get; set; }
    }
}
