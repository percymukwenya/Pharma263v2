using System;

namespace Pharma263.Api.Models.Quotation
{
    public class QuotationItemModel
    {
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Amount { get; set; }
        public decimal Discount { get; set; }
        public int StockId { get; set; }
        public int QuotationId { get; set; }
    }
}
