using System;
using System.Collections.Generic;

namespace Pharma263.MVC.DTOs.Quotation
{
    public class UpdateQuotationDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime QuotationDate { get; set; }
        public int QuotationStatus { get; set; }
        public decimal Total { get; set; }
        public string Notes { get; set; }
        public decimal Discount { get; set; } = 0;
        public decimal GrandTotal { get; set; }
        public ICollection<QuotationItemDto> Items { get; set; }
    }
}
