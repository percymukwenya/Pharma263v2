using System;
using System.Collections.Generic;

namespace Pharma263.Domain.Models.Dtos
{
    public class PurchaseDto
    {
        public int Id { get; set; }
        public int PurchaseCode { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string Notes { get; set; }
        public double Total { get; set; }
        public string PaymentMethod { get; set; }
        public string PurchaseStatus { get; set; }
        public double Discount { get; set; }
        public double GrandTotal { get; set; }

        public ICollection<PurchaseItemsDto> Items { get; set; }
        public string Supplier { get; set; }
        public string SupplierPhoneNumber { get; set; }
        public string SupplierAddress { get; set; }
    }
}
