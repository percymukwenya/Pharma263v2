using Microsoft.AspNetCore.Mvc;
using Pharma263.MVC.Models.Calculations;
using Pharma263.MVC.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pharma263.MVC.Controllers
{
    [Route("api/calculations")]
    [ApiController]
    public class CalculationsController : ControllerBase
    {
        private readonly ICalculationService _calculationService;

        public CalculationsController(ICalculationService calculationService)
        {
            _calculationService = calculationService;
        }

        /// <summary>
        /// Calculate sales totals with pharmaceutical business rules
        /// </summary>
        [HttpPost("sales-total")]
        public async Task<ActionResult<CalculationResult>> CalculateSalesTotal([FromBody] CalculationRequest request)
        {
            try
            {
                var result = await _calculationService.CalculateSalesTotalAsync(request.Items);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Calculation failed", error = ex.Message });
            }
        }

        /// <summary>
        /// Calculate purchase totals with supplier-specific rules
        /// </summary>
        [HttpPost("purchase-total")]
        public async Task<ActionResult<CalculationResult>> CalculatePurchaseTotal([FromBody] CalculationRequest request)
        {
            try
            {
                var result = await _calculationService.CalculatePurchaseTotalAsync(request.Items);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Calculation failed", error = ex.Message });
            }
        }

        /// <summary>
        /// Calculate and validate discount amounts
        /// </summary>
        [HttpPost("discount")]
        public async Task<ActionResult<DiscountResult>> CalculateDiscount([FromBody] DiscountRequest request)
        {
            try
            {
                var result = await _calculationService.CalculateDiscountAsync(
                    request.Subtotal, 
                    request.DiscountPercent, 
                    request.DiscountAmount, 
                    request.DiscountType
                );
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Discount calculation failed", error = ex.Message });
            }
        }

        /// <summary>
        /// Validate stock quantities with business rules
        /// </summary>
        [HttpPost("stock-validation")]
        public async Task<ActionResult<StockValidationResult>> ValidateStockQuantity([FromBody] StockValidationRequest request)
        {
            try
            {
                var result = await _calculationService.ValidateStockQuantityAsync(
                    request.StockId, 
                    request.RequestedQuantity, 
                    request.ExistingOrders
                );
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Stock validation failed", error = ex.Message });
            }
        }

        /// <summary>
        /// Calculate pricing with volume discounts and customer-specific rules
        /// </summary>
        [HttpPost("pricing")]
        public async Task<ActionResult<PricingResult>> CalculatePricing([FromBody] PricingRequest request)
        {
            try
            {
                var result = await _calculationService.CalculatePricingAsync(
                    request.MedicineId, 
                    request.CustomerId, 
                    request.Quantity, 
                    request.BasePrice
                );
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Pricing calculation failed", error = ex.Message });
            }
        }

        /// <summary>
        /// Calculate taxes and regulatory fees
        /// </summary>
        [HttpPost("taxes")]
        public async Task<ActionResult<TaxResult>> CalculateTaxes([FromBody] TaxRequest request)
        {
            try
            {
                var result = await _calculationService.CalculateTaxesAsync(
                    request.Items, 
                    request.CustomerId, 
                    request.TransactionType
                );
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Tax calculation failed", error = ex.Message });
            }
        }

        /// <summary>
        /// Validate pharmaceutical business rules and compliance
        /// </summary>
        [HttpPost("pharmaceutical-validation")]
        public async Task<ActionResult<PharmaceuticalValidationResult>> ValidatePharmaceuticalRules([FromBody] PharmaceuticalValidationRequest request)
        {
            try
            {
                var result = await _calculationService.ValidatePharmaceuticalRulesAsync(
                    request.Items, 
                    request.CustomerId, 
                    request.TransactionType
                );
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Pharmaceutical validation failed", error = ex.Message });
            }
        }

        /// <summary>
        /// Real-time item calculation for dynamic forms
        /// </summary>
        [HttpPost("item-calculation")]
        public async Task<ActionResult<ItemCalculationResult>> CalculateItemTotal([FromBody] ItemCalculationRequest request)
        {
            try
            {
                var result = await _calculationService.CalculateItemTotalAsync(
                    request.Price, 
                    request.Quantity, 
                    request.DiscountPercent, 
                    request.DiscountAmount, 
                    request.DiscountType
                );
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Item calculation failed", error = ex.Message });
            }
        }
    }
}

