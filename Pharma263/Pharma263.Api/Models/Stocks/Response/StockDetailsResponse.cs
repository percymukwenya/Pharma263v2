using System;

namespace Pharma263.Api.Models.Stocks.Response
{
    public class StockDetailsResponse
    {
        public int Id { get; set; }
        public int MedicineId { get; set; }
        public string MedicineName { get; set; }
        public DateTimeOffset ExpiryDate { get; set; }
        public string BatchNo { get; set; }
        public double BuyingPrice { get; set; }
        public double SellingPrice { get; set; }
        public int TotalQuantity { get; set; }
    }
}
