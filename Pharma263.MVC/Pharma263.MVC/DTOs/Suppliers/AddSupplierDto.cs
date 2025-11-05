using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Pharma263.MVC.DTOs.Suppliers
{
    public class AddSupplierDto
    {
        [Required]
        [DisplayName("Supplier Name")]
        public string Name { get; set; }

        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(10)]
        [MaxLength(15)]
        public string Phone { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(100)]
        public string Address { get; set; }

        public string Notes { get; set; }
        public string MCAZLicence { get; set; }
        public string BusinessPartnerNumber { get; set; }
        public string VATNumber { get; set; }
    }
}
