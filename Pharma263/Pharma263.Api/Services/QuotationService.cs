using Microsoft.EntityFrameworkCore;
using Pharma263.Api.Contracts;
using Pharma263.Api.Models.Quotation.Request;
using Pharma263.Api.Models.Quotation.Response;
using Pharma263.Api.Shared.Contracts;
using Pharma263.Application.Contracts.Logging;
using Pharma263.Application.Contracts.Services;
using Pharma263.Application.Models;
using Pharma263.Application.Services.Printing;
using Pharma263.Domain.Common;
using Pharma263.Domain.Entities;
using Pharma263.Domain.Interfaces.Repository;
using Pharma263.Domain.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Pharma263.Api.Services
{
    public class QuotationService : IScopedInjectedService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IQuotationRepository _quotationRepository;
        private readonly IStockManagementService _stockManagementService;
        private readonly IValidationService _validationService;
        private readonly IAppLogger<QuotationService> _logger;

        public QuotationService(IUnitOfWork unitOfWork, IQuotationRepository quotationRepository,
            IStockManagementService stockManagementService, IValidationService validationService,
            IAppLogger<QuotationService> logger)
        {
            _unitOfWork = unitOfWork;
            _quotationRepository = quotationRepository;
            _stockManagementService = stockManagementService;
            _validationService = validationService;
            _logger = logger;
        }

        public async Task<ApiResponse<List<QuotationListResponse>>> GetQuotations()
        {
            try
            {
                // Removed unnecessary .Include(c => c.Items) - not used in list response
                var quotations = await _unitOfWork.Repository<Quotation>().GetAllAsync(query => query.Include(c => c.Customer).Include(x => x.QuoteStatus));

                var quotationList = quotations.Select(x => new QuotationListResponse
                {
                    Id = x.Id,
                    QuotationDate = x.QuotationDate,
                    CustomerName = x.Customer.Name,
                    Notes = x.Notes,
                    Total = (double)x.Total,
                    Discount = (double)x.Discount,
                    GrandTotal = (double)x.GrandTotal,
                    QuotationStatus = x.QuoteStatus.Name,
                    CustomerAddress = x.Customer.DeliveryAddress,
                    CustomerPhone = x.Customer.Phone
                }).ToList();

                quotationList = quotationList.OrderByDescending(x => x.QuotationDate).ToList();

                return ApiResponse<List<QuotationListResponse>>.CreateSuccess(quotationList);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"An error occurred while retrieving quotations. {ex.Message}", ex);

                return ApiResponse<List<QuotationListResponse>>.CreateFailure($"An error occurred while retrieving quotations. {ex.Message}", 500);
            }

        }

        public async Task<ApiResponse<PaginatedList<QuotationListResponse>>> GetQuotationsPaged(PagedRequest request)
        {
            try
            {
                Expression<Func<Quotation, bool>> filter = null;

                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    var searchTerm = request.SearchTerm.ToLower();
                    filter = x => x.Customer.Name.ToLower().Contains(searchTerm) ||
                                  x.Notes.ToLower().Contains(searchTerm) ||
                                  x.QuoteStatus.Name.ToLower().Contains(searchTerm);
                }

                Func<IQueryable<Quotation>, IOrderedQueryable<Quotation>> orderBy = request.SortBy?.ToLower() switch
                {
                    "customername" => request.SortDescending ? (query => query.OrderByDescending(x => x.Customer.Name)) : (query => query.OrderBy(x => x.Customer.Name)),
                    "quotationdate" => request.SortDescending ? (query => query.OrderByDescending(x => x.QuotationDate)) : (query => query.OrderBy(x => x.QuotationDate)),
                    "total" => request.SortDescending ? (query => query.OrderByDescending(x => x.Total)) : (query => query.OrderBy(x => x.Total)),
                    "grandtotal" => request.SortDescending ? (query => query.OrderByDescending(x => x.GrandTotal)) : (query => query.OrderBy(x => x.GrandTotal)),
                    "quotationstatus" => request.SortDescending ? (query => query.OrderByDescending(x => x.QuoteStatus.Name)) : (query => query.OrderBy(x => x.QuoteStatus.Name)),
                    _ => query => query.OrderByDescending(x => x.QuotationDate)
                };

                var paginatedQuotations = await _unitOfWork.Repository<Quotation>().GetPaginatedAsync(
                    request.Page,
                    request.PageSize,
                    filter,
                    orderBy,
                    query => query.Include(c => c.Customer)
                                  .Include(c => c.QuoteStatus));

                var quotations = paginatedQuotations.Items.ToList();

                var data = quotations.Select(x => new QuotationListResponse
                {
                    Id = x.Id,
                    QuotationDate = x.QuotationDate,
                    CustomerName = x.Customer.Name,
                    Notes = x.Notes,
                    Total = (double)x.Total,
                    Discount = (double)x.Discount,
                    GrandTotal = (double)x.GrandTotal,
                    QuotationStatus = x.QuoteStatus.Name,
                    CustomerAddress = x.Customer.DeliveryAddress,
                    CustomerPhone = x.Customer.Phone
                }).ToList();

                var paginatedResult = new PaginatedList<QuotationListResponse>(data, paginatedQuotations.TotalCount, paginatedQuotations.PageIndex, request.PageSize);

                return ApiResponse<PaginatedList<QuotationListResponse>>.CreateSuccess(paginatedResult);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"An error occurred while retrieving paginated quotations. {ex.Message}", ex);

                return ApiResponse<PaginatedList<QuotationListResponse>>.CreateFailure($"An error occurred while retrieving paginated quotations. {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<QuotationDetailsResponse>> GetQuotation(int id)
        {
            try
            {
                // Fix N+1 query: Eagerly load Stock.Medicine with ThenInclude
                var quotation = await _unitOfWork.Repository<Quotation>().GetByIdWithIncludesAsync(id,
                    query => query.Include(c => c.Customer)
                                  .Include(c => c.Items)
                                      .ThenInclude(i => i.Stock)
                                          .ThenInclude(s => s.Medicine)
                                  .Include(c => c.QuoteStatus));

                // convert data object to DTO object
                var quotationDetails = new QuotationDetailsResponse
                {
                    Id = quotation.Id,
                    QuotationDate = quotation.QuotationDate,
                    CustomerName = quotation.Customer.Name,
                    Notes = quotation.Notes,
                    Total = (double)quotation.Total,
                    Discount = (double)quotation.Discount,
                    GrandTotal = (double)quotation.GrandTotal,
                    QuotationStatusId = quotation.QuoteStatusId,
                    QuotationStatus = quotation.QuoteStatus.Name,
                    CustomerAddress = quotation.Customer.DeliveryAddress,
                    CustomerPhone = quotation.Customer.Phone,
                    CustomerId = quotation.CustomerId,
                    Items = quotation.Items.Select(item => new GetQuotationItemsResponse
                    {
                        Id = item.Id,
                        Price = item.Price,
                        Quantity = item.Quantity,
                        Discount = item.Discount,
                        Amount = item.Amount,
                        StockId = item.StockId,
                        MedicineName = item.Stock?.Medicine?.Name ?? string.Empty,
                        MedicineId = item.Stock?.MedicineId ?? 0
                    }).ToList()
                };

                // N+1 query removed - Stock.Medicine now eagerly loaded above

                return ApiResponse<QuotationDetailsResponse>.CreateSuccess(quotationDetails);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"An error occurred while retrieving quotation details. {ex.Message}", ex);

                return ApiResponse<QuotationDetailsResponse>.CreateFailure($"An error occurred while retrieving quotation details. {ex.Message}", 500);
            }
        }

        public async Task<byte[]> GetQuotationDoc(int id)
        {
            var store = await _unitOfWork.Repository<StoreSetting>().GetAllAsync();
            var storeInfo = store.FirstOrDefault();

            // Fix N+1 query: Add Medicine to ThenInclude chain
            var quotation = await _unitOfWork.Repository<Quotation>().GetByIdWithIncludesAsync(id,
                query => query.Include(c => c.Customer)
                              .Include(c => c.QuoteStatus)
                              .Include(c => c.Items)
                                  .ThenInclude(x => x.Stock)
                                      .ThenInclude(s => s.Medicine));

            var quotationDto = new QuotationDto
            {
                Id = quotation.Id,
                QuotationDate = quotation.QuotationDate,
                CustomerName = quotation.Customer.Name,
                Notes = quotation.Notes,
                Total = (double)quotation.Total,
                Discount = (double)quotation.Discount,
                GrandTotal = (double)quotation.GrandTotal,
                QuotationStatus = quotation.QuoteStatus.Name,
                CustomerAddress = quotation.Customer.DeliveryAddress,
                CustomerPhone = quotation.Customer.Phone,
                Items = [.. quotation.Items.Select(x => new GetQuotationItemDto
                {
                     Id = x.Id,
                    Price = x.Price,
                    Quantity = x.Quantity,
                    Discount = x.Discount,
                    Amount = x.Amount,
                    StockId = x.StockId,
                    MedicineName = x.Stock?.Medicine?.Name ?? string.Empty,
                    MedicineId = x.Stock?.MedicineId ?? 0,
                    BatchNo = x.Stock?.BatchNo ?? string.Empty,
                    ExpiryDate = x.Stock?.ExpiryDate ?? DateTime.Now,
                    QuotationId = x.QuotationId
                })]
            };

            // N+1 query removed - Stock.Medicine now eagerly loaded above

            if (quotation != null)
            {
                var qoute = new QuotationReportViewModel
                {
                    Company = new StoreSettingsDetailsDto
                    {
                        Id = storeInfo.Id,
                        Logo = storeInfo.Logo,
                        StoreName = storeInfo.StoreName,
                        Email = storeInfo.Email,
                        Phone = storeInfo.Phone,
                        Currency = storeInfo.Currency,
                        Address = storeInfo.Address,
                        MCAZLicence = storeInfo.MCAZLicence,
                        VATNumber = storeInfo.VATNumber,
                        BankingDetails = storeInfo.BankingDetails,
                        ReturnsPolicy = storeInfo.ReturnsPolicy
                    },
                    Quotation = quotationDto
                };
                QuotationInvoiceReport salesReport = new QuotationInvoiceReport();
                byte[] bytes = salesReport.CreateReport(qoute);
                return bytes;
            }
            return null;
        }

        public async Task<ApiResponse<bool>> AddQuotation(AddQuotationRequest request)
        {
            // Validate request using ValidationService
            var requestValidation = await _validationService.ValidateQuotationRequestAsync(request);
            if (!requestValidation.IsValid)
            {
                _logger.LogWarning("Quotation request validation failed: {Errors}", string.Join(", ", requestValidation.Errors));
                return ApiResponse<bool>.CreateFailure("Quotation request validation failed", 400, requestValidation.Errors);
            }

            try
            {
                var quoteToCreate = new Quotation
                {
                    CustomerId = request.CustomerId,
                    QuotationDate = request.QuotationDate,
                    Notes = request.Notes,
                    Total = request.Total,
                    Discount = request.Discount,
                    GrandTotal = request.GrandTotal,
                    QuoteExpiryDate = DateTime.UtcNow.AddHours(2).AddDays(2),
                    QuoteStatusId = 5, // Default status
                    Items = new List<QuotationItems>()
                };

                // Process items (validation already done by ValidationService)
                foreach (var item in request.Items)
                {
                    decimal netAmount = (item.Price * item.Quantity) - item.Discount;

                    var quotationItem = new QuotationItems
                    {
                        Price = item.Price,
                        Quantity = item.Quantity,
                        Discount = item.Discount,
                        Amount = netAmount,
                        StockId = item.StockId,
                    };

                    quoteToCreate.Items.Add(quotationItem);
                }

                await _unitOfWork.Repository<Quotation>().AddAsync(quoteToCreate);

                var result = await _unitOfWork.SaveChangesAsync();

                if (result == 0)
                {
                    return ApiResponse<bool>.CreateFailure("Error adding quotation", 400);
                }

                return ApiResponse<bool>.CreateSuccess(true);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"An error occurred while adding quotation. {ex.Message}", ex);

                return ApiResponse<bool>.CreateFailure($"An error occurred while adding quotation. {ex.Message}", 500);
            }

        }

        public async Task<ApiResponse<bool>> UpdateQuotation(UpdateQuotationRequest request)
        {
            // Validate request using ValidationService
            var requestValidation = await _validationService.ValidateUpdateQuotationRequestAsync(request);
            if (!requestValidation.IsValid)
            {
                _logger.LogWarning("Quotation update validation failed: QuotationId={QuotationId}, Errors={Errors}",
                    request.Id, string.Join(", ", requestValidation.Errors));
                return ApiResponse<bool>.CreateFailure("Quotation update validation failed", 400, requestValidation.Errors);
            }

            try
            {
                var quoteToUpdate = await _unitOfWork.Repository<Quotation>()
                        .GetByIdWithIncludesAsync(request.Id, query => query.Include(q => q.Items));

                if (quoteToUpdate == null)
                    return ApiResponse<bool>.CreateFailure("Quotation not found.", 404);

                // Optionally remove existing items.
                if (quoteToUpdate.Items.Any())
                {
                    foreach (var existingItem in quoteToUpdate.Items.ToList())
                    {
                        _unitOfWork.Repository<QuotationItems>().Delete(existingItem);
                    }
                    await _unitOfWork.SaveChangesAsync();
                }

                // Update the header details from the request.
                quoteToUpdate.CustomerId = request.CustomerId;
                quoteToUpdate.QuotationDate = request.QuotationDate;
                quoteToUpdate.Total = request.Total;
                quoteToUpdate.Discount = request.Discount;
                quoteToUpdate.GrandTotal = request.GrandTotal;
                quoteToUpdate.Notes = request.Notes;

                // Process items (validation already done by ValidationService)
                foreach (var item in request.Items)
                {
                    decimal netAmount = (item.Price * item.Quantity) - item.Discount;

                    quoteToUpdate.Items.Add(new QuotationItems
                    {
                        Price = item.Price,
                        Quantity = item.Quantity,
                        Discount = item.Discount,
                        Amount = netAmount,
                        StockId = item.StockId
                    });
                }

                _unitOfWork.Repository<Quotation>().Update(quoteToUpdate);

                var result = await _unitOfWork.SaveChangesAsync();

                if (result == 0)
                    return ApiResponse<bool>.CreateFailure("Error updating quotation.", 400);

                return ApiResponse<bool>.CreateSuccess(true);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"An error occurred while updating quotation. {ex.Message}", ex);

                return ApiResponse<bool>.CreateFailure($"An error occurred while updating quotation. {ex.Message}", 500);
            }

        }

        public async Task<ApiResponse<bool>> FinalizeQuotationToSale(int quotationId)
        {
            try
            {
                // Retrieve the quotation including its items - outside transaction for early validation
                var quotation = await _unitOfWork.Repository<Quotation>()
                                        .GetByIdWithIncludesAsync(quotationId, query => query.Include(q => q.Items));
                if (quotation == null)
                    return ApiResponse<bool>.CreateFailure("Quotation not found.", 404);

                // Wrap entire operation in transaction for data integrity
                return await _unitOfWork.ExecuteTransactionAsync(async () =>
                {
                    // Create a new Sale entity and map details from the quotation
                    var sale = new Sales
                    {
                        CustomerId = quotation.CustomerId,
                        SalesDate = DateTime.Now,
                        Notes = quotation.Notes,
                        Total = quotation.Total,
                        Discount = quotation.Discount,
                        GrandTotal = quotation.GrandTotal,
                        SaleStatusId = 1, // e.g., Pending or Finalized
                        Items = new List<SalesItems>()
                    };

                    var errors = new List<string>();

                    // Process each quotation item
                    foreach (var quoteItem in quotation.Items)
                    {
                        decimal netAmount = (quoteItem.Price * quoteItem.Quantity) - quoteItem.Discount;
                        if (netAmount < 0)
                        {
                            errors.Add($"Invalid discount calculation for item StockId {quoteItem.StockId}: {netAmount}");
                            continue;
                        }

                        // Validate stock availability before deduction
                        var isAvailable = await _stockManagementService.IsStockAvailableAsync(
                            quoteItem.StockId, quoteItem.Quantity);
                        if (!isAvailable)
                        {
                            errors.Add($"Insufficient stock for StockId: {quoteItem.StockId}");
                            continue;
                        }

                        // Deduct stock with business logic validation
                        var stockResult = await _stockManagementService.DeductStockAsync(
                            quoteItem.StockId, quoteItem.Quantity,
                            $"Quotation {quotationId} finalized to sale for Customer ID: {quotation.CustomerId}");

                        if (!stockResult.Success)
                        {
                            errors.Add($"Failed to deduct stock for StockId {quoteItem.StockId}: {stockResult.Message}");
                            continue;
                        }

                        var saleItem = new SalesItems
                        {
                            StockId = quoteItem.StockId,
                            Price = quoteItem.Price,
                            Quantity = quoteItem.Quantity,
                            Discount = quoteItem.Discount,
                            Amount = netAmount
                        };

                        sale.Items.Add(saleItem);
                    }

                    if (errors.Any())
                    {
                        return ApiResponse<bool>.CreateFailure("Error finalizing quotation to sale", 400, errors);
                    }

                    if (!sale.Items.Any())
                    {
                        return ApiResponse<bool>.CreateFailure("No valid items to convert to sale", 400);
                    }

                    // Save the new sale and update quotation status atomically
                    await _unitOfWork.Repository<Sales>().AddAsync(sale);

                    // Mark quotation as converted (if you have a status for this)
                    // quotation.QuoteStatusId = /* Converted status ID */;
                    _unitOfWork.Repository<Quotation>().Update(quotation);

                    // Single SaveChangesAsync - commits or rolls back ALL changes
                    await _unitOfWork.SaveChangesAsync();

                    _logger.LogInformation("Quotation finalized to sale: QuotationId={QuotationId}, SaleId={SaleId}, Items={ItemCount}",
                        quotationId, sale.Id, sale.Items.Count);

                    return ApiResponse<bool>.CreateSuccess(true, "Quotation successfully converted to sale", 200);
                });
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Error finalizing quotation to sale: QuotationId={QuotationId}, Error={Error}",
                    quotationId, ex.Message, ex);

                return ApiResponse<bool>.CreateFailure($"An error occurred while finalizing quotation to sale: {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<bool>> DeleteQuotation(int id)
        {
            try
            {
                var quoteToDelete = await _unitOfWork.Repository<Quotation>().FirstOrDefaultAsync(x => x.Id == id);

                if (quoteToDelete == null)
                    return ApiResponse<bool>.CreateFailure("Quotation not found.", 404);

                var items = await _unitOfWork.Repository<QuotationItems>().FindAsync(x => x.QuotationId == id);

                _unitOfWork.Repository<Quotation>().Delete(quoteToDelete);

                var result = await _unitOfWork.SaveChangesAsync();

                if (result == 0)
                    return ApiResponse<bool>.CreateFailure("Error deleting quotation.", 400);

                return ApiResponse<bool>.CreateSuccess(true);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"An error occurred while deleting quotation. {ex.Message}", ex);

                return ApiResponse<bool>.CreateFailure($"An error occurred while deleting quotation. {ex.Message}", 500);
            }
        }
    }
}
