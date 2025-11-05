using Pharma263.MVC.Enums;
using System;
using System.Collections.Generic;

namespace Pharma263.MVC.DTOs.Sales
{
    public class UpdateSaleDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime SalesDate { get; set; }
        public string Notes { get; set; }
        public double Total { get; set; }
        public int SaleStatusId { get; set; }
        public double Discount { get; set; }
        public double GrandTotal { get; set; }
        public int PaymentMethodId { get; set; }        

        public List<SalesItemDto> Items { get; set; }
    }
}
