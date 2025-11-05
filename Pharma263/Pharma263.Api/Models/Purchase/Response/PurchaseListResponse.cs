using System;

namespace Pharma263.Api.Models.Purchase.Response
{
    public class PurchaseListResponse
    {
        public int Id { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string Notes { get; set; }
        public double Total { get; set; }
        public string PaymentMethod { get; set; }
        public string PurchaseStatus { get; set; }
        public double Discount { get; set; }
        public double GrandTotal { get; set; }
        public string Supplier { get; set; }
        public string SupplierPhoneNumber { get; set; }
        public string SupplierAddress { get; set; }
    }
}
