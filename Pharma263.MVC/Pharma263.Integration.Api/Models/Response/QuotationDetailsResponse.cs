using System;
using System.Collections.Generic;

namespace Pharma263.Integration.Api.Models.Response
{
    public class QuotationDetailsResponse
    {
        public DateTimeOffset QuotationDate { get; set; }
        public string Notes { get; set; }
        public double Total { get; set; }
        public double Discount { get; set; }
        public double GrandTotal { get; set; }
        public int QuotationStatusId { get; set; }
        public ICollection<QuotationItemModel> Items { get; set; }
        public string CustomerName { get; set; }
        public int CustomerId { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerAddress { get; set; }
    }
}
