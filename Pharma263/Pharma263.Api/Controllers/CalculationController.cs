using Microsoft.AspNetCore.Mvc;
using Pharma263.Api.Extensions;
using Pharma263.Api.Models.Calculations.Request;
using Pharma263.Api.Models.Calculations.Response;
using Pharma263.Api.Services;
using Pharma263.Domain.Common;
using System;
using System.Threading.Tasks;

namespace Pharma263.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculationController : ControllerBase
    {
        private readonly ICalculationService _calculationService;

        public CalculationController(ICalculationService calculationService)
        {
            _calculationService = calculationService;
        }

        /// <summary>
        /// Calculate sales totals with pharmaceutical business rules
        /// </summary>
        [HttpPost("sales-total")]
        [ProducesResponseType(typeof(ApiResponse<CalculationResult>), 200)]
        [ProducesResponseType(typeof(ApiResponse<CalculationResult>), 400)]
        public async Task<ActionResult<ApiResponse<CalculationResult>>> CalculateSalesTotal([FromBody] CalculationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponseExtensions.CreateValidationFailure<CalculationResult>(
                    "Invalid calculation request", ModelState));
            }

            try
            {
                var result = await _calculationService.CalculateSalesTotalAsync(request.Items);
                return Ok(ApiResponse<CalculationResult>.CreateSuccess(result, "Sales total calculated successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<CalculationResult>.CreateFailure(
                    $"Sales calculation failed: {ex.Message}"));
            }
        }

        /// <summary>
        /// Calculate purchase totals with supplier-specific rules
        /// </summary>
        [HttpPost("purchase-total")]
        [ProducesResponseType(typeof(ApiResponse<CalculationResult>), 200)]
        [ProducesResponseType(typeof(ApiResponse<CalculationResult>), 400)]
        public async Task<ActionResult<ApiResponse<CalculationResult>>> CalculatePurchaseTotal([FromBody] CalculationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponseExtensions.CreateValidationFailure<CalculationResult>(
                    "Invalid calculation request", ModelState));
            }

            try
            {
                var result = await _calculationService.CalculatePurchaseTotalAsync(request.Items);
                return Ok(ApiResponse<CalculationResult>.CreateSuccess(result, "Purchase total calculated successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<CalculationResult>.CreateFailure(
                    $"Purchase calculation failed: {ex.Message}"));
            }
        }

        /// <summary>
        /// Calculate and validate discount amounts
        /// </summary>
        [HttpPost("discount")]
        [ProducesResponseType(typeof(ApiResponse<DiscountResult>), 200)]
        [ProducesResponseType(typeof(ApiResponse<DiscountResult>), 400)]
        public async Task<ActionResult<ApiResponse<DiscountResult>>> CalculateDiscount([FromBody] DiscountRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponseExtensions.CreateValidationFailure<DiscountResult>(
                    "Invalid discount request", ModelState));
            }

            try
            {
                var result = await _calculationService.CalculateDiscountAsync(
                    request.Subtotal,
                    request.DiscountPercent,
                    request.DiscountAmount,
                    request.DiscountType
                );

                return Ok(ApiResponse<DiscountResult>.CreateSuccess(result, "Discount calculated successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<DiscountResult>.CreateFailure(
                    $"Discount calculation failed: {ex.Message}"));
            }
        }

        /// <summary>
        /// Validate stock quantities with business rules
        /// </summary>
        [HttpPost("stock-validation")]
        [ProducesResponseType(typeof(ApiResponse<StockValidationResult>), 200)]
        [ProducesResponseType(typeof(ApiResponse<StockValidationResult>), 400)]
        public async Task<ActionResult<ApiResponse<StockValidationResult>>> ValidateStockQuantity([FromBody] StockValidationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponseExtensions.CreateValidationFailure<StockValidationResult>(
                    "Invalid stock validation request", ModelState));
            }

            try
            {
                var result = await _calculationService.ValidateStockQuantityAsync(
                    request.StockId,
                    request.RequestedQuantity,
                    request.ExistingOrders
                );

                return Ok(ApiResponse<StockValidationResult>.CreateSuccess(result, "Stock validation completed"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<StockValidationResult>.CreateFailure(
                    $"Stock validation failed: {ex.Message}"));
            }
        }

        /// <summary>
        /// Calculate pricing with volume discounts and customer-specific rules
        /// </summary>
        [HttpPost("pricing")]
        [ProducesResponseType(typeof(ApiResponse<PricingResult>), 200)]
        [ProducesResponseType(typeof(ApiResponse<PricingResult>), 400)]
        public async Task<ActionResult<ApiResponse<PricingResult>>> CalculatePricing([FromBody] PricingRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponseExtensions.CreateValidationFailure<PricingResult>(
                    "Invalid pricing request", ModelState));
            }

            try
            {
                var result = await _calculationService.CalculatePricingAsync(
                    request.MedicineId,
                    request.CustomerId,
                    request.Quantity,
                    request.BasePrice
                );

                return Ok(ApiResponse<PricingResult>.CreateSuccess(result, "Pricing calculated successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<PricingResult>.CreateFailure(
                    $"Pricing calculation failed: {ex.Message}"));
            }
        }

        /// <summary>
        /// Calculate taxes and regulatory fees
        /// </summary>
        [HttpPost("taxes")]
        [ProducesResponseType(typeof(ApiResponse<TaxResult>), 200)]
        [ProducesResponseType(typeof(ApiResponse<TaxResult>), 400)]
        public async Task<ActionResult<ApiResponse<TaxResult>>> CalculateTaxes([FromBody] TaxRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponseExtensions.CreateValidationFailure<TaxResult>(
                    "Invalid tax calculation request", ModelState));
            }

            try
            {
                var result = await _calculationService.CalculateTaxesAsync(
                    request.Items,
                    request.CustomerId,
                    request.TransactionType
                );

                return Ok(ApiResponse<TaxResult>.CreateSuccess(result, "Tax calculation completed"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<TaxResult>.CreateFailure(
                    $"Tax calculation failed: {ex.Message}"));
            }
        }

        /// <summary>
        /// Validate pharmaceutical business rules and compliance
        /// </summary>
        [HttpPost("pharmaceutical-validation")]
        [ProducesResponseType(typeof(ApiResponse<PharmaceuticalValidationResult>), 200)]
        [ProducesResponseType(typeof(ApiResponse<PharmaceuticalValidationResult>), 400)]
        public async Task<ActionResult<ApiResponse<PharmaceuticalValidationResult>>> ValidatePharmaceuticalRules([FromBody] PharmaceuticalValidationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponseExtensions.CreateValidationFailure<PharmaceuticalValidationResult>(
                    "Invalid pharmaceutical validation request", ModelState));
            }

            try
            {
                var result = await _calculationService.ValidatePharmaceuticalRulesAsync(
                    request.Items,
                    request.CustomerId,
                    request.TransactionType
                );

                return Ok(ApiResponse<PharmaceuticalValidationResult>.CreateSuccess(result, "Pharmaceutical validation completed"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<PharmaceuticalValidationResult>.CreateFailure(
                    $"Pharmaceutical validation failed: {ex.Message}"));
            }
        }

        /// <summary>
        /// Real-time item calculation for dynamic forms
        /// </summary>
        [HttpPost("item-calculation")]
        [ProducesResponseType(typeof(ApiResponse<ItemCalculationResult>), 200)]
        [ProducesResponseType(typeof(ApiResponse<ItemCalculationResult>), 400)]
        public async Task<ActionResult<ApiResponse<ItemCalculationResult>>> CalculateItemTotal([FromBody] ItemCalculationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponseExtensions.CreateValidationFailure<ItemCalculationResult>(
                    "Invalid item calculation request", ModelState));
            }

            try
            {
                var result = await _calculationService.CalculateItemTotalAsync(
                    request.Price,
                    request.Quantity,
                    request.DiscountPercent,
                    request.DiscountAmount,
                    request.DiscountType
                );

                return Ok(ApiResponse<ItemCalculationResult>.CreateSuccess(result, "Item calculation completed"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<ItemCalculationResult>.CreateFailure(
                    $"Item calculation failed: {ex.Message}"));
            }
        }
    }
}