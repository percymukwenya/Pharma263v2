using System;
using System.ComponentModel.DataAnnotations;

namespace Pharma263.Api.Models.Purchase
{
    public class PurchaseItemModel
    {
        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than or equal to 0")]
        public decimal Price { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public int Quantity { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Amount must be greater than or equal to 0")]
        public decimal Amount { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Discount must be greater than or equal to 0")]
        public decimal Discount { get; set; }

        [Required(ErrorMessage = "Medicine is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid medicine")]
        public int MedicineId { get; set; }

        [Required(ErrorMessage = "Medicine name is required")]
        [StringLength(100, ErrorMessage = "Medicine name cannot exceed 100 characters")]
        public string MedicineName { get; set; }

        [Required(ErrorMessage = "Batch number is required")]
        [StringLength(50, ErrorMessage = "Batch number cannot exceed 50 characters")]
        public string BatchNo { get; set; }

        [Required(ErrorMessage = "Expiry date is required")]
        public DateTimeOffset ExpiryDate { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Buying price must be greater than or equal to 0")]
        public decimal BuyingPrice { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Selling price must be greater than or equal to 0")]
        public decimal SellingPrice { get; set; }
    }
}
