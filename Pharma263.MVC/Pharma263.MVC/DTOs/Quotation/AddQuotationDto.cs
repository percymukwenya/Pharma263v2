using System.Collections.Generic;
using System;

namespace Pharma263.MVC.DTOs.Quotation
{
    public class AddQuotationDto
    {
        public int CustomerId { get; set; }
        public DateTime QuotationDate { get; set; }
        public int QuotationStatus { get; set; }
        public decimal Total { get; set; }
        public string Notes { get; set; }
        public decimal Discount { get; set; } = 0;
        public decimal GrandTotal { get; set; }
        public List<QuotationItemDto> Items { get; set; }
    }
}
