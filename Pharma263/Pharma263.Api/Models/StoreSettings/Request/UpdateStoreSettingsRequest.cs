using System.ComponentModel.DataAnnotations;

namespace Pharma263.Api.Models.StoreSettings.Request
{
    public class UpdateStoreSettingsRequest
    {
        [Required(ErrorMessage = "Id is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid store settings ID")]
        public int Id { get; set; }

        [StringLength(500, ErrorMessage = "Logo path cannot exceed 500 characters")]
        public string Logo { get; set; }

        [Required(ErrorMessage = "Store name is required")]
        [StringLength(100, ErrorMessage = "Store name cannot exceed 100 characters")]
        public string StoreName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address format")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Currency is required")]
        [StringLength(10, ErrorMessage = "Currency cannot exceed 10 characters")]
        public string Currency { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [StringLength(500, ErrorMessage = "Address cannot exceed 500 characters")]
        public string Address { get; set; }

        [StringLength(50, ErrorMessage = "MCAZ licence cannot exceed 50 characters")]
        public string MCAZLicence { get; set; }

        [StringLength(20, ErrorMessage = "VAT number cannot exceed 20 characters")]
        public string VATNumber { get; set; }

        [StringLength(500, ErrorMessage = "Banking details cannot exceed 500 characters")]
        public string BankingDetails { get; set; }

        [StringLength(1000, ErrorMessage = "Returns policy cannot exceed 1000 characters")]
        public string ReturnsPolicy { get; set; }
    }
}
