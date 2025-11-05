using System;

namespace Pharma263.Integration.Api.Models.Request
{
    public class AddStockRequest
    {
        public string MedicineName { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string BatchNo { get; set; }
        public decimal BuyingPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public int TotalQuantity { get; set; }
    }
}
