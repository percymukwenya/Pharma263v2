using Pharma263.Api.Models.Calculations.Request;
using Pharma263.Api.Models.Calculations.Response;
using Pharma263.Application.Contracts.Logging;
using Pharma263.Domain.Common;
using Pharma263.Domain.Entities;
using Pharma263.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharma263.Api.Services
{
    public class CalculationService : ICalculationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly DapperContext _context;
        private readonly IAppLogger<CalculationService> _logger;

        public CalculationService(IUnitOfWork unitOfWork, DapperContext context, IAppLogger<CalculationService> logger)
        {
            _unitOfWork = unitOfWork;
            _context = context;
            _logger = logger;
        }

        public async Task<CalculationResult> CalculateSalesTotalAsync(List<CalculationItem> items)
        {
            try
            {
                var result = new CalculationResult();
                var calculatedItems = new List<ItemCalculationResult>();

                foreach (var item in items)
                {
                    // Simple calculation that matches original JavaScript exactly
                    var itemSubtotal = item.Price * item.Quantity;
                    var discountAmount = item.DiscountType?.ToLower() == "percentage"
                        ? itemSubtotal * (item.DiscountPercent / 100)
                        : item.DiscountAmount;

                    // Ensure discount isn't more than subtotal (original JavaScript logic)
                    discountAmount = Math.Min(discountAmount, itemSubtotal);
                    var itemTotal = itemSubtotal - discountAmount;

                    var calculatedItem = new ItemCalculationResult
                    {
                        MedicineId = item.MedicineId,
                        Price = item.Price,
                        Quantity = item.Quantity,
                        Subtotal = Math.Round(itemSubtotal, 2),
                        DiscountAmount = Math.Round(discountAmount, 2),
                        Total = Math.Round(itemTotal, 2),
                        FinalTotal = Math.Round(itemTotal, 2)
                    };

                    calculatedItems.Add(calculatedItem);

                    result.Subtotal += calculatedItem.Subtotal;
                    result.TotalDiscount += calculatedItem.DiscountAmount;
                    result.GrandTotal += calculatedItem.Total;
                }

                // Round final totals to match JavaScript behavior
                result.Subtotal = Math.Round(result.Subtotal, 2);
                result.TotalDiscount = Math.Round(result.TotalDiscount, 2);
                result.GrandTotal = Math.Round(result.GrandTotal, 2);
                result.Items = calculatedItems;

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating sales total for {ItemCount} items", items.Count);
                throw;
            }
        }

        public async Task<CalculationResult> CalculatePurchaseTotalAsync(List<CalculationItem> items)
        {
            // For now, use same logic as sales - can be extended with purchase-specific rules
            return await CalculateSalesTotalAsync(items);
        }

        public async Task<DiscountResult> CalculateDiscountAsync(decimal subtotal, decimal discountPercent, decimal discountAmount, string discountType)
        {
            try
            {
                var result = new DiscountResult { IsValid = true };

                if (discountType?.ToLower() == "percentage")
                {
                    // Validate discount percentage
                    if (discountPercent < 0 || discountPercent > 100)
                    {
                        result.IsValid = false;
                        result.ValidationMessage = "Discount percentage must be between 0 and 100";
                        return result;
                    }

                    result.DiscountAmount = subtotal * (discountPercent / 100);
                    result.DiscountPercent = discountPercent;
                }
                else
                {
                    // Fixed amount discount
                    if (discountAmount < 0)
                    {
                        result.IsValid = false;
                        result.ValidationMessage = "Discount amount cannot be negative";
                        return result;
                    }

                    result.DiscountAmount = discountAmount;
                    result.DiscountPercent = subtotal > 0 ? (discountAmount / subtotal * 100) : 0;
                }

                // Ensure discount isn't more than subtotal (matches original JavaScript logic)
                result.DiscountAmount = Math.Min(result.DiscountAmount, subtotal);
                result.FinalTotal = subtotal - result.DiscountAmount;

                return await Task.FromResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating discount for subtotal {Subtotal} with {DiscountType} discount", subtotal, discountType);
                throw;
            }
        }

        public async Task<StockValidationResult> ValidateStockQuantityAsync(int stockId, int requestedQuantity, List<ExistingOrder> existingOrders)
        {
            try
            {
                var stock = await _unitOfWork.Repository<Stock>().GetByIdAsync(stockId);
                if (stock == null)
                {
                    return new StockValidationResult
                    {
                        IsValid = false,
                        Error = "Stock item not found",
                        ValidationMessage = "Stock item not found"
                    };
                }

                // Calculate reserved quantity from existing orders
                var reservedQuantity = existingOrders?.Where(o => o.StockId == stockId)
                    .Sum(o => o.Quantity) ?? 0;

                var availableQuantity = stock.TotalQuantity - reservedQuantity;

                return new StockValidationResult
                {
                    IsValid = requestedQuantity <= availableQuantity,
                    AvailableQuantity = (int)availableQuantity,
                    RequestedQuantity = requestedQuantity,
                    ReservedQuantity = reservedQuantity,
                    ValidationMessage = requestedQuantity <= availableQuantity
                        ? "Stock quantity is available"
                        : $"Insufficient stock. Available: {availableQuantity}, Requested: {requestedQuantity}"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating stock quantity for StockId {StockId}, RequestedQuantity {RequestedQuantity}", stockId, requestedQuantity);
                return new StockValidationResult
                {
                    IsValid = false,
                    Error = "Error validating stock",
                    ValidationMessage = "Unable to validate stock quantity"
                };
            }
        }

        public async Task<PricingResult> CalculatePricingAsync(int medicineId, int? customerId, int quantity, decimal basePrice)
        {
            try
            {
                // Simple pricing that returns base price (matches original behavior)
                // Advanced discounts can be applied via separate discount calculations
                var result = new PricingResult
                {
                    FinalPrice = basePrice,
                    VolumeDiscount = 0,
                    CustomerDiscount = 0,
                    AppliedDiscounts = new List<string>()
                };

                return await Task.FromResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating pricing for MedicineId {MedicineId}, CustomerId {CustomerId}, Quantity {Quantity}", medicineId, customerId, quantity);
                return new PricingResult
                {
                    FinalPrice = basePrice,
                    VolumeDiscount = 0,
                    CustomerDiscount = 0,
                    AppliedDiscounts = new List<string>()
                };
            }
        }

        public async Task<TaxResult> CalculateTaxesAsync(List<CalculationItem> items, int? customerId, string transactionType)
        {
            try
            {
                var result = new TaxResult
                {
                    TaxBreakdown = new List<TaxBreakdown>()
                };

                var customer = customerId.HasValue
                    ? await _unitOfWork.Repository<Customer>().GetByIdAsync(customerId.Value)
                    : null;

                // Basic tax calculation - return zero taxes to maintain original behavior
                // Advanced tax rules can be added later without breaking existing functionality
                result.TotalTax = 0;
                result.TaxableAmount = 0;
                result.ExemptAmount = 0;

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating taxes for {ItemCount} items, CustomerId {CustomerId}, TransactionType {TransactionType}", items.Count, customerId, transactionType);
                return new TaxResult
                {
                    TotalTax = 0,
                    TaxableAmount = 0,
                    ExemptAmount = 0,
                    TaxBreakdown = new List<TaxBreakdown>()
                };
            }
        }

        public async Task<PharmaceuticalValidationResult> ValidatePharmaceuticalRulesAsync(List<CalculationItem> items, int? customerId, string transactionType)
        {
            try
            {
                var result = new PharmaceuticalValidationResult
                {
                    IsValid = true,
                    Warnings = new List<string>(),
                    Errors = new List<string>(),
                    ComplianceChecks = new List<ComplianceCheck>()
                };

                foreach (var item in items)
                {
                    // Get medicine details for validation
                    var medicine = await _unitOfWork.Repository<Medicine>().GetByIdAsync(item.MedicineId);
                    if (medicine == null)
                    {
                        result.Errors.Add($"Medicine with ID {item.MedicineId} not found");
                        result.IsValid = false;
                        continue;
                    }

                    // Check for controlled substances
                    if (medicine.Name?.ToLower().Contains("codeine") == true ||
                        medicine.Name?.ToLower().Contains("morphine") == true)
                    {
                        result.ComplianceChecks.Add(new ComplianceCheck
                        {
                            Rule = "Controlled Substance Check",
                            Passed = true,
                            Message = $"Controlled substance detected: {medicine.Name}",
                            Severity = "warning"
                        });
                        result.Warnings.Add($"Controlled substance: {medicine.Name} - ensure proper documentation");
                    }

                    // Check large quantity orders
                    if (item.Quantity > 1000)
                    {
                        result.Warnings.Add($"Large quantity order detected: {item.Quantity} units of {medicine.Name}");
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating pharmaceutical rules for {ItemCount} items, CustomerId {CustomerId}, TransactionType {TransactionType}", items.Count, customerId, transactionType);
                return new PharmaceuticalValidationResult
                {
                    IsValid = false,
                    Errors = new List<string> { "Error validating pharmaceutical rules" },
                    Warnings = new List<string>(),
                    ComplianceChecks = new List<ComplianceCheck>()
                };
            }
        }

        public async Task<ItemCalculationResult> CalculateItemTotalAsync(decimal price, int quantity, decimal discountPercent, decimal discountAmount, string discountType)
        {
            try
            {
                var result = new ItemCalculationResult
                {
                    Price = price,
                    Quantity = quantity,
                    Subtotal = price * quantity
                };

                if (discountType?.ToLower() == "percentage")
                {
                    result.DiscountAmount = result.Subtotal * (discountPercent / 100);
                }
                else
                {
                    result.DiscountAmount = discountAmount;
                }

                // Ensure discount isn't more than subtotal (matches original JavaScript logic)
                result.DiscountAmount = Math.Min(result.DiscountAmount, result.Subtotal);

                result.Total = result.Subtotal - result.DiscountAmount;
                result.FinalTotal = result.Total;

                return await Task.FromResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating item total for Price {Price}, Quantity {Quantity}, DiscountType {DiscountType}", price, quantity, discountType);
                throw;
            }
        }
    }
}