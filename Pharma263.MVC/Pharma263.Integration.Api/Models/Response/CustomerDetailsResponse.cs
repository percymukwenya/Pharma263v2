using System;

namespace Pharma263.Integration.Api.Models.Response
{
    public class CustomerDetailsResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string PhysicalAddress { get; set; }
        public string DeliveryAddress { get; set; }
        public string Notes { get; set; }
        public string MCAZLicence { get; set; }
        public string HPALicense { get; set; }
        public string VATNumber { get; set; }
        public int CustomerTypeId { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
