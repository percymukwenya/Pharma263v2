namespace Pharma263.Domain.Models
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
