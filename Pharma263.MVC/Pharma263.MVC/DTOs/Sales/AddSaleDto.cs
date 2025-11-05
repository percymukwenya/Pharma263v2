using System;
using System.Collections.Generic;

namespace Pharma263.MVC.DTOs.Sales
{
    public class AddSaleDto
    {
        public int CustomerId { get; set; }
        public DateTime SalesDate { get; set; }
        public int PaymentMethodId { get; set; }
        public decimal Total { get; set; }
        public int SaleStatusId { get; set; }
        public string Notes { get; set; }
        public decimal Discount { get; set; } = 0;
        public decimal GrandTotal { get; set; }
        public List<SalesItemDto> Items { get; set; }
    }
}
