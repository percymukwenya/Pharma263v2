using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Pharma263.Api.Contracts;
using Pharma263.Api.Models.Returns.Request;
using Pharma263.Api.Models.Returns.Response;
using Pharma263.Api.Shared.Contracts;
using Pharma263.Application.Contracts.Logging;
using Pharma263.Application.Contracts.Services;
using Pharma263.Domain.Common;
using Pharma263.Domain.Entities;
using Pharma263.Domain.Interfaces.Repository;
using System;
using System.Linq.Expressions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharma263.Api.Services
{
    public class ReturnService : IScopedInjectedService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStockManagementService _stockManagementService;
        private readonly IValidationService _validationService;
        private readonly IAppLogger<ReturnService> _logger;
        private readonly IMemoryCache _memoryCache;
        private const string ReturnsCacheKey = "returns_list";
        private const string ReturnDetailsCacheKeyPrefix = "return_details_";
        private readonly TimeSpan CacheExpiry = TimeSpan.FromMinutes(5);

        public ReturnService(IUnitOfWork unitOfWork, IStockManagementService stockManagementService,
            IValidationService validationService, IAppLogger<ReturnService> logger, IMemoryCache memoryCache)
        {
            _unitOfWork = unitOfWork;
            _stockManagementService = stockManagementService;
            _validationService = validationService;
            _logger = logger;
            _memoryCache = memoryCache;
        }

        public async Task<ApiResponse<List<ReturnsListResponse>>> GetReturns()
        {
            try
            {
                if (_memoryCache.TryGetValue(ReturnsCacheKey, out List<ReturnsListResponse> cached))
                {
                    return ApiResponse<List<ReturnsListResponse>>.CreateSuccess(cached);
                }

                var returns = await _unitOfWork.Repository<Returns>().GetAllAsync(q => q.Include(x => x.ReturnReason).Include(x => x.ReturnDestination).Include(x => x.ReturnStatus));

                var mappedList = returns.Select(r => new ReturnsListResponse
                {
                    Id = r.Id,
                    Quantity = r.Quantity,
                    DateReturned = r.DateReturned,
                    ReturnReason = r.ReturnReason?.Name ?? "Unknown Reason",
                    ReturnDestination = r.ReturnDestination?.Name ?? "Unknown Destination",
                    ReturnStatus = r.ReturnStatus?.Name ?? "Unknown Status",
                    SaleId = r.SaleId,
                    StockId = r.StockId,
                    SaleItemId = r.Sale?.Items?.FirstOrDefault(si => si.StockId == r.StockId)?.Id ?? 0,
                }).ToList();

                _memoryCache.Set(ReturnsCacheKey, mappedList, CacheExpiry);

                return ApiResponse<List<ReturnsListResponse>>.CreateSuccess(mappedList);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"An error occurred while retrieving returns. {ex.Message}", ex);

                return ApiResponse<List<ReturnsListResponse>>.CreateFailure($"An error occurred while retrieving returns. {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<PaginatedList<ReturnsListResponse>>> GetReturnsPaged(PagedRequest request)
        {
            try
            {
                Expression<Func<Returns, bool>> filter = null;
                
                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    var searchTerm = request.SearchTerm.ToLower();
                    filter = x => x.ReturnReason.Name.ToLower().Contains(searchTerm) ||
                                  x.ReturnDestination.Name.ToLower().Contains(searchTerm) ||
                                  x.ReturnStatus.Name.ToLower().Contains(searchTerm);
                }

                Func<IQueryable<Returns>, IOrderedQueryable<Returns>> orderBy = request.SortBy?.ToLower() switch
                {
                    "quantity" => request.SortDescending ? (query => query.OrderByDescending(x => x.Quantity)) : (query => query.OrderBy(x => x.Quantity)),
                    "datereturned" => request.SortDescending ? (query => query.OrderByDescending(x => x.DateReturned)) : (query => query.OrderBy(x => x.DateReturned)),
                    "returnreason" => request.SortDescending ? (query => query.OrderByDescending(x => x.ReturnReason.Name)) : (query => query.OrderBy(x => x.ReturnReason.Name)),
                    "returndestination" => request.SortDescending ? (query => query.OrderByDescending(x => x.ReturnDestination.Name)) : (query => query.OrderBy(x => x.ReturnDestination.Name)),
                    "returnstatus" => request.SortDescending ? (query => query.OrderByDescending(x => x.ReturnStatus.Name)) : (query => query.OrderBy(x => x.ReturnStatus.Name)),
                    _ => query => query.OrderByDescending(x => x.DateReturned)
                };

                var paginatedReturns = await _unitOfWork.Repository<Returns>().GetPaginatedAsync(
                    request.Page,
                    request.PageSize,
                    filter,
                    orderBy,
                    query => query.Include(x => x.ReturnReason)
                                  .Include(x => x.ReturnDestination)
                                  .Include(x => x.ReturnStatus));
                
                var returns = paginatedReturns.Items.ToList();

                var data = returns.Select(r => new ReturnsListResponse
                {
                    Id = r.Id,
                    Quantity = r.Quantity,
                    DateReturned = r.DateReturned,
                    ReturnReason = r.ReturnReason?.Name ?? "Unknown Reason",
                    ReturnDestination = r.ReturnDestination?.Name ?? "Unknown Destination",
                    ReturnStatus = r.ReturnStatus?.Name ?? "Unknown Status",
                    SaleId = r.SaleId,
                    StockId = r.StockId,
                    SaleItemId = r.Sale?.Items?.FirstOrDefault(si => si.StockId == r.StockId)?.Id ?? 0,
                }).ToList();

                var paginatedResult = new PaginatedList<ReturnsListResponse>(data, paginatedReturns.TotalCount, paginatedReturns.PageIndex, request.PageSize);

                return ApiResponse<PaginatedList<ReturnsListResponse>>.CreateSuccess(paginatedResult);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"An error occurred while retrieving paged returns. {ex.Message}", ex);

                return ApiResponse<PaginatedList<ReturnsListResponse>>.CreateFailure($"An error occurred while retrieving paged returns. {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<int>> AddReturn(ProcessReturnRequest request)
        {
            // Validate request using ValidationService
            var requestValidation = await _validationService.ValidateReturnRequestAsync(request);
            if (!requestValidation.IsValid)
            {
                _logger.LogWarning("Return request validation failed: SaleId={SaleId}, Errors={Errors}",
                    request.SaleId, string.Join(", ", requestValidation.Errors));
                return ApiResponse<int>.CreateFailure("Return request validation failed", 400, requestValidation.Errors);
            }

            try
            {
                // Verify sale exists before transaction
                var sale = await _unitOfWork.Repository<Sales>().FirstOrDefaultAsync(
                    x => x.Id == request.SaleId,
                    query => query.Include(s => s.Items)
                                  .Include(s => s.Customer)
                                  .Include(s => s.SaleStatus)
                                  .Include(s => s.PaymentMethod));

                if (sale == null)
                {
                    return ApiResponse<int>.CreateFailure("Sale not found.");
                }

                // Wrap entire operation in transaction for data integrity
                return await _unitOfWork.ExecuteTransactionAsync(async () =>
                {
                    // Count of successfully processed return items
                    int successfulReturns = 0;
                    List<string> errors = new List<string>();

                    // Process each return item
                    foreach (var saleItemToReturn in request.SaleItemsToReturn)
                    {
                        // Find the sale item
                        var saleItem = await _unitOfWork.Repository<SalesItems>().GetByIdWithIncludesAsync(
                            saleItemToReturn.SaleItemId, q => q.Include(x => x.Stock).ThenInclude(m => m.Medicine));

                        if (saleItem == null)
                        {
                            errors.Add($"Sale item with ID {saleItemToReturn.SaleItemId} not found.");
                            continue;
                        }

                        // Check if the item is eligible for return
                        if (!IsEligibleForReturn(saleItemToReturn, saleItem))
                        {
                            // Determine the reason for ineligibility
                            if (saleItemToReturn.Quantity <= 0)
                            {
                                errors.Add($"Return quantity must be greater than 0 for item {saleItem.Stock.Medicine.Name}.");
                            }
                            else if (saleItemToReturn.Quantity > saleItem.Quantity)
                            {
                                errors.Add($"Return quantity cannot exceed original quantity for item {saleItem.Stock.Medicine.Name}.");
                            }
                            else if (saleItemToReturn.ReturnReasonId == 3 && DateTimeOffset.Now.Subtract(saleItem.CreatedDate).Days > 7)
                            {
                                errors.Add($"Item {saleItem.Stock.Medicine.Name} cannot be returned after 7 days for the selected reason.");
                            }
                            else
                            {
                                errors.Add($"Item {saleItem.Stock.Medicine.Name} is not eligible for return.");
                            }
                            continue;
                        }

                        try
                        {
                            // Process the return for this item
                            int returnId = await ProcessSaleItemReturn(saleItemToReturn, saleItem);
                            if (returnId > 0)
                            {
                                successfulReturns++;
                            }
                        }
                        catch (Exception ex)
                        {
                            errors.Add($"Error processing return for item {saleItem.Stock.Medicine.Name}: {ex.Message}");
                        }
                    }

                    // Return appropriate response based on results
                    if (successfulReturns > 0)
                    {
                        // Save all changes within transaction
                        await _unitOfWork.SaveChangesAsync();

                        // Invalidate cache after successful transaction
                        InvalidateReturnCache();

                        var message = successfulReturns == request.SaleItemsToReturn.Count
                            ? "All returns processed successfully."
                            : $"{successfulReturns} of {request.SaleItemsToReturn.Count} returns processed successfully.";

                        if (errors.Any())
                        {
                            message += " Errors: " + string.Join(" ", errors);
                        }

                        _logger.LogInformation(
                            "Returns processed successfully: SaleId={SaleId}, ReturnsProcessed={ReturnsProcessed}/{TotalRequested}",
                            request.SaleId, successfulReturns, request.SaleItemsToReturn.Count);

                        return ApiResponse<int>.CreateSuccess(successfulReturns, message);
                    }
                    else
                    {
                        return ApiResponse<int>.CreateFailure("Failed to process any returns. " + string.Join(" ", errors));
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"An error occurred while processing returns: {ex.Message}", ex);
                return ApiResponse<int>.CreateFailure($"An error occurred: {ex.Message}");
            }
        }

        private bool IsEligibleForReturn(SaleItemToReturnModel saleItemToReturn, SalesItems salesItem)
        {
            if (saleItemToReturn.Quantity <= 0 || saleItemToReturn.Quantity > salesItem.Quantity)
                return false;

            if (saleItemToReturn.ReturnReasonId == 3 && DateTimeOffset.Now.Subtract(salesItem.CreatedDate).Days > 7)
                return false;

            return true;
        }

        private async Task<int> ProcessSaleItemReturn(SaleItemToReturnModel saleItemToReturn, SalesItems salesItem)
        {
            // Create return entity
            var returnEntity = new Returns
            {
                SaleId = salesItem.SaleId,
                StockId = salesItem.StockId,
                Quantity = saleItemToReturn.Quantity,
                ReturnReasonId = saleItemToReturn.ReturnReasonId,
                ReturnDestinationId = saleItemToReturn.ReturnDestinationId,
                ReturnStatusId = 1, // Completed
                DateReturned = DateTime.Now
            };

            await _unitOfWork.Repository<Returns>().AddAsync(returnEntity);

            var stock = await _unitOfWork.Repository<Stock>().FirstOrDefaultAsync(x => x.Id == salesItem.StockId);

            if (stock != null)
            {
                // Return reason 1: Return to Stock
                if (saleItemToReturn.ReturnReasonId == 1)
                {
                    // Increase stock quantity with business logic validation
                    var stockResult = await _stockManagementService.AddStockAsync(
                        stock.Id,
                        saleItemToReturn.Quantity,
                        $"Return to stock for Sale ID: {salesItem.SaleId}");

                    if (!stockResult.Success)
                    {
                        _logger.LogWarning($"Failed to add stock during return: {stockResult.Message}");
                    }
                }
                // Return reason 2 or 3: Defective/Expired items go to quarantine
                else if (saleItemToReturn.ReturnReasonId == 2 || saleItemToReturn.ReturnReasonId == 3)
                {
                    // Create quarantine record
                    var quarantineEntity = new Quarantine
                    {
                        TotalQuantity = saleItemToReturn.Quantity,
                        BatchNo = stock.BatchNo,
                        MedicineId = stock.MedicineId,
                        ReturnReasonId = saleItemToReturn.ReturnReasonId,
                        CreatedDate = DateTime.Now
                    };

                    await _unitOfWork.Repository<Quarantine>().AddAsync(quarantineEntity);

                    _logger.LogInformation(
                        "Item moved to quarantine: MedicineId={MedicineId}, BatchNo={BatchNo}, Quantity={Quantity}, Reason={ReasonId}",
                        stock.MedicineId, stock.BatchNo, saleItemToReturn.Quantity, saleItemToReturn.ReturnReasonId);
                }
            }

            // If all items are returned, update the sale status if needed
            if (salesItem.Quantity == 0)
            {
                var allItemsInSale = await _unitOfWork.Repository<SalesItems>().FindAsync(si => si.SaleId == salesItem.SaleId);
                bool allItemsReturned = allItemsInSale.All(item => item.Quantity == 0);

                if (allItemsReturned)
                {
                    // Get the sale
                    var sale = await _unitOfWork.Repository<Sales>().GetByIdAsync(salesItem.SaleId);
                    if (sale != null)
                    {
                        // Update to "Fully Returned" status
                        sale.SaleStatusId = 3;
                        _unitOfWork.Repository<Sales>().Update(sale);
                    }
                }
            }

            // Note: SaveChangesAsync is called by the transaction wrapper in ProcessReturn
            // Return entity ID will be generated after save
            return returnEntity.Id;
        }

        public async Task<ApiResponse<ReturnsDetailResponse>> GetReturnById(int id)
        {
            try
            {
                var cacheKey = $"{ReturnDetailsCacheKeyPrefix}{id}";
                if (_memoryCache.TryGetValue(cacheKey, out ReturnsDetailResponse cached))
                {
                    return ApiResponse<ReturnsDetailResponse>.CreateSuccess(cached);
                }

                var returnItem = await _unitOfWork.Repository<Returns>().FirstOrDefaultAsync(r => r.Id == id, query =>
                query.Include(r => r.ReturnReason)
                     .Include(r => r.ReturnDestination)
                     .Include(r => r.ReturnStatus)
                     .Include(r => r.Stock)
                        .ThenInclude(s => s.Medicine)
                     .Include(r => r.Sale)
            );

                if (returnItem == null)
                    return ApiResponse<ReturnsDetailResponse>.CreateFailure("Return not found.", 404);

                var response = new ReturnsDetailResponse
                {
                    Id = returnItem.Id,
                    Quantity = returnItem.Quantity,
                    DateReturned = returnItem.DateReturned,
                    Notes = returnItem.Notes,
                    ReturnReason = returnItem.ReturnReason?.Name ?? "Unknown Reason",
                    ReturnDestination = returnItem.ReturnDestination?.Name ?? "Unknown Destination",
                    ReturnStatus = returnItem.ReturnStatus?.Name ?? "Unknown Status",
                    SaleId = returnItem.SaleId,
                    StockId = returnItem.StockId,
                    SaleItemId = returnItem.Sale?.Items?.FirstOrDefault(si => si.StockId == returnItem.StockId)?.Id ?? 0,
                    ReturnReasonId = returnItem.ReturnReasonId,
                    ReturnDestinationId = returnItem.ReturnDestinationId,
                    ReturnStatusId = returnItem.ReturnStatusId,
                    MedicineName = returnItem.Stock?.Medicine?.Name ?? "Unknown Medicine"
                };

                _memoryCache.Set(cacheKey, response, CacheExpiry);

                return ApiResponse<ReturnsDetailResponse>.CreateSuccess(response);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"An error occurred while retrieving return details. {ex.Message}", ex);

                return ApiResponse<ReturnsDetailResponse>.CreateFailure($"An error occurred while retrieving return details. {ex.Message}", 500);
            }
        }

        private void InvalidateReturnCache()
        {
            _memoryCache.Remove(ReturnsCacheKey);
            
            var keys = new List<object>();
            if (_memoryCache is MemoryCache memCache)
            {
                var field = typeof(MemoryCache).GetField("_coherentState", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                var coherentState = field?.GetValue(memCache);
                var entriesCollection = coherentState?.GetType().GetProperty("EntriesCollection", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                var entries = (IDictionary)entriesCollection?.GetValue(coherentState);

                if (entries != null)
                {
                    foreach (DictionaryEntry entry in entries)
                    {
                        if (entry.Key.ToString().StartsWith(ReturnDetailsCacheKeyPrefix))
                        {
                            keys.Add(entry.Key);
                        }
                    }
                }
            }

            foreach (var key in keys)
            {
                _memoryCache.Remove(key);
            }
        }
    }
}
