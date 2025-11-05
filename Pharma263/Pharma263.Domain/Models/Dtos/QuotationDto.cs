using System;
using System.Collections.Generic;

namespace Pharma263.Domain.Models.Dtos
{
    public class QuotationDto
    {
        public int Id { get; set; }
        public DateTimeOffset QuotationDate { get; set; }
        public string Notes { get; set; }
        public double Total { get; set; }
        public double Discount { get; set; }
        public double GrandTotal { get; set; }
        public string QuotationStatus { get; set; }
        public ICollection<GetQuotationItemDto> Items { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerAddress { get; set; }
    }
}
