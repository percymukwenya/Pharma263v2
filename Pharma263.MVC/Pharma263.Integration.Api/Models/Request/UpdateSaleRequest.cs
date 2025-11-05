using System.Collections.Generic;
using System;

namespace Pharma263.Integration.Api.Models.Request
{
    public class UpdateSaleRequest
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime SalesDate { get; set; }
        public string Notes { get; set; }
        public decimal Total { get; set; }
        public int SaleStatusId { get; set; }
        public decimal Discount { get; set; }
        public decimal GrandTotal { get; set; }
        public int PaymentMethodId { get; set; }
        
        public List<SaleItemModel> Items { get; set; }
    }
}
