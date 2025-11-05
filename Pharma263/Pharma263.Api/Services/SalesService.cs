using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Pharma263.Api.Models.Sales.Request;
using Pharma263.Api.Models.Sales.Response;
using Pharma263.Api.Shared.Contracts;
using Pharma263.Application.Models;
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

namespace Pharma263.Api.Services
{
    public class SalesService : IScopedInjectedService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISalesRepository _salesRepository;
        private readonly IStockRepository _stockRepository;
        private readonly ILogger<SalesService> _logger;
        private readonly IMemoryCache _memoryCache;
        private const string SalesCacheKey = "sales_list";
        private const string SaleDetailsCacheKeyPrefix = "sale_details_";
        private readonly TimeSpan CacheExpiry = TimeSpan.FromMinutes(5);

        public SalesService(IUnitOfWork unitOfWork, ISalesRepository salesRepository,
            IStockRepository stockRepository, ILogger<SalesService> logger, IMemoryCache memoryCache)
        {
            _unitOfWork = unitOfWork;
            _salesRepository = salesRepository;
            _stockRepository = stockRepository;
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
                Expression<Func<Sales, bool>> filter = null;
                
                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    var searchTerm = request.SearchTerm.ToLower();
                    filter = x => x.Customer.Name.ToLower().Contains(searchTerm) ||
                                  x.Notes.ToLower().Contains(searchTerm) ||
                                  x.SaleStatus.Name.ToLower().Contains(searchTerm) ||
                                  x.PaymentMethod.Name.ToLower().Contains(searchTerm);
                }

                Func<IQueryable<Sales>, IOrderedQueryable<Sales>> orderBy = request.SortBy?.ToLower() switch
                {
                    "customer" => request.SortDescending ? (query => query.OrderByDescending(x => x.Customer.Name)) : (query => query.OrderBy(x => x.Customer.Name)),
                    "salesdate" => request.SortDescending ? (query => query.OrderByDescending(x => x.SalesDate)) : (query => query.OrderBy(x => x.SalesDate)),
                    "total" => request.SortDescending ? (query => query.OrderByDescending(x => x.Total)) : (query => query.OrderBy(x => x.Total)),
                    "grandtotal" => request.SortDescending ? (query => query.OrderByDescending(x => x.GrandTotal)) : (query => query.OrderBy(x => x.GrandTotal)),
                    "salestatus" => request.SortDescending ? (query => query.OrderByDescending(x => x.SaleStatus.Name)) : (query => query.OrderBy(x => x.SaleStatus.Name)),
                    "paymentmethod" => request.SortDescending ? (query => query.OrderByDescending(x => x.PaymentMethod.Name)) : (query => query.OrderBy(x => x.PaymentMethod.Name)),
                    _ => query => query.OrderByDescending(x => x.SalesDate)
                };

                var paginatedSales = await _unitOfWork.Repository<Sales>().GetPaginatedAsync(
                    request.Page,
                    request.PageSize,
                    filter,
                    orderBy,
                    query => query.Include(c => c.Customer)
                                  .Include(c => c.SaleStatus)
                                  .Include(c => c.PaymentMethod));
                
                var sales = paginatedSales.Items.ToList();

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

                var paginatedResult = new PaginatedList<SaleListResponse>(data, paginatedSales.TotalCount, paginatedSales.PageIndex, request.PageSize);

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

