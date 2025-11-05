namespace Pharma263.Api.Models
{
    public class StockModel
    {
        public int Id { get; set; }
        public string MedicineName { get; set; }
        public string BatchNo { get; set; }
        public int TotalQuantity { get; set; }
    }
}