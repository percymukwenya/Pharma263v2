using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Pharma263.Api.Models.Purchase;
using Pharma263.Api.Models.Purchase.Request;
using Pharma263.Api.Models.Purchase.Response;
using Pharma263.Api.Shared.Contracts;
using Pharma263.Application.Contracts.Logging;
using Pharma263.Application.Models;
using Pharma263.Application.Services.Printing;
using Pharma263.Domain.Common;
using Pharma263.Domain.Entities;
using Pharma263.Domain.Interfaces.Repository;
using Pharma263.Domain.Models;
using Pharma263.Domain.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Pharma263.Api.Services
{
    public class PurchaseService : IScopedInjectedService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IStockRepository _stockRepository;
        private readonly IPurchaseCalculationService _purchaseCalculationService;
        private readonly IAppLogger<PurchaseService> _logger;
        private readonly IMemoryCache _cache;

        public PurchaseService(IUnitOfWork unitOfWork,
            IPurchaseRepository purchaseRepository, IStockRepository stockRepository,
            IPurchaseCalculationService purchaseCalculationService, IAppLogger<PurchaseService> logger, IMemoryCache cache)
        {
            _unitOfWork = unitOfWork;
            _purchaseRepository = purchaseRepository;
            _stockRepository = stockRepository;
            _purchaseCalculationService = purchaseCalculationService;
            _logger = logger;
            _cache = cache;
        }

        public async Task<ApiResponse<List<PurchaseListResponse>>> GetPurchases()
        {
            const string cacheKey = "purchases_all";

            try
            {
                // Try to get from cache first
                if (_cache.TryGetValue(cacheKey, out List<PurchaseListResponse> cachedPurchases))
                {
                    return ApiResponse<List<PurchaseListResponse>>.CreateSuccess(cachedPurchases);
                }

                var purchases = await _unitOfWork.Repository<Purchase>().GetAllAsync(query =>
                query.Include(p => p.PaymentMethod)
                      .Include(p => p.PurchaseStatus)
                      .Include(s => s.Supplier));

                var mappedPurchases = purchases.Select(p => new PurchaseListResponse
                {
                    Id = p.Id,
                    PurchaseDate = p.PurchaseDate,
                    Notes = p.Notes,
                    Total = (double)p.Total,
                    PaymentMethod = p.PaymentMethod.Name,
                    PurchaseStatus = p.PurchaseStatus.Name,
                    Discount = (double)p.Discount,
                    GrandTotal = (double)p.GrandTotal,
                    Supplier = p.Supplier.Name,
                    SupplierPhoneNumber = p.Supplier.Phone,
                    SupplierAddress = p.Supplier.Address
                }).ToList();

                // Cache for 5 minutes
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                    SlidingExpiration = TimeSpan.FromMinutes(2),
                    Priority = CacheItemPriority.Normal
                };

                _cache.Set(cacheKey, mappedPurchases, cacheEntryOptions);

                return ApiResponse<List<PurchaseListResponse>>.CreateSuccess(mappedPurchases);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"An error occurred while retrieving purchases. {ex.Message}", ex);

                return ApiResponse<List<PurchaseListResponse>>.CreateFailure($"An error occurred while retrieving purchases. {ex.Message}", 500);
            }

        }

        private void InvalidatePurchaseCache()
        {
            _cache.Remove("purchases_all");
        }

        public async Task<ApiResponse<PaginatedList<PurchaseListResponse>>> GetPurchasesPaged(PagedRequest request)
        {
            try
            {
                // Build filter expression
                Expression<Func<Purchase, bool>> filter = x => !x.IsDeleted;
                
                if (!string.IsNullOrWhiteSpace(request.SearchTerm))
                {
                    var searchTerm = request.SearchTerm.ToLower();
                    filter = x => !x.IsDeleted && (x.Notes.ToLower().Contains(searchTerm) ||
                                                  x.Supplier.Name.ToLower().Contains(searchTerm) ||
                                                  x.PaymentMethod.Name.ToLower().Contains(searchTerm) ||
                                                  x.PurchaseStatus.Name.ToLower().Contains(searchTerm));
                }

                // Build sorting
                Func<IQueryable<Purchase>, IOrderedQueryable<Purchase>> orderBy = null;
                if (!string.IsNullOrWhiteSpace(request.SortBy))
                {
                    switch (request.SortBy.ToLower())
                    {
                        case "purchasedate":
                            orderBy = request.SortDescending ? q => q.OrderByDescending(x => x.PurchaseDate) : q => q.OrderBy(x => x.PurchaseDate);
                            break;
                        case "total":
                            orderBy = request.SortDescending ? q => q.OrderByDescending(x => x.Total) : q => q.OrderBy(x => x.Total);
                            break;
                        case "grandtotal":
                            orderBy = request.SortDescending ? q => q.OrderByDescending(x => x.GrandTotal) : q => q.OrderBy(x => x.GrandTotal);
                            break;
                        case "supplier":
                            orderBy = request.SortDescending ? q => q.OrderByDescending(x => x.Supplier.Name) : q => q.OrderBy(x => x.Supplier.Name);
                            break;
                        case "status":
                            orderBy = request.SortDescending ? q => q.OrderByDescending(x => x.PurchaseStatus.Name) : q => q.OrderBy(x => x.PurchaseStatus.Name);
                            break;
                        case "createddate":
                            orderBy = request.SortDescending ? q => q.OrderByDescending(x => x.CreatedDate) : q => q.OrderBy(x => x.CreatedDate);
                            break;
                        default:
                            orderBy = q => q.OrderByDescending(x => x.PurchaseDate);
                            break;
                    }
                }
                else
                {
                    orderBy = q => q.OrderByDescending(x => x.PurchaseDate);
                }

                // Include relationships for pagination
                Func<IQueryable<Purchase>, IQueryable<Purchase>> include = query =>
                    query.Include(p => p.PaymentMethod)
                         .Include(p => p.PurchaseStatus)
                         .Include(s => s.Supplier);

                // Get paginated results using existing method
                var paginatedPurchases = await _unitOfWork.Repository<Purchase>()
                    .GetPaginatedAsync(request.Page, request.PageSize, filter, orderBy, include);

                var mappedPurchases = paginatedPurchases.Items.Select(p => new PurchaseListResponse
                {
                    Id = p.Id,
                    PurchaseDate = p.PurchaseDate,
                    Notes = p.Notes,
                    Total = (double)p.Total,
                    PaymentMethod = p.PaymentMethod.Name,
                    PurchaseStatus = p.PurchaseStatus.Name,
                    Discount = (double)p.Discount,
                    GrandTotal = (double)p.GrandTotal,
                    Supplier = p.Supplier.Name,
                    SupplierPhoneNumber = p.Supplier.Phone,
                    SupplierAddress = p.Supplier.Address
                }).ToList();

                var result = new PaginatedList<PurchaseListResponse>(
                    mappedPurchases, paginatedPurchases.TotalCount, paginatedPurchases.PageIndex, request.PageSize);

                return ApiResponse<PaginatedList<PurchaseListResponse>>.CreateSuccess(result);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Error retrieving paginated purchases", ex);
                return ApiResponse<PaginatedList<PurchaseListResponse>>.CreateFailure($"Failed to retrieve purchases. {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<PurchaseDetailsResponse>> GetPurchase(int id)
        {
            try
            {
                var purchase = await _unitOfWork.Repository<Purchase>().GetByIdWithIncludesAsync(id, query =>
                    query.Include(i => i.Items)
                        .Include(p => p.PaymentMethod)
                        .Include(p => p.PurchaseStatus)
                        .Include(s => s.Supplier));

                var mappedPurchase = new PurchaseDetailsResponse
                {
                    Id = id,
                    PurchaseDate = purchase.PurchaseDate,
                    Notes = purchase.Notes,
                    Total = (double)purchase.Total,
                    PaymentMethodId = purchase.PaymentMethodId,
                    PaymentMethod = purchase.PaymentMethod.Name,
                    PurchaseStatusId = purchase.PurchaseStatusId,
                    PurchaseStatus = purchase.PurchaseStatus.Name,
                    Discount = (double)purchase.Discount,
                    GrandTotal = (double)purchase.GrandTotal,
                    SupplierId = purchase.SupplierId,
                    Supplier = purchase.Supplier.Name,
                    SupplierPhoneNumber = purchase.Supplier.Phone,
                    SupplierAddress = purchase.Supplier.Address,
                    Items = [.. purchase.Items.Select(i => new PurchaseItemModel
                {
                    MedicineId = i.MedicineId,
                    BatchNo = i.BatchNo,
                    Quantity = i.Quantity,
                    Price = i.Price,
                    Discount = i.Discount,
                    Amount = i.Amount
                })]
                };

                foreach (var item in mappedPurchase.Items)
                {
                    var stockItem = await _unitOfWork.Repository<Stock>().FirstOrDefaultAsync(x => x.MedicineId == item.MedicineId && x.BatchNo == item.BatchNo, query => query.Include(m => m.Medicine));
                    item.MedicineName = stockItem.Medicine.Name;
                    item.ExpiryDate = stockItem.ExpiryDate;
                    item.BuyingPrice = stockItem.BuyingPrice;
                    item.SellingPrice = stockItem.SellingPrice;
                }

                return ApiResponse<PurchaseDetailsResponse>.CreateSuccess(mappedPurchase);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"An error occurred while retrieving purchase. {ex.Message}", ex);

                return ApiResponse<PurchaseDetailsResponse>.CreateFailure($"An error occurred while retrieving purchase. {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<bool>> AddPurchase(AddPurchaseRequest request)
        {
            try
            {
                var purchaseToCreate = new Purchase
                {
                    PurchaseDate = DateTime.Now,
                    Notes = request.Notes,
                    PaymentMethodId = request.PaymentMethodId,
                    PurchaseStatusId = request.PurchaseStatusId,
                    SupplierId = request.SupplierId,
                    Discount = (decimal)request.Discount,
                    GrandTotal = (decimal)request.GrandTotal,
                    Total = (decimal)request.Total
                };

                if (await _purchaseRepository.IsDuplicate(purchaseToCreate))
                    return ApiResponse<bool>.CreateFailure("Purchase is a duplicate", 400);

                DateTime now = DateTime.Now;
                string transactionDate = now.ToString("yyyyMMddHHmm");

                var transactionId = $"{transactionDate}";

                int dayDue = request.PurchaseStatusId switch
                {
                    4 => 7,
                    5 => 14,
                    6 => 30,
                    _ => 0
                };

                purchaseToCreate.PaymentDueDate = DateTime.Now.AddDays(dayDue);

                foreach (var item in request.Items)
                {
                    var stock = await _unitOfWork.Repository<Stock>().FirstOrDefaultAsync(x => x.MedicineId == item.MedicineId && x.BatchNo == item.BatchNo);

                    if (stock != null)
                    {
                        // update quantity
                        await _stockRepository.AddQuantity(item.Quantity, stock.Id);
                    }
                    else
                    {   // add new stock
                        await _unitOfWork.Repository<Stock>().AddAsync(new Stock
                        {
                            TotalQuantity = item.Quantity,
                            NotifyForQuantityBelow = SettingsModel.NotifyDefaultQuantity,
                            MedicineId = item.MedicineId,
                            BatchNo = item.BatchNo,
                            ExpiryDate = item.ExpiryDate,
                            BuyingPrice = item.BuyingPrice,
                            SellingPrice = item.SellingPrice
                        });
                    }

                    purchaseToCreate.Items.Add(new PurchaseItems
                    {
                        Amount = item.Amount,
                        BatchNo = item.BatchNo,
                        Discount = item.Discount,
                        MedicineId = item.MedicineId,
                        Price = item.Price,
                        Quantity = item.Quantity
                    });
                }

                // add to database
                await _unitOfWork.Repository<Purchase>().AddAsync(purchaseToCreate);
                await _unitOfWork.SaveChangesAsync();

                if (request.PurchaseStatusId != 3)
                {
                    var obj = new AccountsPayable
                    {
                        SupplierId = purchaseToCreate.SupplierId,
                        AmountOwed = purchaseToCreate.GrandTotal,
                        AmountPaid = 0,
                        BalanceOwed = purchaseToCreate.GrandTotal,
                        DueDate = (DateTime)purchaseToCreate.PaymentDueDate,
                        AccountsPayableStatusId = 1
                    };

                    await _unitOfWork.Repository<AccountsPayable>().AddAsync(obj);
                    await _unitOfWork.SaveChangesAsync();
                }

                // Invalidate cache after successful creation
                InvalidatePurchaseCache();

                // return record id
                return ApiResponse<bool>.CreateSuccess(true, "Purchase created successfully");
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"An error occurred while adding purchase. {ex.Message}", ex);

                return ApiResponse<bool>.CreateFailure($"An error occurred while adding purchase. {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<bool>> UpdatePurchase(UpdatePurchaseRequest request)
        {
            try
            {
                var purchaseToUpdate = await _unitOfWork.Repository<Purchase>()
                .GetByIdWithIncludesAsync(request.Id, query => query.Include(p => p.Items));

                if (purchaseToUpdate == null)
                    return ApiResponse<bool>.CreateFailure("Purchase not found", 404);

                foreach (var oldItem in purchaseToUpdate.Items)
                {
                    var stock = await _unitOfWork.Repository<Stock>().FirstOrDefaultAsync(
                        x => x.MedicineId == oldItem.MedicineId && x.BatchNo == oldItem.BatchNo);

                    if (stock != null)
                    {
                        // Subtract the quantity that was added when the purchase was created
                        await _stockRepository.SubQuantity(oldItem.Quantity, stock.Id);
                    }
                }

                purchaseToUpdate.Items.Clear();

                purchaseToUpdate.Notes = request.Notes;
                purchaseToUpdate.PaymentMethodId = request.PaymentMethodId;
                purchaseToUpdate.PurchaseStatusId = request.PurchaseStatusId;
                purchaseToUpdate.Total = 0;
                purchaseToUpdate.GrandTotal = 0;

                foreach (var itemReq in request.Items)
                {
                    // Calculate the net amount using the business rules service.
                    decimal netAmount = _purchaseCalculationService.CalculateLineNetAmount(itemReq.Price, itemReq.Quantity, itemReq.Discount);

                    // Create a new purchase item with the new per-item discount
                    var newItem = new PurchaseItems
                    {
                        Price = itemReq.Price,
                        Quantity = itemReq.Quantity,
                        Discount = itemReq.Discount,
                        Amount = netAmount,
                        MedicineId = itemReq.MedicineId,
                        BatchNo = itemReq.BatchNo
                    };

                    // Add the new item to the purchase
                    purchaseToUpdate.Items.Add(newItem);

                    // Update stock based on new purchase items:
                    var stock = await _unitOfWork.Repository<Stock>().FirstOrDefaultAsync(
                        x => x.MedicineId == itemReq.MedicineId && x.BatchNo == itemReq.BatchNo);

                    if (stock != null)
                    {
                        // Add quantity to the stock to reflect the updated purchase
                        await _stockRepository.AddQuantity(itemReq.Quantity, stock.Id);
                    }
                    else
                    {
                        // If no stock record exists for this batch, add a new one
                        await _unitOfWork.Repository<Stock>().AddAsync(new Stock
                        {
                            TotalQuantity = itemReq.Quantity,
                            NotifyForQuantityBelow = SettingsModel.NotifyDefaultQuantity,
                            MedicineId = itemReq.MedicineId,
                            BatchNo = itemReq.BatchNo,
                            ExpiryDate = itemReq.ExpiryDate,
                            BuyingPrice = itemReq.BuyingPrice,
                            SellingPrice = itemReq.SellingPrice
                        });
                    }
                }

                _purchaseCalculationService.RecalculateTotals(purchaseToUpdate);

                // Update the purchase record in the repository
                _unitOfWork.Repository<Purchase>().Update(purchaseToUpdate);

                // Save all changes in one transaction
                var result = await _unitOfWork.SaveChangesAsync();

                if (result <= 0)
                    return ApiResponse<bool>.CreateFailure("Failed to update purchase", 500);

                // Invalidate cache after successful update
                InvalidatePurchaseCache();

                return ApiResponse<bool>.CreateSuccess(true, "Purchase updated successfully");
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"An error occurred while updating purchase. {ex.Message}", ex);

                return ApiResponse<bool>.CreateFailure($"An error occurred while updating purchase. {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<bool>> DeletePurchase(int id)
        {
            try
            {
                var purchaseToDelete = await _unitOfWork.Repository<Purchase>().GetByIdAsync(id);

                if (purchaseToDelete == null) return ApiResponse<bool>.CreateFailure("Purchase not found", 404);

                var items = await _unitOfWork.Repository<PurchaseItems>().FindAsync(x => x.PurchaseId == id);

                if (items.Any())
                {
                    // subtract item quantity from stock
                    foreach (var item in items)
                    {
                        var stock = await _unitOfWork.Repository<Stock>().FirstOrDefaultAsync(x => x.MedicineId == item.MedicineId);

                        if (stock != null)
                            await _stockRepository.SubQuantity(item.Quantity, stock.Id);
                    }
                }

                // remove from database
                _unitOfWork.Repository<Purchase>().Delete(purchaseToDelete);

                var result = await _unitOfWork.SaveChangesAsync();

                if (result <= 0)
                    return ApiResponse<bool>.CreateFailure("Failed to delete purchase", 500);

                // Invalidate cache after successful deletion
                InvalidatePurchaseCache();

                return ApiResponse<bool>.CreateSuccess(true, "Purchase deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"An error occurred while deleting purchase. {ex.Message}", ex);

                return ApiResponse<bool>.CreateFailure($"An error occurred while deleting purchase. {ex.Message}", 500);
            }
        }

        public async Task<byte[]> GetPurchaseInvoice(int id)
        {
            var store = await _unitOfWork.Repository<StoreSetting>().GetAllAsync();

            var storeInfo = store.FirstOrDefault();

            var purchase = await _purchaseRepository.GetByIdWithIncludesAsync(id,
                query => query.Include(i => i.Items).Include(s => s.Supplier).Include(x => x.PurchaseStatus).Include(x => x.PaymentMethod));

            var purchaseDto = new PurchaseDto
            {
                Id = purchase.Id,
                PurchaseDate = purchase.PurchaseDate,
                Notes = purchase.Notes,
                Total = (double)purchase.Total,
                PaymentMethod = purchase.PaymentMethod.Name,
                PurchaseStatus = purchase.PurchaseStatus.Name,
                Discount = (double)purchase.Discount,
                GrandTotal = (double)purchase.GrandTotal,
                Supplier = purchase.Supplier.Name,
                SupplierPhoneNumber = purchase.Supplier.Phone,
                SupplierAddress = purchase.Supplier.Address,
                Items = [.. purchase.Items.Select(i => new PurchaseItemsDto
                {
                    MedicineId = i.MedicineId,
                    BatchNo = i.BatchNo,
                    Quantity = i.Quantity,
                    Price = i.Price,
                    Discount = i.Discount,
                    Amount = i.Amount
                })]
            };

            foreach (var item in purchaseDto.Items)
            {
                var medicine = await _unitOfWork.Repository<Medicine>().GetByIdAsync(item.MedicineId);

                item.MedicineName = medicine.Name;
            }

            if (purchase != null)
            {
                var purchasees = new PurchaseReportViewModel
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
                    Purchase = purchaseDto
                };

                PurchaseInvoiceReportNew paymentReport = new PurchaseInvoiceReportNew();

                byte[] bytes = paymentReport.CreateReport(purchasees);

                return bytes;
            }

            return null;
        }
        private async Task<Stock> GetStockAsync(int medicineId, string batchNo)
        {
            return await _unitOfWork.Repository<Stock>().FirstOrDefaultAsync(
                x => x.MedicineId == medicineId && x.BatchNo == batchNo);
        }

        private async Task UpdateStockQuantityAsync(int quantity, int stockId, bool isAdding)
        {
            if (isAdding)
                await _stockRepository.AddQuantity(quantity, stockId);
            else
                await _stockRepository.SubQuantity(quantity, stockId);
        }
    }
}
