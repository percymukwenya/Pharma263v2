using Pharma263.Domain.Common;
using System;
using System.Collections.Generic;

namespace Pharma263.Domain.Entities
{
    public class Quotation : ConcurrencyTokenEntity, IAuditable
    {
        public DateTime QuotationDate { get; set; }
        public string Notes { get; set; }
        public decimal Total { get; set; }
        public decimal Discount { get; set; }
        public decimal GrandTotal { get; set; }
        public DateTimeOffset? QuoteExpiryDate { get; set; }

        public int QuoteStatusId { get; set; }
        public QuoteStatus QuoteStatus { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public ICollection<QuotationItems> Items { get; set; }        
    }
}
