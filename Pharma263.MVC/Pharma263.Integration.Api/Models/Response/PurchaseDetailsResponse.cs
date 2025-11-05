using System.Collections.Generic;
using System;

namespace Pharma263.Integration.Api.Models.Response
{
    public class PurchaseDetailsResponse
    {
        public int Id { get; set; }
        public int PurchaseCode { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string Notes { get; set; }
        public double Total { get; set; }
        public string PaymentMethod { get; set; }
        public int PaymentMethodId { get; set; }
        public string PurchaseStatus { get; set; }
        public int PurchaseStatusId { get; set; }
        public double Discount { get; set; }
        public double GrandTotal { get; set; }
        public string Supplier { get; set; }
        public int SupplierId { get; set; }
        public string SupplierPhoneNumber { get; set; }
        public string SupplierAddress { get; set; }
        public ICollection<PurchaseItemModel> Items { get; set; }
    }
}
