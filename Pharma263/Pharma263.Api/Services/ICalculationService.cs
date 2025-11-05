using Pharma263.Api.Models.Calculations.Request;
using Pharma263.Api.Models.Calculations.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pharma263.Api.Services
{
    public interface ICalculationService
    {
        /// <summary>
        /// Calculate sales totals with pharmaceutical business rules
        /// </summary>
        Task<CalculationResult> CalculateSalesTotalAsync(List<CalculationItem> items);

        /// <summary>
        /// Calculate purchase totals with supplier-specific rules
        /// </summary>
        Task<CalculationResult> CalculatePurchaseTotalAsync(List<CalculationItem> items);

        /// <summary>
        /// Calculate and validate discount amounts
        /// </summary>
        Task<DiscountResult> CalculateDiscountAsync(decimal subtotal, decimal discountPercent, decimal discountAmount, string discountType);

        /// <summary>
        /// Validate stock quantities with business rules
        /// </summary>
        Task<StockValidationResult> ValidateStockQuantityAsync(int stockId, int requestedQuantity, List<ExistingOrder> existingOrders);

        /// <summary>
        /// Calculate pricing with volume discounts and customer-specific rules
        /// </summary>
        Task<PricingResult> CalculatePricingAsync(int medicineId, int? customerId, int quantity, decimal basePrice);

        /// <summary>
        /// Calculate taxes and regulatory fees
        /// </summary>
        Task<TaxResult> CalculateTaxesAsync(List<CalculationItem> items, int? customerId, string transactionType);

        /// <summary>
        /// Validate pharmaceutical business rules and compliance
        /// </summary>
        Task<PharmaceuticalValidationResult> ValidatePharmaceuticalRulesAsync(List<CalculationItem> items, int? customerId, string transactionType);

        /// <summary>
        /// Real-time item calculation for dynamic forms
        /// </summary>
        Task<ItemCalculationResult> CalculateItemTotalAsync(decimal price, int quantity, decimal discountPercent, decimal discountAmount, string discountType);
    }
}