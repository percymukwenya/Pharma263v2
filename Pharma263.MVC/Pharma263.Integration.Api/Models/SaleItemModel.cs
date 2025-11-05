namespace Pharma263.Integration.Api.Models
{
    public class SaleItemModel
    {
        public int StockId { get; set; }
        public string MedicineName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Amount { get; set; }
        public decimal Discount { get; set; }
    }
}
