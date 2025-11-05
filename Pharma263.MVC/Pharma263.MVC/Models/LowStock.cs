namespace Pharma263.MVC.Models
{
    public class LowStock
    {
        public int Id { get; set; }
        public string MedicineName { get; set; }
        public string BatchNo { get; set; }
        public int TotalQuantity { get; set; }
        public double BuyingPrice { get; set; }
        public double SellingPrice { get; set; }
    }
}