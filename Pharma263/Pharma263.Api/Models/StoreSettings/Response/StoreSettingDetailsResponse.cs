namespace Pharma263.Api.Models.StoreSettings.Response
{
    public class StoreSettingDetailsResponse
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
        public string BankingDetails { get; set; }
        public string ReturnsPolicy { get; set; }
    }
}