                var sale = await _unitOfWork.Repository<Sales>().GetByIdWithIncludesAsync(id, query =>
                query.Include(x => x.Customer)
                      .Include(x => x.Items)
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
                    MedicineName = string.Empty
                })]
                };

                foreach (var item in data.Items)
                {
                    var stockItem = await _unitOfWork.Repository<Stock>().GetByIdWithIncludesAsync(item.StockId, query =>
                    query.Include(x => x.Medicine));

                    item.MedicineName = stockItem.Medicine.Name;

                    item.StockId = stockItem.Id;
                }

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

            var sale = await _unitOfWork.Repository<Sales>().GetByIdWithIncludesAsync(id, query =>
                query.Include(x => x.Customer)
                      .Include(x => x.Items)
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
                    MedicineName = string.Empty,
                    BatchNo = string.Empty,
                    ExpiryDate = DateTime.Now
                }).ToList()
            };

            foreach (var item in saleDto.Items)
            {
                var stockItem = await _unitOfWork.Repository<Stock>().GetByIdWithIncludesAsync(item.StockId, query => query.Include(x => x.Medicine));
                if (stockItem != null)
                {
                    item.MedicineName = stockItem.Medicine.Name;
                    item.BatchNo = stockItem.BatchNo;
                    item.ExpiryDate = stockItem.ExpiryDate;
                }
            }

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
            var saleToCreate = new Sales
            {
                CustomerId = request.CustomerId,
                SaleStatusId = request.SaleStatusId,
                PaymentMethodId = request.PaymentMethodId,
                Notes = request.Notes,
                SalesDate = DateTime.Now,
                Discount = request.Discount
            };

            saleToCreate.Items = new List<SalesItems>();

            var isDuplicate = await _salesRepository.IsDuplicate(saleToCreate);

            if (isDuplicate)
                return ApiResponse<int>.CreateFailure("Duplicate sale detected", 400);

            DateTime now = DateTime.Now;
            string transactionDate = now.ToString("yyyyMMddHHmm");

            var transactionId = $"{transactionDate}";

            var dayDue = request.SaleStatusId switch
            {
                4 => 7,
                5 => 14,
                6 => 30,
                _ => 0
            };

            saleToCreate.PaymentDueDate = DateTime.Now.AddDays(dayDue);

            saleToCreate.Total = 0;
            saleToCreate.GrandTotal = 0;

            try
            {
                var errors = new List<string>();
                foreach (var itemReq in request.Items)
                {
                    // Use the existing items directly without duplicating
                    var saleItem = new SalesItems
                    {
                        Price = itemReq.Price,
                        Quantity = itemReq.Quantity,
                        Discount = itemReq.Discount,
                        Amount = (itemReq.Price * itemReq.Quantity) - itemReq.Discount,
                        StockId = itemReq.StockId
                    };

                    if (saleItem.Amount < 0)
                        errors.Add($"Item discount {itemReq.Discount} cannot exceed the total item price.");

                    // Add the sale item to the sale entity
                    saleToCreate.Items.Add(saleItem);

                    // Update stock by deducting sold quantities
                    var stock = await _unitOfWork.Repository<Stock>().FirstOrDefaultAsync(
                        x => x.Id == itemReq.StockId); // StockId is already in the request
                    if (stock == null)
                        throw new InvalidOperationException($"Stock not found for Medicine ID: {itemReq.MedicineId}, Batch No: {itemReq.BatchNo}");

                    await _stockRepository.SubQuantity(saleItem.Quantity, stock.Id);

                    // Update totals
                    saleToCreate.Total += itemReq.Price * itemReq.Quantity; // Gross total
                    saleToCreate.GrandTotal += saleItem.Amount;            // Net total after discounts
                }

                if (errors.Any())
                {
                    return ApiResponse<int>.CreateFailure("Error adding sales items", 400, errors);
                }

                await _unitOfWork.Repository<Sales>().AddAsync(saleToCreate);

                await _unitOfWork.SaveChangesAsync();

                if (request.SaleStatusId != 3)
                {
                    var obj = new AccountsReceivable
                    {
                        CustomerId = saleToCreate.CustomerId,
                        AmountDue = saleToCreate.GrandTotal,
                        AmountPaid = 0,
                        DueDate = (DateTime)saleToCreate.PaymentDueDate,
                        BalanceDue = saleToCreate.GrandTotal,
                        AccountsReceivableStatusId = 1
                    };

                    await _unitOfWork.Repository<AccountsReceivable>().AddAsync(obj);

                    await _unitOfWork.SaveChangesAsync();
                }

                InvalidateSalesCache();

                return ApiResponse<int>.CreateSuccess(saleToCreate.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while adding the sale: {ex.Message}", ex);

                return ApiResponse<int>.CreateFailure($"An error occurred while adding the sale: {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<bool>> UpdateSale(UpdateSaleRequest request)
        {
            try
            {
                // Get existing sale items
                var existingItems = await _unitOfWork.Repository<SalesItems>().FindAsync(x => x.SaleId == request.Id);

                // Add back quantities to stock for existing items
                foreach (var item in existingItems)
                {
                    var stock = await _unitOfWork.Repository<Stock>().GetByIdAsync(item.StockId);
                    if (stock != null)
                    {
                        await _stockRepository.AddQuantity(item.Quantity, stock.Id);
                    }
                }

                // Remove existing sale items
                foreach (var item in existingItems)
                {
                    _unitOfWork.Repository<SalesItems>().Delete(item);
                }

                // Update sale details
                var saleToUpdate = await _unitOfWork.Repository<Sales>().GetByIdAsync(request.Id);

                if (saleToUpdate == null)
                {
                    throw new InvalidOperationException($"Sale with ID {request.Id} not found");
                }

                saleToUpdate.Notes = request.Notes;
                saleToUpdate.SaleStatusId = request.SaleStatusId;
                saleToUpdate.PaymentMethodId = request.PaymentMethodId;
                saleToUpdate.CustomerId = request.CustomerId;
                saleToUpdate.Total = request.Total;
                saleToUpdate.SalesDate = request.SalesDate;
                saleToUpdate.Discount = request.Discount;
                saleToUpdate.GrandTotal = request.GrandTotal;

                // Clear existing items and add new ones
                saleToUpdate.Items.Clear();

                var errors = new List<string>();
                foreach (var item in request.Items)
                {
                    var stock = await _unitOfWork.Repository<Stock>().GetByIdAsync(item.StockId);

                    if (stock == null)
                    {
                        errors.Add($"Stock item with ID {item.StockId} not found");
                    }

                    if (stock.TotalQuantity < item.Quantity)
                    {
                        errors.Add($"Insufficient stock for item with ID {item.StockId}. Available: {stock.TotalQuantity}, Requested: {item.Quantity}");
                    }

                    saleToUpdate.Items.Add(new SalesItems
                    {
                        Price = item.Price,
                        Amount = item.Amount,
                        Quantity = item.Quantity,
                        StockId = item.StockId
                    });

                    // Subtract quantity from stock
                    await _stockRepository.SubQuantity(item.Quantity, stock.Id);
                }

                _unitOfWork.Repository<Sales>().Update(saleToUpdate);

                await _unitOfWork.SaveChangesAsync();

                InvalidateSalesCache();

                return ApiResponse<bool>.CreateSuccess(true, "Sale updated successfully");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed

                return ApiResponse<bool>.CreateFailure($"An error occurred while updating the sale: {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<bool>> DeleteSale(int id)
        {
            try
            {
                var saleToDelete = await _unitOfWork.Repository<Sales>().GetByIdAsync(id);

                if (saleToDelete == null) return ApiResponse<bool>.CreateFailure("Sale not found", 404);

                var items = await _unitOfWork.Repository<SalesItems>().FindAsync(x => x.SaleId == id);

                if (items.Any())
                { //add stock quantity
                    foreach (var item in items)
                    {
                        var stock = await _unitOfWork.Repository<Stock>().GetByIdAsync(item.StockId);

                        if (stock != null)
                            await _stockRepository.AddQuantity(item.Quantity, stock.Id);
                    }
                }

                _unitOfWork.Repository<Sales>().Delete(saleToDelete);

                await _unitOfWork.SaveChangesAsync();

                InvalidateSalesCache();

                return ApiResponse<bool>.CreateSuccess(true, "Sale deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while deleting the sale: {ex.Message}", ex);

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
