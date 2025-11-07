namespace Pharma263.Integration.Api.Models.Response
{
    public class LowStockModel
    {
        public string MedicineName { get; set; }
        public string BatchNo { get; set; }
        public int TotalQuantity { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal BuyingPrice { get; set; }
    }
}
