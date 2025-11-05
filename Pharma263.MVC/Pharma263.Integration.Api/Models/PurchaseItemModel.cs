using System;

namespace Pharma263.Integration.Api.Models
{
    public class PurchaseItemModel
    {
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Amount { get; set; }
        public decimal Discount { get; set; }
        public int MedicineId { get; set; }
        public string MedicineName { get; set; }
        public string BatchNo { get; set; }
        public DateTimeOffset ExpiryDate { get; set; }
        public decimal BuyingPrice { get; set; }
        public decimal SellingPrice { get; set; }
    }
}
