using Pharma263.MVC.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Pharma263.MVC.DTOs.Customer
{
    public class AddCustomerDto
    {
        [Required]
        [DisplayName("Name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

        [MinLength(10)]
        [MaxLength(15)]
        public string Phone { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(100)]
        public string PhysicalAddress { get; set; }

        [MinLength(5)]
        [MaxLength(100)]
        public string DeliveryAddress { get; set; }

        public string MCAZLicence { get; set; }
        public string HPALicense { get; set; }
        public string VATNumber { get; set; }

        [Required]
        public int CustomerTypeId { get; set; }
    }
}
