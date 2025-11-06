using Pharma263.Api.Models.Purchase.Request;
using Pharma263.Api.Models.Quotation.Request;
using Pharma263.Api.Models.Return.Request;
using Pharma263.Api.Models.Sales.Request;
using Pharma263.Application.Models;
using System.Threading.Tasks;

namespace Pharma263.Application.Contracts.Services
{
    /// <summary>
    /// Service for centralized validation logic across all business operations
    /// </summary>
    public interface IValidationService
    {
        // Entity Existence Validation
        Task<ValidationResult> ValidateCustomerExistsAsync(int customerId);
        Task<ValidationResult> ValidateSupplierExistsAsync(int supplierId);
        Task<ValidationResult> ValidateMedicineExistsAsync(int medicineId);
        Task<ValidationResult> ValidateStockExistsAsync(int stockId);
        Task<ValidationResult> ValidateSaleExistsAsync(int saleId);

        // Financial Validation
        ValidationResult ValidateDiscount(decimal itemTotal, decimal discount, string itemDescription = "Item");
        ValidationResult ValidatePrice(decimal price, string fieldName = "Price");
        ValidationResult ValidateQuantity(int quantity, string fieldName = "Quantity");

        // Stock Validation
        Task<ValidationResult> ValidateStockAvailabilityAsync(int stockId, int requestedQuantity);

        // Request-Level Validation (Business Rules)
        Task<ValidationResult> ValidateSaleRequestAsync(AddSaleRequest request);
        Task<ValidationResult> ValidateUpdateSaleRequestAsync(UpdateSaleRequest request);
        Task<ValidationResult> ValidatePurchaseRequestAsync(AddPurchaseRequest request);
        Task<ValidationResult> ValidateUpdatePurchaseRequestAsync(UpdatePurchaseRequest request);
        Task<ValidationResult> ValidateQuotationRequestAsync(AddQuotationRequest request);
        Task<ValidationResult> ValidateUpdateQuotationRequestAsync(UpdateQuotationRequest request);
        Task<ValidationResult> ValidateReturnRequestAsync(ProcessReturnRequest request);

        // Duplicate Detection
        Task<ValidationResult> ValidateSaleNotDuplicateAsync(AddSaleRequest request);
        Task<ValidationResult> ValidatePurchaseNotDuplicateAsync(AddPurchaseRequest request);
    }
}
