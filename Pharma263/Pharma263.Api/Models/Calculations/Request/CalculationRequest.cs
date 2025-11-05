using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pharma263.Api.Models.Calculations.Request
{
    public class CalculationRequest
    {
        [Required]
        public  List<CalculationItem> Items { get; set; } = new();
    }

    public class CalculationItem
    {
        [Required]
        public int MedicineId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public int Quantity { get; set; }

        [Range(0, 100, ErrorMessage = "Discount percent must be between 0 and 100")]
        public decimal DiscountPercent { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Discount amount must be non-negative")]
        public decimal DiscountAmount { get; set; }

        public string DiscountType { get; set; } = "percentage";
    }

    public class DiscountRequest
    {
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Subtotal must be greater than 0")]
        public decimal Subtotal { get; set; }

        [Range(0, 100, ErrorMessage = "Discount percent must be between 0 and 100")]
        public decimal DiscountPercent { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Discount amount must be non-negative")]
        public decimal DiscountAmount { get; set; }

        public string DiscountType { get; set; } = "percentage";
    }

    public class StockValidationRequest
    {
        [Required]
        public int StockId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Requested quantity must be greater than 0")]
        public int RequestedQuantity { get; set; }

        public List<ExistingOrder> ExistingOrders { get; set; } = new();
    }

    public class ExistingOrder
    {
        [Required]
        public int StockId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public int Quantity { get; set; }
    }

    public class PricingRequest
    {
        [Required]
        public int MedicineId { get; set; }

        public int? CustomerId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public int Quantity { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Base price must be greater than 0")]
        public decimal BasePrice { get; set; }
    }

    public class TaxRequest
    {
        [Required]
        public List<CalculationItem> Items { get; set; } = new();

        public int? CustomerId { get; set; }

        public string TransactionType { get; set; } = "sale";
    }

    public class PharmaceuticalValidationRequest
    {
        [Required]
        public List<CalculationItem> Items { get; set; } = new();

        public int? CustomerId { get; set; }

        public string TransactionType { get; set; } = "sale";
    }

    public class ItemCalculationRequest
    {
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public int Quantity { get; set; }

        [Range(0, 100, ErrorMessage = "Discount percent must be between 0 and 100")]
        public decimal DiscountPercent { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Discount amount must be non-negative")]
        public decimal DiscountAmount { get; set; }

        public string DiscountType { get; set; } = "percentage";
    }
}