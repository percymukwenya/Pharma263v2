using System;
using System.ComponentModel.DataAnnotations;

namespace Pharma263.Api.Models.Stocks.Request
{
    public class UpdateStockRequest
    {
        [Required(ErrorMessage = "Id is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Invalid stock ID")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Medicine is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid medicine")]
        public int MedicineId { get; set; }

        [Required(ErrorMessage = "Expiry date is required")]
        public DateTime ExpiryDate { get; set; }

        [Required(ErrorMessage = "Batch number is required")]
        [StringLength(50, ErrorMessage = "Batch number cannot exceed 50 characters")]
        public string BatchNo { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Buying price must be greater than or equal to 0")]
        public decimal BuyingPrice { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Selling price must be greater than or equal to 0")]
        public decimal SellingPrice { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Total quantity must be greater than 0")]
        public int TotalQuantity { get; set; }
    }
}
