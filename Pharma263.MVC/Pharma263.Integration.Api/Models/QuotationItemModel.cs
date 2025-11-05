using System;

namespace Pharma263.Integration.Api.Models
{
    public class QuotationItemModel
    {
        public int StockId { get; set; }
        public string MedicineName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Amount { get; set; }
        public decimal Discount { get; set; }

        public DateTimeOffset ExpiryDate { get; set; }
        public int QuotationId { get; set; }
    }
}
