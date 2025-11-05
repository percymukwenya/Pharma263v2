using System;
using System.Collections.Generic;

namespace Pharma263.Domain.Models.Dtos
{
    public class SaleDto
    {
        public int Id { get; set; }
        public DateTimeOffset SalesDate { get; set; }
        public int SaleCode { get; set; }
        public string Notes { get; set; }
        public double Total { get; set; }
        public string SaleStatus { get; set; }
        public double Discount { get; set; }
        public double GrandTotal { get; set; }
        public string PaymentMethod { get; set; }
        public ICollection<GetSalesItemDto> Items { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerAddress { get; set; }
    }
}
