using System;

namespace Pharma263.Api.Models.Quotation.Response
{
    public class QuotationItemResponse
    {
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Amount { get; set; }
        public int StockId { get; set; }
        public string Medicine { get; set; }
        public DateTimeOffset ExpiryDate { get; set; }
        public int QuotationId { get; set; }
    }
}
