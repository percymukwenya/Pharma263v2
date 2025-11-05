namespace Pharma263.MVC.DTOs.StoreSettings
{
    public class AddStoreSettingsDto
    {
        public string Logo { get; set; } = string.Empty;
        public string StoreName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Currency { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string MCAZLicence { get; set; }
        public string VATNumber { get; set; }
    }
}
