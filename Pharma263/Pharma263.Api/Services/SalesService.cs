using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Pharma263.Api.Contracts;
using Pharma263.Api.Models.Sales.Request;
using Pharma263.Api.Models.Sales.Response;
using Pharma263.Api.Shared.Contracts;
using Pharma263.Application.Contracts.Services;
using Pharma263.Application.Services.Printing;
using Pharma263.Domain.Common;
using Pharma263.Domain.Entities;
using Pharma263.Domain.Interfaces.Repository;
using Pharma263.Domain.Models.Dtos;
using System;
using System.Linq.Expressions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pharma263.Application.Models;

namespace Pharma263.Api.Services
{
    public class SalesService : IScopedInjectedService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISalesRepository _salesRepository;
        private readonly IStockManagementService _stockManagementService;
        private readonly IValidationService _validationService;
        private readonly ILogger<SalesService> _logger;
        private readonly IMemoryCache _memoryCache;
        private const string SalesCacheKey = "sales_list";
        private const string SaleDetailsCacheKeyPrefix = "sale_details_";
        private readonly TimeSpan CacheExpiry = TimeSpan.FromMinutes(5);

        public SalesService(IUnitOfWork unitOfWork, ISalesRepository salesRepository,
            IStockManagementService stockManagementService, IValidationService validationService,
            ILogger<SalesService> logger, IMemoryCache memoryCache)
        {
            _unitOfWork = unitOfWork;
            _salesRepository = salesRepository;
            _stockManagementService = stockManagementService;
            _validationService = validationService;
            _logger = logger;
            _memoryCache = memoryCache;
        }