namespace Pharma263.MVC.Models.Calculations
{
    public class CalculationRequest
    {
        public List<CalculationItem> Items { get; set; } = new();
    }

    public class CalculationItem
    {
        public int MedicineId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal DiscountAmount { get; set; }
        public string DiscountType { get; set; } = "percentage";
    }

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
    }

    public class DiscountRequest
    {
        public decimal Subtotal { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal DiscountAmount { get; set; }
        public string DiscountType { get; set; } = "percentage";
    }

    public class DiscountResult
    {
        public decimal DiscountAmount { get; set; }
        public decimal FinalTotal { get; set; }
        public decimal DiscountPercent { get; set; }
        public bool IsValid { get; set; }
        public string ValidationMessage { get; set; } = "";
    }

    public class StockValidationRequest
    {
        public int StockId { get; set; }
        public int RequestedQuantity { get; set; }
        public List<ExistingOrder> ExistingOrders { get; set; } = new();
    }

    public class ExistingOrder
    {
        public int StockId { get; set; }
        public int Quantity { get; set; }
    }

    public class StockValidationResult
    {
        public bool IsValid { get; set; }
        public int AvailableQuantity { get; set; }
        public int RequestedQuantity { get; set; }
        public int ReservedQuantity { get; set; }
        public string ValidationMessage { get; set; } = "";
        public string Error { get; internal set; }
    }

    public class PricingRequest
    {
        public int MedicineId { get; set; }
        public int? CustomerId { get; set; }
        public int Quantity { get; set; }
        public decimal BasePrice { get; set; }
    }

    public class PricingResult
    {
        public decimal FinalPrice { get; set; }
        public decimal VolumeDiscount { get; set; }
        public decimal CustomerDiscount { get; set; }
        public List<string> AppliedDiscounts { get; set; } = new();
    }

    public class TaxRequest
    {
        public List<CalculationItem> Items { get; set; } = new();
        public int? CustomerId { get; set; }
        public string TransactionType { get; set; } = "sale";
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

    public class PharmaceuticalValidationRequest
    {
        public List<CalculationItem> Items { get; set; } = new();
        public int? CustomerId { get; set; }
        public string TransactionType { get; set; } = "sale";
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

    public class ItemCalculationRequest
    {
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal DiscountAmount { get; set; }
        public string DiscountType { get; set; } = "percentage";
    }
}

namespace Pharma263.MVC.Services
{
    public interface ICalculationService
    {
        Task<CalculationResult> CalculateSalesTotalAsync(List<CalculationItem> items);
        Task<CalculationResult> CalculatePurchaseTotalAsync(List<CalculationItem> items);
        Task<DiscountResult> CalculateDiscountAsync(decimal subtotal, decimal discountPercent, decimal discountAmount, string discountType);
        Task<StockValidationResult> ValidateStockQuantityAsync(int stockId, int requestedQuantity, List<ExistingOrder> existingOrders);
        Task<PricingResult> CalculatePricingAsync(int medicineId, int? customerId, int quantity, decimal basePrice);
        Task<TaxResult> CalculateTaxesAsync(List<CalculationItem> items, int? customerId, string transactionType);
        Task<PharmaceuticalValidationResult> ValidatePharmaceuticalRulesAsync(List<CalculationItem> items, int? customerId, string transactionType);
        Task<ItemCalculationResult> CalculateItemTotalAsync(decimal price, int quantity, decimal discountPercent, decimal discountAmount, string discountType);
    }
}