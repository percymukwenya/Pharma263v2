namespace Pharma263.MVC.DTOs.Sales
{
    public class SalesItemDto
    {
        public int StockId { get; set; }
        public string MedicineName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Amount { get; set; }
        public decimal Discount { get; set; }
    }
}