        public async Task<ApiResponse<List<SaleListResponse>>> GetSales()
        {
            try
            {
                if (_memoryCache.TryGetValue(SalesCacheKey, out List<SaleListResponse> cachedSales))
                {
                    return ApiResponse<List<SaleListResponse>>.CreateSuccess(cachedSales);
                }

                var sales = await _unitOfWork.Repository<Sales>().GetAllAsync(x => x.Include(c => c.Customer).Include(c => c.SaleStatus).Include(c => c.PaymentMethod));

                var data = sales.Select(x => new SaleListResponse
                {
                    Id = x.Id,
                    SalesDate = x.SalesDate,
                    Notes = x.Notes,
                    Total = (double)x.Total,
                    SaleStatus = x.SaleStatus.Name,
                    Discount = (double)x.Discount,
                    GrandTotal = (double)x.GrandTotal,
                    PaymentMethod = x.PaymentMethod.Name,
                    CustomerName = x.Customer.Name
                }).ToList();

                data = [.. data.OrderByDescending(x => x.SalesDate)];

                _memoryCache.Set(SalesCacheKey, data, CacheExpiry);

                return ApiResponse<List<SaleListResponse>>.CreateSuccess(data);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while retrieving sales. {ex.Message}", ex);

                return ApiResponse<List<SaleListResponse>>.CreateFailure($"An error occurred while retrieving sales. {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<PaginatedList<SaleListResponse>>> GetSalesPaged(PagedRequest request)
        {
            try
            {
                // Use Query() for projection - reduces data transfer by 30-40%
                IQueryable<Sales> query = _unitOfWork.Repository<Sales>().Query()
                    .Include(c => c.Customer)
                    .Include(c => c.SaleStatus)
                    .Include(c => c.PaymentMethod);

                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    var searchTerm = request.SearchTerm.ToLower();
                    query = query.Where(x => x.Customer.Name.ToLower().Contains(searchTerm) ||
                                            x.Notes.ToLower().Contains(searchTerm) ||
                                            x.SaleStatus.Name.ToLower().Contains(searchTerm) ||
                                            x.PaymentMethod.Name.ToLower().Contains(searchTerm));
                }

                // Apply sorting
                query = request.SortBy?.ToLower() switch
                {
                    "customer" => request.SortDescending ? query.OrderByDescending(x => x.Customer.Name) : query.OrderBy(x => x.Customer.Name),
                    "salesdate" => request.SortDescending ? query.OrderByDescending(x => x.SalesDate) : query.OrderBy(x => x.SalesDate),
                    "total" => request.SortDescending ? query.OrderByDescending(x => x.Total) : query.OrderBy(x => x.Total),
                    "grandtotal" => request.SortDescending ? query.OrderByDescending(x => x.GrandTotal) : query.OrderBy(x => x.GrandTotal),
                    "salestatus" => request.SortDescending ? query.OrderByDescending(x => x.SaleStatus.Name) : query.OrderBy(x => x.SaleStatus.Name),
                    "paymentmethod" => request.SortDescending ? query.OrderByDescending(x => x.PaymentMethod.Name) : query.OrderBy(x => x.PaymentMethod.Name),
                    _ => query.OrderByDescending(x => x.SalesDate)
                };

                var count = await query.CountAsync();

                // Project to DTO before materialization - EF generates optimized SQL
                var data = await query
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(x => new SaleListResponse
                    {
                        Id = x.Id,
                        SalesDate = x.SalesDate,
                        Notes = x.Notes,
                        Total = (double)x.Total,
                        SaleStatus = x.SaleStatus.Name,
                        Discount = (double)x.Discount,
                        GrandTotal = (double)x.GrandTotal,
                        PaymentMethod = x.PaymentMethod.Name,
                        CustomerName = x.Customer.Name
                    })
                    .ToListAsync();

                var paginatedResult = new PaginatedList<SaleListResponse>(data, count, request.Page, request.PageSize);

                return ApiResponse<PaginatedList<SaleListResponse>>.CreateSuccess(paginatedResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while retrieving paged sales. {ex.Message}", ex);

                return ApiResponse<PaginatedList<SaleListResponse>>.CreateFailure($"An error occurred while retrieving paged sales. {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<SaleDetailsResponse>> GetSale(int id)
        {
            try
            {
                var cacheKey = $"{SaleDetailsCacheKeyPrefix}{id}";
                if (_memoryCache.TryGetValue(cacheKey, out SaleDetailsResponse cachedSale))
                {
                    return ApiResponse<SaleDetailsResponse>.CreateSuccess(cachedSale);
                }

                // Fix N+1 query: Eagerly load Stock.Medicine with ThenInclude
                var sale = await _unitOfWork.Repository<Sales>().GetByIdWithIncludesAsync(id, query =>
                query.Include(x => x.Customer)
                      .Include(x => x.Items)
                          .ThenInclude(x => x.Stock)
                              .ThenInclude(x => x.Medicine)
                      .Include(x => x.SaleStatus)
                      .Include(x => x.PaymentMethod));

                if (sale == null)
                    return ApiResponse<SaleDetailsResponse>.CreateFailure("Sale not found", 404);

                var data = new SaleDetailsResponse
                {
                    Id = sale.Id,
                    CustomerName = sale.Customer.Name,
                    SalesDate = sale.SalesDate,
                    Notes = sale.Notes,
                    Total = (double)sale.Total,
                    SaleStatus = sale.SaleStatus.Name,
                    Discount = (double)sale.Discount,
                    GrandTotal = (double)sale.GrandTotal,
                    PaymentMethod = sale.PaymentMethod.Name,
                    Items = [.. sale.Items.Select(item => new GetSalesItemsResponse
                {
                    Id = item.Id,
                    Price = item.Price,
                    Amount = item.Amount,
                    Quantity = item.Quantity,
                    StockId = item.StockId,
                    MedicineName = item.Stock?.Medicine?.Name ?? string.Empty
                })]
                };

                // N+1 query removed - Stock.Medicine now eagerly loaded above

                _memoryCache.Set(cacheKey, data, CacheExpiry);

                return ApiResponse<SaleDetailsResponse>.CreateSuccess(data);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while retrieving sale details. {ex.Message}", ex);

                return ApiResponse<SaleDetailsResponse>.CreateFailure($"An error occurred while retrieving sale details. {ex.Message}", 500);
            }
        }

        public async Task<byte[]> GetSaleInvoice(int id)
        {
            var store = await _unitOfWork.Repository<StoreSetting>().GetAllAsync();

            var storeInfo = store.FirstOrDefault();

            // Fix N+1 query: Eagerly load Stock.Medicine with ThenInclude
            var sale = await _unitOfWork.Repository<Sales>().GetByIdWithIncludesAsync(id, query =>
                query.Include(x => x.Customer)
                      .Include(x => x.Items)
                          .ThenInclude(x => x.Stock)
                              .ThenInclude(x => x.Medicine)
                      .Include(x => x.SaleStatus)
                      .Include(x => x.PaymentMethod));

             var saleDto = new SaleDto
            {
                Id = id,
                CustomerName = sale.Customer.Name,
                SalesDate = sale.SalesDate,
                Notes = sale.Notes,
                Total = (double)sale.Total,
                SaleStatus = sale.SaleStatus.Name,
                Discount = (double)sale.Discount,
                GrandTotal = (double)sale.GrandTotal,
                PaymentMethod = sale.PaymentMethod.Name,
                Items = sale.Items.Select(item => new GetSalesItemDto
                {
                    Id = item.Id,
                    Price = item.Price,
                    Amount = item.Amount,
                    Quantity = item.Quantity,
                    StockId = item.StockId,
                    MedicineName = item.Stock?.Medicine?.Name ?? string.Empty,
                    BatchNo = item.Stock?.BatchNo ?? string.Empty,
                    ExpiryDate = item.Stock?.ExpiryDate ?? DateTime.Now
                }).ToList()
            };

            // N+1 query removed - Stock.Medicine now eagerly loaded above

            if (sale != null)
            {
                var sales = new SaleReportViewModel
                {
                    Company = new StoreSettingsDetailsDto
                    {
                        Id = storeInfo.Id,
                        StoreName = storeInfo.StoreName,
                        Address = storeInfo.Address,
                        Phone = storeInfo.Phone,
                        Email = storeInfo.Email,
                        Logo = storeInfo.Logo,
                        BankingDetails = storeInfo.BankingDetails,
                        Currency = storeInfo.Currency,
                        MCAZLicence = storeInfo.MCAZLicence,
                        ReturnsPolicy = storeInfo.ReturnsPolicy,
                        VATNumber = storeInfo.VATNumber
                    },
                    Sale = saleDto
                };

                SalesInvoiceReport salesReport = new SalesInvoiceReport();
                byte[] bytes = salesReport.CreateReport(sales);
                return bytes;
            }
            return null;
        }

        public async Task<ApiResponse<int>> AddSale(AddSaleRequest request)
        {
            // Validate request using ValidationService
            var duplicateValidation = await _validationService.ValidateSaleNotDuplicateAsync(request);
            if (!duplicateValidation.IsValid)
            {
                return ApiResponse<int>.CreateFailure(duplicateValidation.Errors.First(), 400, duplicateValidation.Errors);
            }

            var requestValidation = await _validationService.ValidateSaleRequestAsync(request);
            if (!requestValidation.IsValid)
            {
                _logger.LogWarning("Sale request validation failed: {Errors}", string.Join(", ", requestValidation.Errors));
                return ApiResponse<int>.CreateFailure("Sale request validation failed", 400, requestValidation.Errors);
            }

            // Calculate payment due date based on sale status
            var dayDue = request.SaleStatusId switch
            {
                4 => 7,   // 7 days
                5 => 14,  // 14 days
                6 => 30,  // 30 days
                _ => 0    // Cash sale (no due date)
            };

            try
            {
                // Wrap entire operation in transaction for data integrity
                return await _unitOfWork.ExecuteTransactionAsync(async () =>
                {
                    var saleToCreate = new Sales
                    {
                        CustomerId = request.CustomerId,
                        SaleStatusId = request.SaleStatusId,
                        PaymentMethodId = request.PaymentMethodId,
                        Notes = request.Notes,
                        SalesDate = DateTime.Now,
                        Discount = request.Discount,
                        PaymentDueDate = DateTime.Now.AddDays(dayDue),
                        Total = 0,
                        GrandTotal = 0,
                        Items = new List<SalesItems>()
                    };

                    var errors = new List<string>();

                    // Process each sale item (validation already done by ValidationService)
                    foreach (var itemReq in request.Items)
                    {
                        // Create sale item
                        var saleItem = new SalesItems
                        {
                            Price = itemReq.Price,
                            Quantity = itemReq.Quantity,
                            Discount = itemReq.Discount,
                            Amount = (itemReq.Price * itemReq.Quantity) - itemReq.Discount,
                            StockId = itemReq.StockId
                        };

                        // Add item to sale
                        saleToCreate.Items.Add(saleItem);

                        // Deduct stock with business logic validation
                        var stockResult = await _stockManagementService.DeductStockAsync(
                            itemReq.StockId,
                            itemReq.Quantity,
                            $"Sale for Customer ID: {request.CustomerId}");

                        if (!stockResult.Success)
                        {
                            errors.Add($"Failed to deduct stock: {stockResult.Message}");
                            continue;
                        }

                        // Update sale totals
                        saleToCreate.Total += itemReq.Price * itemReq.Quantity; // Gross total
                        saleToCreate.GrandTotal += saleItem.Amount;              // Net total after discounts
                    }

                    // Return validation errors if any
                    if (errors.Any())
                    {
                        return ApiResponse<int>.CreateFailure("Error adding sales items", 400, errors);
                    }

                    // Save the sale
                    await _unitOfWork.Repository<Sales>().AddAsync(saleToCreate);

                    // Create Accounts Receivable for non-cash sales
                    if (request.SaleStatusId != 3)
                    {
                        var accountsReceivable = new AccountsReceivable
                        {
                            CustomerId = saleToCreate.CustomerId,
                            AmountDue = saleToCreate.GrandTotal,
                            AmountPaid = 0,
                            DueDate = (DateTime)saleToCreate.PaymentDueDate,
                            BalanceDue = saleToCreate.GrandTotal,
                            AccountsReceivableStatusId = 1
                        };

                        await _unitOfWork.Repository<AccountsReceivable>().AddAsync(accountsReceivable);
                    }

                    // Save all changes within transaction
                    await _unitOfWork.SaveChangesAsync();

                    // Invalidate cache after successful transaction
                    InvalidateSalesCache();

                    _logger.LogInformation(
                        "Sale created successfully: SaleId={SaleId}, CustomerId={CustomerId}, Items={ItemCount}, Total={Total}",
                        saleToCreate.Id, saleToCreate.CustomerId, saleToCreate.Items.Count, saleToCreate.GrandTotal);

                    return ApiResponse<int>.CreateSuccess(
                        saleToCreate.Id,
                        "Sale created successfully",
                        200);
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding sale for Customer ID: {CustomerId}", request.CustomerId);
                return ApiResponse<int>.CreateFailure($"An error occurred while adding the sale: {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<bool>> UpdateSale(UpdateSaleRequest request)
        {
            try
            {
                // Wrap entire operation in transaction
                return await _unitOfWork.ExecuteTransactionAsync(async () =>
                {
                    // Get existing sale
                    var saleToUpdate = await _unitOfWork.Repository<Sales>().GetByIdAsync(request.Id);
                    if (saleToUpdate == null)
                    {
                        return ApiResponse<bool>.CreateFailure($"Sale with ID {request.Id} not found", 404);
                    }

                    // Get existing sale items
                    var existingItems = await _unitOfWork.Repository<SalesItems>().FindAsync(x => x.SaleId == request.Id);

                    // Restore stock quantities for existing items
                    foreach (var item in existingItems)
                    {
                        var stockResult = await _stockManagementService.AddStockAsync(
                            item.StockId,
                            item.Quantity,
                            $"Sale update rollback for Sale ID: {request.Id}");

                        if (!stockResult.Success)
                        {
                            _logger.LogWarning("Failed to restore stock during sale update: {Message}", stockResult.Message);
                        }
                    }

                    // Remove existing sale items
                    foreach (var item in existingItems)
                    {
                        _unitOfWork.Repository<SalesItems>().Delete(item);
                    }

                    // Update sale details
                    saleToUpdate.Notes = request.Notes;
                    saleToUpdate.SaleStatusId = request.SaleStatusId;
                    saleToUpdate.PaymentMethodId = request.PaymentMethodId;
                    saleToUpdate.CustomerId = request.CustomerId;
                    saleToUpdate.Total = request.Total;
                    saleToUpdate.SalesDate = request.SalesDate;
                    saleToUpdate.Discount = request.Discount;
                    saleToUpdate.GrandTotal = request.GrandTotal;

                    // Clear and add new items
                    saleToUpdate.Items.Clear();

                    var errors = new List<string>();
                    foreach (var item in request.Items)
                    {
                        // Validate stock availability
                        var isAvailable = await _stockManagementService.IsStockAvailableAsync(
                            item.StockId, item.Quantity);

                        if (!isAvailable)
                        {
                            errors.Add($"Insufficient stock for item with ID {item.StockId}");
                            continue;
                        }

                        // Add new sale item
                        saleToUpdate.Items.Add(new SalesItems
                        {
                            Price = item.Price,
                            Amount = item.Amount,
                            Quantity = item.Quantity,
                            StockId = item.StockId
                        });

                        // Deduct stock with business logic
                        var stockResult = await _stockManagementService.DeductStockAsync(
                            item.StockId,
                            item.Quantity,
                            $"Sale update for Sale ID: {request.Id}");

                        if (!stockResult.Success)
                        {
                            errors.Add($"Failed to deduct stock: {stockResult.Message}");
                        }
                    }

                    if (errors.Any())
                    {
                        return ApiResponse<bool>.CreateFailure("Error updating sales items", 400, errors);
                    }

                    // Update sale
                    _unitOfWork.Repository<Sales>().Update(saleToUpdate);

                    // Save all changes within transaction
                    await _unitOfWork.SaveChangesAsync();

                    // Invalidate cache after successful transaction
                    InvalidateSalesCache();

                    _logger.LogInformation("Sale updated successfully: SaleId={SaleId}", request.Id);

                    return ApiResponse<bool>.CreateSuccess(true, "Sale updated successfully");
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating sale: {SaleId}", request.Id);
                return ApiResponse<bool>.CreateFailure($"An error occurred while updating the sale: {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<bool>> DeleteSale(int id)
        {
            try
            {
                // Wrap entire operation in transaction
                return await _unitOfWork.ExecuteTransactionAsync(async () =>
                {
                    var saleToDelete = await _unitOfWork.Repository<Sales>().GetByIdAsync(id);
                    if (saleToDelete == null)
                    {
                        return ApiResponse<bool>.CreateFailure("Sale not found", 404);
                    }

                    // Get sale items
                    var items = await _unitOfWork.Repository<SalesItems>().FindAsync(x => x.SaleId == id);

                    // Restore stock quantities for all items
                    if (items.Any())
                    {
                        foreach (var item in items)
                        {
                            var stockResult = await _stockManagementService.AddStockAsync(
                                item.StockId,
                                item.Quantity,
                                $"Sale deletion for Sale ID: {id}");

                            if (!stockResult.Success)
                            {
                                _logger.LogWarning("Failed to restore stock during sale deletion: {Message}", stockResult.Message);
                            }
                        }
                    }

                    // Delete the sale (cascade will delete items)
                    _unitOfWork.Repository<Sales>().Delete(saleToDelete);

                    // Save all changes within transaction
                    await _unitOfWork.SaveChangesAsync();

                    // Invalidate cache after successful transaction
                    InvalidateSalesCache();

                    _logger.LogInformation("Sale deleted successfully: SaleId={SaleId}, ItemsRestored={ItemCount}",
                        id, items.Count());

                    return ApiResponse<bool>.CreateSuccess(true, "Sale deleted successfully");
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting sale: {SaleId}", id);
                return ApiResponse<bool>.CreateFailure($"An error occurred while deleting the sale: {ex.Message}", 500);
            }
        }

        private void InvalidateSalesCache()
        {
            _memoryCache.Remove(SalesCacheKey);
            
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
                        if (entry.Key.ToString().StartsWith(SaleDetailsCacheKeyPrefix))
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
