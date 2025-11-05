using System.ComponentModel.DataAnnotations;

namespace Pharma263.MVC.DTOs.StoreSettings
{
    public class UpdateStoreSettingsDto
    {
        public int Id { get; set; }
        public string Logo { get; set; } = string.Empty;
        public string StoreName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Currency { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string MCAZLicence { get; set; }
        public string VATNumber { get; set; }
        [Display(Name = "Banking Details")]
        public string BankingDetails { get; set; }
        [Display(Name = "Returns Policy")]
        public string ReturnsPolicy { get; set; }
    }
}
