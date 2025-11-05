using System;

namespace Pharma263.MVC.DTOs.Customer
{
    public class CustomerDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string PhysicalAddress { get; set; }
        public string DeliveryAddress { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public string MCAZLicence { get; set; } = string.Empty;
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
