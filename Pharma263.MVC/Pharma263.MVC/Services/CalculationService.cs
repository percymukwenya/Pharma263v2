using Pharma263.MVC.DTOs;
using Pharma263.MVC.Models.Calculations;
using Pharma263.MVC.Utility;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pharma263.MVC.Services
{
    /// <summary>
    /// Service for calculation operations (sales, purchases, discounts, pricing, taxes, stock validation).
    /// Migrated from BaseService to IApiService for better performance and consistency.
    /// </summary>
    public class CalculationService : ICalculationService
    {
        private readonly IApiService _apiService;

        public CalculationService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<CalculationResult> CalculateSalesTotalAsync(List<CalculationItem> items)
        {
            var response = await _apiService.PostApiResponseAsync<CalculationResult>(
                "/api/calculation/sales-total",
                new { Items = items });
            return response.Data ?? new CalculationResult();
        }

        public async Task<CalculationResult> CalculatePurchaseTotalAsync(List<CalculationItem> items)
        {
            var response = await _apiService.PostApiResponseAsync<CalculationResult>(
                "/api/calculation/purchase-total",
                new { Items = items });
            return response.Data ?? new CalculationResult();
        }

        public async Task<DiscountResult> CalculateDiscountAsync(decimal subtotal, decimal discountPercent, decimal discountAmount, string discountType)
        {
            var response = await _apiService.PostApiResponseAsync<DiscountResult>(
                "/api/calculation/discount",
                new
                {
                    Subtotal = subtotal,
                    DiscountPercent = discountPercent,
                    DiscountAmount = discountAmount,
                    DiscountType = discountType
                });
            return response.Data ?? new DiscountResult { IsValid = false };
        }

        public async Task<StockValidationResult> ValidateStockQuantityAsync(int stockId, int requestedQuantity, List<ExistingOrder> existingOrders)
        {
            var response = await _apiService.PostApiResponseAsync<StockValidationResult>(
                "/api/calculation/stock-validation",
                new
                {
                    StockId = stockId,
                    RequestedQuantity = requestedQuantity,
                    ExistingOrders = existingOrders
                });
            return response.Data ?? new StockValidationResult
            {
                IsValid = false,
                AvailableQuantity = 0,
                RequestedQuantity = requestedQuantity,
                ValidationMessage = "Unable to validate stock",
                Error = "API response error"
            };
        }

        public async Task<PricingResult> CalculatePricingAsync(int medicineId, int? customerId, int quantity, decimal basePrice)
        {
            var response = await _apiService.PostApiResponseAsync<PricingResult>(
                "/api/calculation/pricing",
                new
                {
                    MedicineId = medicineId,
                    CustomerId = customerId,
                    Quantity = quantity,
                    BasePrice = basePrice
                });
            return response.Data ?? new PricingResult();
        }

        public async Task<TaxResult> CalculateTaxesAsync(List<CalculationItem> items, int? customerId, string transactionType)
        {
            var response = await _apiService.PostApiResponseAsync<TaxResult>(
                "/api/calculation/taxes",
                new
                {
                    Items = items,
                    CustomerId = customerId,
                    TransactionType = transactionType
                });
            return response.Data ?? new TaxResult();
        }

        public async Task<PharmaceuticalValidationResult> ValidatePharmaceuticalRulesAsync(List<CalculationItem> items, int? customerId, string transactionType)
        {
            var response = await _apiService.PostApiResponseAsync<PharmaceuticalValidationResult>(
                "/api/calculation/pharmaceutical-validation",
                new
                {
                    Items = items,
                    CustomerId = customerId,
                    TransactionType = transactionType
                });
            return response.Data ?? new PharmaceuticalValidationResult { IsValid = false };
        }

        public async Task<ItemCalculationResult> CalculateItemTotalAsync(decimal price, int quantity, decimal discountPercent, decimal discountAmount, string discountType)
        {
            var response = await _apiService.PostApiResponseAsync<ItemCalculationResult>(
                "/api/calculation/item-calculation",
                new
                {
                    Price = price,
                    Quantity = quantity,
                    DiscountPercent = discountPercent,
                    DiscountAmount = discountAmount,
                    DiscountType = discountType
                });
            return response.Data ?? new ItemCalculationResult();
        }
    }
}
