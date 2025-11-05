using System;
using System.Collections.Generic;

namespace Pharma263.Integration.Api.Models.Request
{
    public class CreatePurchaseRequest
    {
        public DateTime PurchaseDate { get; set; }
        public string Notes { get; set; }
        public double Total { get; set; }
        public int PaymentMethodId { get; set; }
        public int PurchaseStatusId { get; set; }
        public double Discount { get; set; }
        public double GrandTotal { get; set; }
        public int SupplierId { get; set; }
        public List<PurchaseItemModel> Items { get; set; }
    }
}
