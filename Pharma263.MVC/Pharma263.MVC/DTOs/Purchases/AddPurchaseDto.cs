using System;
using System.Collections.Generic;

namespace Pharma263.MVC.DTOs.Purchases
{
    public class AddPurchaseDto
    {
        public int Id { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string Notes { get; set; }
        public double Total { get; set; }
        public int PaymentMethodId { get; set; }
        public int PurchaseStatusId { get; set; }
        public double Discount { get; set; }
        public double GrandTotal { get; set; }
        public int SupplierId { get; set; }
        public ICollection<PurchaseItemsDto> Items { get; set; }
    }
}
