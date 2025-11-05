using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pharma263.Api.Models.Purchase.Request
{
    public class AddPurchaseRequest
    {
        [Required(ErrorMessage = "Purchase date is required")]
        public DateTime PurchaseDate { get; set; }

        [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters")]
        public string Notes { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Total must be greater than or equal to 0")]
        public decimal Total { get; set; }

        [Required(ErrorMessage = "Payment method is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid payment method")]
        public int PaymentMethodId { get; set; }

        [Required(ErrorMessage = "Purchase status is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid purchase status")]
        public int PurchaseStatusId { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Discount must be greater than or equal to 0")]
        public decimal Discount { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Grand total must be greater than or equal to 0")]
        public decimal GrandTotal { get; set; }

        [Required(ErrorMessage = "Supplier is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid supplier")]
        public int SupplierId { get; set; }

        [Required(ErrorMessage = "Purchase items are required")]
        [MinLength(1, ErrorMessage = "At least one purchase item is required")]
        public List<PurchaseItemModel> Items { get; set; }
    }
}
