using System.Collections.Generic;
using System;

namespace Pharma263.MVC.DTOs.Quotation
{
    public class QuotationDetailsDto
    {
        public int Id { get; set; }
        public DateTimeOffset QuotationDate { get; set; }
        public string Notes { get; set; }
        public double Total { get; set; }
        public int QuotationStatusId { get; set; }
        public double Discount { get; set; }
        public double GrandTotal { get; set; }
        public string CustomerName { get; set; }
        public int CustomerId { get; set; }
        public ICollection<GetQuotationItemDto> Items { get; set; }
    }
}
