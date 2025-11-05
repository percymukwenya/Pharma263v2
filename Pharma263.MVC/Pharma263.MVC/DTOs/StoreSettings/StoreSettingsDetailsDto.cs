using System.ComponentModel.DataAnnotations;

namespace Pharma263.MVC.DTOs.StoreSettings
{
    public class StoreSettingsDetailsDto
    {
        public int Id { get; set; }
        public string Logo { get; set; }
        public string StoreName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Currency { get; set; }
        public string Address { get; set; }
        public string MCAZLicence { get; set; }
        public string VATNumber { get; set; }
        [Display(Name = "Banking Details")]
        public string BankingDetails { get; set; }
        [Display(Name = "Returns Policy")]
        public string ReturnsPolicy { get; set; }
    }
}
