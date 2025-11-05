using System.Collections.Generic;

namespace Pharma263.Api.Models.Calculations.Response
{
    public class CalculationResult
    {
        public decimal Subtotal { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal GrandTotal { get; set; }
        public List<ItemCalculationResult> Items { get; set; } = new();
    }

    public class ItemCalculationResult
    {
        public int MedicineId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Subtotal { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal Total { get; set; }
        public decimal FinalTotal { get; set; }
    }

    public class DiscountResult
    {
        public decimal DiscountAmount { get; set; }
        public decimal FinalTotal { get; set; }
        public decimal DiscountPercent { get; set; }
        public bool IsValid { get; set; }
        public string ValidationMessage { get; set; } = "";
    }

    public class StockValidationResult
    {
        public bool IsValid { get; set; }
        public int AvailableQuantity { get; set; }
        public int RequestedQuantity { get; set; }
        public int ReservedQuantity { get; set; }
        public string ValidationMessage { get; set; } = "";
        public string Error { get; set; } = "";
    }

    public class PricingResult
    {
        public decimal FinalPrice { get; set; }
        public decimal VolumeDiscount { get; set; }
        public decimal CustomerDiscount { get; set; }
        public List<string> AppliedDiscounts { get; set; } = new();
    }

    public class TaxResult
    {
        public decimal TotalTax { get; set; }
        public decimal TaxableAmount { get; set; }
        public decimal ExemptAmount { get; set; }
        public List<TaxBreakdown> TaxBreakdown { get; set; } = new();
    }

    public class TaxBreakdown
    {
        public string TaxType { get; set; } = "";
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public decimal TaxableAmount { get; set; }
    }

    public class PharmaceuticalValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Warnings { get; set; } = new();
        public List<string> Errors { get; set; } = new();
        public List<ComplianceCheck> ComplianceChecks { get; set; } = new();
    }

    public class ComplianceCheck
    {
        public string Rule { get; set; } = "";
        public bool Passed { get; set; }
        public string Message { get; set; } = "";
        public string Severity { get; set; } = "info"; // info, warning, error
    }
}