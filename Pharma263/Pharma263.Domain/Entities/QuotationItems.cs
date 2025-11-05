using Pharma263.Domain.Common;

namespace Pharma263.Domain.Entities
{
    public class QuotationItems : ConcurrencyTokenEntity, IAuditable
    {
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Discount { get; set; }
        public decimal Amount { get; set; }

        public int StockId { get; set; }
        public Stock Stock { get; set; }

        public int QuotationId { get; set; }
        public Quotation Quotation { get; set; }
    }
}