using System;

namespace Pharma263.Api.Models.Quotation.Response
{
    public class QuotationListResponse
    {
        public int Id { get; set; }
        public DateTimeOffset QuotationDate { get; set; }
        public string Notes { get; set; }
        public double Total { get; set; }
        public double Discount { get; set; }
        public double GrandTotal { get; set; }
        public string QuotationStatus { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerAddress { get; set; }
    }
}
