using System.Collections.Generic;
using System;

namespace Pharma263.Integration.Api.Models.Request
{
    public class CreateQuotationRequest
    {
        public int CustomerId { get; set; }
        public DateTime QuotationDate { get; set; }
        public int QuoteStatus { get; set; }
        public decimal Total { get; set; }
        public string Notes { get; set; }
        public decimal Discount { get; set; }
        public decimal GrandTotal { get; set; }
        
        public List<QuotationItemModel> Items { get; set; }
    }
}
