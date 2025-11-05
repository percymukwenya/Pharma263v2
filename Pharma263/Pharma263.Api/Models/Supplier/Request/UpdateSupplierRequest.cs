using System.ComponentModel.DataAnnotations;

namespace Pharma263.Api.Models.Supplier.Request
{
    public class UpdateSupplierRequest
    {
        [Required(ErrorMessage = "Id is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid supplier ID")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Supplier name is required")]
        [StringLength(100, ErrorMessage = "Supplier name cannot exceed 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [StringLength(255, ErrorMessage = "Address cannot exceed 255 characters")]
        public string Address { get; set; }

        [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters")]
        public string Notes { get; set; }

        [StringLength(50, ErrorMessage = "MCAZ License cannot exceed 50 characters")]
        public string MCAZLicence { get; set; }

        [StringLength(50, ErrorMessage = "Business Partner Number cannot exceed 50 characters")]
        public string BusinessPartnerNumber { get; set; }

        [StringLength(30, ErrorMessage = "VAT Number cannot exceed 30 characters")]
        public string VATNumber { get; set; }
    }
}
