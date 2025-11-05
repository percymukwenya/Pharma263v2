using System;

namespace Pharma263.Api.Models.Sales.Response
{
    public class SaleListResponse
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
        public string CustomerName { get; set; }
    }
}
