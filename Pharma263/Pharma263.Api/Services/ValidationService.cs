using Microsoft.EntityFrameworkCore;
using Pharma263.Api.Contracts;
using Pharma263.Api.Models.Purchase.Request;
using Pharma263.Api.Models.Quotation.Request;
using Pharma263.Api.Models.Returns.Request;
using Pharma263.Api.Models.Sales.Request;
using Pharma263.Api.Models.Validation;
using Pharma263.Api.Shared.Contracts;
using Pharma263.Application.Contracts.Logging;
using Pharma263.Domain.Common;
using Pharma263.Domain.Entities;
using Pharma263.Domain.Interfaces.Repository;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Pharma263.Api.Services
{
    public class ValidationService : IValidationService, IScopedInjectedService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAppLogger<ValidationService> _logger;
        private readonly ISalesRepository _salesRepository;
        private readonly IPurchaseRepository _purchaseRepository;

        public ValidationService(
            IUnitOfWork unitOfWork,
            ISalesRepository salesRepository,
            IPurchaseRepository purchaseRepository,
            IAppLogger<ValidationService> logger)
        {
            _unitOfWork = unitOfWork;
            _salesRepository = salesRepository;
            _purchaseRepository = purchaseRepository;
            _logger = logger;
        }

        #region Entity Existence Validation

        public async Task<ValidationResult> ValidateCustomerExistsAsync(int customerId)
        {
            var customer = await _unitOfWork.Repository<Customer>().GetByIdAsync(customerId);
            if (customer == null)
            {
                _logger.LogWarning("Customer validation failed: CustomerId={CustomerId} not found", customerId);
                return ValidationResult.Failure($"Customer with ID {customerId} not found");
            }
            return ValidationResult.Success();
        }

        public async Task<ValidationResult> ValidateSupplierExistsAsync(int supplierId)
        {
            var supplier = await _unitOfWork.Repository<Supplier>().GetByIdAsync(supplierId);
            if (supplier == null)
            {
                _logger.LogWarning("Supplier validation failed: SupplierId={SupplierId} not found", supplierId);
                return ValidationResult.Failure($"Supplier with ID {supplierId} not found");
            }
            return ValidationResult.Success();
        }

        public async Task<ValidationResult> ValidateMedicineExistsAsync(int medicineId)
        {
            var medicine = await _unitOfWork.Repository<Medicine>().GetByIdAsync(medicineId);
            if (medicine == null)
            {
                _logger.LogWarning("Medicine validation failed: MedicineId={MedicineId} not found", medicineId);
                return ValidationResult.Failure($"Medicine with ID {medicineId} not found");
            }
            return ValidationResult.Success();
        }

        public async Task<ValidationResult> ValidateStockExistsAsync(int stockId)
        {
            var stock = await _unitOfWork.Repository<Stock>().GetByIdAsync(stockId);
            if (stock == null)
            {
                _logger.LogWarning("Stock validation failed: StockId={StockId} not found", stockId);
                return ValidationResult.Failure($"Stock with ID {stockId} not found");
            }
            return ValidationResult.Success();
        }

        public async Task<ValidationResult> ValidateSaleExistsAsync(int saleId)
        {
            var sale = await _unitOfWork.Repository<Sales>().GetByIdAsync(saleId);
            if (sale == null)
            {
                _logger.LogWarning("Sale validation failed: SaleId={SaleId} not found", saleId);
                return ValidationResult.Failure($"Sale with ID {saleId} not found");
            }
            return ValidationResult.Success();
        }

        #endregion

        #region Financial Validation

        public ValidationResult ValidateDiscount(decimal itemTotal, decimal discount, string itemDescription = "Item")
        {
            if (discount < 0)
            {
                return ValidationResult.Failure($"{itemDescription}: Discount cannot be negative");
            }

            if (discount > itemTotal)
            {
                return ValidationResult.Failure($"{itemDescription}: Discount {discount:C} cannot exceed item total {itemTotal:C}");
            }

            return ValidationResult.Success();
        }

        public ValidationResult ValidatePrice(decimal price, string fieldName = "Price")
        {
            if (price < 0)
            {
                return ValidationResult.Failure($"{fieldName} cannot be negative");
            }

            if (price == 0)
            {
                _logger.LogWarning("Price validation warning: {FieldName} is zero", fieldName);
            }

            return ValidationResult.Success();
        }

        public ValidationResult ValidateQuantity(int quantity, string fieldName = "Quantity")
        {
            if (quantity <= 0)
            {
                return ValidationResult.Failure($"{fieldName} must be greater than zero");
            }

            return ValidationResult.Success();
        }

        #endregion

        #region Stock Validation

        public async Task<ValidationResult> ValidateStockAvailabilityAsync(int stockId, int requestedQuantity)
        {
            var stock = await _unitOfWork.Repository<Stock>().GetByIdAsync(stockId);
            if (stock == null)
            {
                return ValidationResult.Failure($"Stock with ID {stockId} not found");
            }

            if (stock.TotalQuantity < requestedQuantity)
            {
                _logger.LogWarning("Insufficient stock: StockId={StockId}, Available={Available}, Requested={Requested}",
                    stockId, stock.TotalQuantity, requestedQuantity);
                return ValidationResult.Failure($"Insufficient stock for StockId {stockId}. Available: {stock.TotalQuantity}, Requested: {requestedQuantity}");
            }

            return ValidationResult.Success();
        }

        #endregion

        #region Sale Request Validation

        public async Task<ValidationResult> ValidateSaleRequestAsync(AddSaleRequest request)
        {
            var result = new ValidationResult();

            // Validate customer exists
            var customerValidation = await ValidateCustomerExistsAsync(request.CustomerId);
            result.Merge(customerValidation);

            // Validate items
            if (request.Items == null || !request.Items.Any())
            {
                result.AddError("Sale must have at least one item");
                return result;
            }

            // Validate each item
            foreach (var item in request.Items)
            {
                // Validate quantity
                var quantityValidation = ValidateQuantity(item.Quantity, $"Item {item.StockId} quantity");
                result.Merge(quantityValidation);

                // Validate price
                var priceValidation = ValidatePrice(item.Price, $"Item {item.StockId} price");
                result.Merge(priceValidation);

                // Validate discount
                var itemTotal = item.Price * item.Quantity;
                var discountValidation = ValidateDiscount(itemTotal, item.Discount, $"Item {item.StockId}");
                result.Merge(discountValidation);

                // Validate stock exists and availability
                var stockValidation = await ValidateStockExistsAsync(item.StockId);
                if (stockValidation.IsValid)
                {
                    var availabilityValidation = await ValidateStockAvailabilityAsync(item.StockId, item.Quantity);
                    result.Merge(availabilityValidation);
                }
                else
                {
                    result.Merge(stockValidation);
                }
            }

            // Validate totals
            if (request.Total < 0 || request.GrandTotal < 0)
            {
                result.AddError("Sale total and grand total cannot be negative");
            }

            if (request.Discount < 0)
            {
                result.AddError("Sale discount cannot be negative");
            }

            return result;
        }

        public async Task<ValidationResult> ValidateUpdateSaleRequestAsync(UpdateSaleRequest request)
        {
            var result = new ValidationResult();

            // Validate sale exists
            var saleValidation = await ValidateSaleExistsAsync(request.Id);
            result.Merge(saleValidation);

            // Validate customer exists
            var customerValidation = await ValidateCustomerExistsAsync(request.CustomerId);
            result.Merge(customerValidation);

            // Validate items (similar to Add)
            if (request.Items == null || !request.Items.Any())
            {
                result.AddError("Sale must have at least one item");
                return result;
            }

            foreach (var item in request.Items)
            {
                var quantityValidation = ValidateQuantity(item.Quantity, $"Item {item.StockId} quantity");
                result.Merge(quantityValidation);

                var priceValidation = ValidatePrice(item.Price, $"Item {item.StockId} price");
                result.Merge(priceValidation);

                var itemTotal = item.Price * item.Quantity;
                var discountValidation = ValidateDiscount(itemTotal, item.Discount, $"Item {item.StockId}");
                result.Merge(discountValidation);

                var stockValidation = await ValidateStockExistsAsync(item.StockId);
                result.Merge(stockValidation);
            }

            return result;
        }

        #endregion

        #region Purchase Request Validation

        public async Task<ValidationResult> ValidatePurchaseRequestAsync(AddPurchaseRequest request)
        {
            var result = new ValidationResult();

            // Validate supplier exists
            var supplierValidation = await ValidateSupplierExistsAsync(request.SupplierId);
            result.Merge(supplierValidation);

            // Validate items
            if (request.Items == null || !request.Items.Any())
            {
                result.AddError("Purchase must have at least one item");
                return result;
            }

            // Validate each item
            foreach (var item in request.Items)
            {
                // Validate medicine exists
                var medicineValidation = await ValidateMedicineExistsAsync(item.MedicineId);
                result.Merge(medicineValidation);

                // Validate quantity
                var quantityValidation = ValidateQuantity(item.Quantity, $"Medicine {item.MedicineId} quantity");
                result.Merge(quantityValidation);

                // Validate price
                var priceValidation = ValidatePrice(item.Price, $"Medicine {item.MedicineId} price");
                result.Merge(priceValidation);

                // Validate discount
                var itemTotal = item.Price * item.Quantity;
                var discountValidation = ValidateDiscount(itemTotal, item.Discount, $"Medicine {item.MedicineId}");
                result.Merge(discountValidation);

                // Validate batch number
                if (string.IsNullOrWhiteSpace(item.BatchNo))
                {
                    result.AddError($"Medicine {item.MedicineId}: Batch number is required");
                }

                // Validate expiry date
                if (item.ExpiryDate <= DateTime.Now)
                {
                    result.AddError($"Medicine {item.MedicineId}: Expiry date must be in the future");
                }
            }

            // Validate totals
            if (request.Total < 0 || request.GrandTotal < 0)
            {
                result.AddError("Purchase total and grand total cannot be negative");
            }

            return result;
        }

        public async Task<ValidationResult> ValidateUpdatePurchaseRequestAsync(UpdatePurchaseRequest request)
        {
            var result = new ValidationResult();

            // Validate purchase exists
            var purchase = await _unitOfWork.Repository<Purchase>().GetByIdAsync(request.Id);
            if (purchase == null)
            {
                result.AddError($"Purchase with ID {request.Id} not found");
                return result;
            }

            // Validate supplier exists
            var supplierValidation = await ValidateSupplierExistsAsync(request.SupplierId);
            result.Merge(supplierValidation);

            // Validate items
            if (request.Items == null || !request.Items.Any())
            {
                result.AddError("Purchase must have at least one item");
                return result;
            }

            foreach (var item in request.Items)
            {
                var medicineValidation = await ValidateMedicineExistsAsync(item.MedicineId);
                result.Merge(medicineValidation);

                var quantityValidation = ValidateQuantity(item.Quantity, $"Medicine {item.MedicineId} quantity");
                result.Merge(quantityValidation);

                var priceValidation = ValidatePrice(item.Price, $"Medicine {item.MedicineId} price");
                result.Merge(priceValidation);
            }

            return result;
        }

        #endregion

        #region Quotation Request Validation

        public async Task<ValidationResult> ValidateQuotationRequestAsync(AddQuotationRequest request)
        {
            var result = new ValidationResult();

            // Validate customer exists
            var customerValidation = await ValidateCustomerExistsAsync(request.CustomerId);
            result.Merge(customerValidation);

            // Validate items
            if (request.Items == null || !request.Items.Any())
            {
                result.AddError("Quotation must have at least one item");
                return result;
            }

            // Validate each item
            foreach (var item in request.Items)
            {
                // Validate stock exists
                var stockValidation = await ValidateStockExistsAsync(item.StockId);
                result.Merge(stockValidation);

                // Validate quantity
                var quantityValidation = ValidateQuantity(item.Quantity, $"Item {item.StockId} quantity");
                result.Merge(quantityValidation);

                // Validate price
                var priceValidation = ValidatePrice(item.Price, $"Item {item.StockId} price");
                result.Merge(priceValidation);

                // Validate discount
                var itemTotal = item.Price * item.Quantity;
                var discountValidation = ValidateDiscount(itemTotal, item.Discount, $"Item {item.StockId}");
                result.Merge(discountValidation);
            }

            // Validate totals
            if (request.Total < 0 || request.GrandTotal < 0)
            {
                result.AddError("Quotation total and grand total cannot be negative");
            }

            // Validate quotation date
            if (request.QuotationDate > DateTime.Now.AddDays(1))
            {
                result.AddError("Quotation date cannot be in the future");
            }

            return result;
        }

        public async Task<ValidationResult> ValidateUpdateQuotationRequestAsync(UpdateQuotationRequest request)
        {
            var result = new ValidationResult();

            // Validate quotation exists
            var quotation = await _unitOfWork.Repository<Quotation>().GetByIdAsync(request.Id);
            if (quotation == null)
            {
                result.AddError($"Quotation with ID {request.Id} not found");
                return result;
            }

            // Validate customer exists
            var customerValidation = await ValidateCustomerExistsAsync(request.CustomerId);
            result.Merge(customerValidation);

            // Validate items
            if (request.Items == null || !request.Items.Any())
            {
                result.AddError("Quotation must have at least one item");
                return result;
            }

            foreach (var item in request.Items)
            {
                var stockValidation = await ValidateStockExistsAsync(item.StockId);
                result.Merge(stockValidation);

                var quantityValidation = ValidateQuantity(item.Quantity, $"Item {item.StockId} quantity");
                result.Merge(quantityValidation);

                var priceValidation = ValidatePrice(item.Price, $"Item {item.StockId} price");
                result.Merge(priceValidation);

                var itemTotal = item.Price * item.Quantity;
                var discountValidation = ValidateDiscount(itemTotal, item.Discount, $"Item {item.StockId}");
                result.Merge(discountValidation);
            }

            return result;
        }

        #endregion

        #region Return Request Validation

        public async Task<ValidationResult> ValidateReturnRequestAsync(ProcessReturnRequest request)
        {
            var result = new ValidationResult();

            // Validate sale exists
            var saleValidation = await ValidateSaleExistsAsync(request.SaleId);
            result.Merge(saleValidation);

            if (!saleValidation.IsValid)
            {
                return result; // Early return if sale doesn't exist
            }

            // Validate items to return
            if (request.SaleItemsToReturn == null || !request.SaleItemsToReturn.Any())
            {
                result.AddError("Return request must have at least one item");
                return result;
            }

            // Load sale with items to validate return quantities
            var sale = await _unitOfWork.Repository<Sales>().GetByIdWithIncludesAsync(
                request.SaleId,
                query => query.Include(s => s.Items));

            foreach (var returnItem in request.SaleItemsToReturn)
            {
                // Validate return quantity
                var quantityValidation = ValidateQuantity(returnItem.Quantity, $"Return item {returnItem.SaleItemId} quantity");
                result.Merge(quantityValidation);

                // Validate sale item exists in the sale
                var saleItem = sale?.Items.FirstOrDefault(i => i.Id == returnItem.SaleItemId);
                if (saleItem == null)
                {
                    result.AddError($"Sale item with ID {returnItem.SaleItemId} not found in sale {request.SaleId}");
                    continue;
                }

                // Validate return quantity doesn't exceed sold quantity
                if (returnItem.Quantity > saleItem.Quantity)
                {
                    result.AddError($"Return quantity {returnItem.Quantity} exceeds sold quantity {saleItem.Quantity} for item {returnItem.SaleItemId}");
                }

                // Validate return reason exists
                var returnReason = await _unitOfWork.Repository<ReturnReason>().GetByIdAsync(returnItem.ReturnReasonId);
                if (returnReason == null)
                {
                    result.AddError($"Return reason with ID {returnItem.ReturnReasonId} not found");
                }
            }

            return result;
        }

        #endregion

        #region Duplicate Detection

        public async Task<ValidationResult> ValidateSaleNotDuplicateAsync(AddSaleRequest request)
        {
            var isDuplicate = await _salesRepository.IsDuplicate(new Sales
            {
                CustomerId = request.CustomerId,
                SaleStatusId = request.SaleStatusId,
                PaymentMethodId = request.PaymentMethodId,
                Notes = request.Notes,
                SalesDate = DateTime.Now,
                Discount = request.Discount
            });

            if (isDuplicate)
            {
                _logger.LogWarning("Duplicate sale detected: CustomerId={CustomerId}", request.CustomerId);
                return ValidationResult.Failure("Duplicate sale detected. A similar sale was recently created.");
            }

            return ValidationResult.Success();
        }

        public async Task<ValidationResult> ValidatePurchaseNotDuplicateAsync(AddPurchaseRequest request)
        {
            var isDuplicate = await _purchaseRepository.IsDuplicate(new Purchase
            {
                SupplierId = request.SupplierId,
                PurchaseStatusId = request.PurchaseStatusId,
                PaymentMethodId = request.PaymentMethodId,
                Notes = request.Notes,
                PurchaseDate = DateTime.Now,
                Discount = request.Discount
            });

            if (isDuplicate)
            {
                _logger.LogWarning("Duplicate purchase detected: SupplierId={SupplierId}", request.SupplierId);
                return ValidationResult.Failure("Duplicate purchase detected. A similar purchase was recently created.");
            }

            return ValidationResult.Success();
        }

        #endregion
    }
}
