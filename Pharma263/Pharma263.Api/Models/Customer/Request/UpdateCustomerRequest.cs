using System.ComponentModel.DataAnnotations;

namespace Pharma263.Api.Models.Customer.Request
{
    public class UpdateCustomerRequest
    {
        [Required(ErrorMessage = "Id is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid customer ID")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Physical address is required")]
        [StringLength(255, ErrorMessage = "Physical address cannot exceed 255 characters")]
        public string PhysicalAddress { get; set; }

        [StringLength(255, ErrorMessage = "Delivery address cannot exceed 255 characters")]
        public string DeliveryAddress { get; set; }

        [StringLength(50, ErrorMessage = "MCAZ License cannot exceed 50 characters")]
        public string MCAZLicence { get; set; }

        [StringLength(50, ErrorMessage = "HPA License cannot exceed 50 characters")]
        public string HPALicense { get; set; }

        [StringLength(30, ErrorMessage = "VAT Number cannot exceed 30 characters")]
        public string VATNumber { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid customer type")]
        public int CustomerTypeId { get; set; }
    }
}
