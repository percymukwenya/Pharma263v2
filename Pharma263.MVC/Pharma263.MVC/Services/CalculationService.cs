using Pharma263.MVC.DTOs;
using Pharma263.MVC.Models.Calculations;
using Pharma263.MVC.Utility;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Pharma263.MVC.Services
{
    public class CalculationService : BaseService, ICalculationService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly string _baseUrl;

        public CalculationService(IHttpClientFactory clientFactory, Microsoft.Extensions.Configuration.IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            _baseUrl = configuration.GetSection("ServiceUrls:PharmaApi").Value + "/api/calculation";
        }

        public async Task<CalculationResult> CalculateSalesTotalAsync(List<CalculationItem> items)
        {
            var request = new ApiRequest
            {
                ApiType = StaticDetails.ApiType.POST,
                Url = $"{_baseUrl}/sales-total",
                Data = new { Items = items }
            };

            var apiResponse = await this.SendAsync<ApiResponseWrapper<CalculationResult>>(request);
            return apiResponse?.Data ?? new CalculationResult();
        }

        public async Task<CalculationResult> CalculatePurchaseTotalAsync(List<CalculationItem> items)
        {
            var request = new ApiRequest
            {
                ApiType = StaticDetails.ApiType.POST,
                Url = $"{_baseUrl}/purchase-total",
                Data = new { Items = items }
            };

            var apiResponse = await this.SendAsync<ApiResponseWrapper<CalculationResult>>(request);
            return apiResponse?.Data ?? new CalculationResult();
        }

        public async Task<DiscountResult> CalculateDiscountAsync(decimal subtotal, decimal discountPercent, decimal discountAmount, string discountType)
        {
            var request = new ApiRequest
            {
                ApiType = StaticDetails.ApiType.POST,
                Url = $"{_baseUrl}/discount",
                Data = new
                {
                    Subtotal = subtotal,
                    DiscountPercent = discountPercent,
                    DiscountAmount = discountAmount,
                    DiscountType = discountType
                }
            };

            var apiResponse = await this.SendAsync<ApiResponseWrapper<DiscountResult>>(request);
            return apiResponse?.Data ?? new DiscountResult { IsValid = false };
        }

        public async Task<StockValidationResult> ValidateStockQuantityAsync(int stockId, int requestedQuantity, List<ExistingOrder> existingOrders)
        {
            var request = new ApiRequest
            {
                ApiType = StaticDetails.ApiType.POST,
                Url = $"{_baseUrl}/stock-validation",
                Data = new
                {
                    StockId = stockId,
                    RequestedQuantity = requestedQuantity,
                    ExistingOrders = existingOrders
                }
            };

            // API returns ApiResponse<StockValidationResult>, so we need to unwrap it
            var apiResponse = await SendAsync<ApiResponseWrapper<StockValidationResult>>(request);
            return apiResponse?.Data ?? new StockValidationResult
            {
                IsValid = false,
                AvailableQuantity = 0,
                RequestedQuantity = requestedQuantity,
                ValidationMessage = "Unable to validate stock",
                Error = "API response error"
            };
        }

        // Helper class to match API response structure
        private class ApiResponseWrapper<T>
        {
            public T Data { get; set; }
            public bool IsSuccess { get; set; }
            public string Message { get; set; }
        }

        public async Task<PricingResult> CalculatePricingAsync(int medicineId, int? customerId, int quantity, decimal basePrice)
        {
            var request = new ApiRequest
            {
                ApiType = StaticDetails.ApiType.POST,
                Url = $"{_baseUrl}/pricing",
                Data = new
                {
                    MedicineId = medicineId,
                    CustomerId = customerId,
                    Quantity = quantity,
                    BasePrice = basePrice
                }
            };

            var apiResponse = await this.SendAsync<ApiResponseWrapper<PricingResult>>(request);
            return apiResponse?.Data ?? new PricingResult();
        }

        public async Task<TaxResult> CalculateTaxesAsync(List<CalculationItem> items, int? customerId, string transactionType)
        {
            var request = new ApiRequest
            {
                ApiType = StaticDetails.ApiType.POST,
                Url = $"{_baseUrl}/taxes",
                Data = new
                {
                    Items = items,
                    CustomerId = customerId,
                    TransactionType = transactionType
                }
            };

            var apiResponse = await this.SendAsync<ApiResponseWrapper<TaxResult>>(request);
            return apiResponse?.Data ?? new TaxResult();
        }

        public async Task<PharmaceuticalValidationResult> ValidatePharmaceuticalRulesAsync(List<CalculationItem> items, int? customerId, string transactionType)
        {
            var request = new ApiRequest
            {
                ApiType = StaticDetails.ApiType.POST,
                Url = $"{_baseUrl}/pharmaceutical-validation",
                Data = new
                {
                    Items = items,
                    CustomerId = customerId,
                    TransactionType = transactionType
                }
            };

            var apiResponse = await this.SendAsync<ApiResponseWrapper<PharmaceuticalValidationResult>>(request);
            return apiResponse?.Data ?? new PharmaceuticalValidationResult { IsValid = false };
        }

        public async Task<ItemCalculationResult> CalculateItemTotalAsync(decimal price, int quantity, decimal discountPercent, decimal discountAmount, string discountType)
        {
            var request = new ApiRequest
            {
                ApiType = StaticDetails.ApiType.POST,
                Url = $"{_baseUrl}/item-calculation",
                Data = new
                {
                    Price = price,
                    Quantity = quantity,
                    DiscountPercent = discountPercent,
                    DiscountAmount = discountAmount,
                    DiscountType = discountType
                }
            };

            var apiResponse = await this.SendAsync<ApiResponseWrapper<ItemCalculationResult>>(request);
            return apiResponse?.Data ?? new ItemCalculationResult();
        }
    }
}
