using System.Collections.Generic;
using System;

namespace Pharma263.Integration.Api.Models.Request
{
    public class UpdateQuotationRequest
    {
        public int Id { get; set; }
        public DateTime QuotationDate { get; set; }
        public string Notes { get; set; }
        public decimal Total { get; set; }
        public int QuoteStatusId { get; set; }
        public decimal Discount { get; set; }
        public decimal GrandTotal { get; set; }
        public int CustomerId { get; set; }
        public List<QuotationItemModel> Items { get; set; }
    }
}
