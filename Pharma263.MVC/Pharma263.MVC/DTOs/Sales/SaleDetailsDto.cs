using System;
using System.Collections.Generic;

namespace Pharma263.MVC.DTOs.Sales
{
    public class SaleDetailsDto
    {
        public int Id { get; set; }
        public DateTimeOffset SalesDate { get; set; }
        public string Notes { get; set; }
        public double Total { get; set; }
        public string SaleStatus { get; set; }
        public int SaleStatusId { get; set; }
        public double Discount { get; set; }
        public double GrandTotal { get; set; }
        public string PaymentMethod { get; set; }
        public int PaymentMethodId { get; set; }
        public string CustomerName { get; set; }
        public int CustomerId { get; set; }

        public ICollection<GetSalesItemDto> Items { get; set; }
    }
}
