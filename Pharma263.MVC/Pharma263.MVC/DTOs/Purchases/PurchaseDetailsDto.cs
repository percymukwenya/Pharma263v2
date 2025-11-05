using System;
using System.Collections.Generic;

namespace Pharma263.MVC.DTOs.Purchases
{
    public class PurchaseDetailsDto
    {
        public int Id { get; set; }
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
        public List<PurchaseItemsDto> Items { get; set; }

        public DateTimeOffset CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
