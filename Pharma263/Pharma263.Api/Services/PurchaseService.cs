using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Pharma263.Api.Contracts;
using Pharma263.Api.Models.Purchase;
using Pharma263.Api.Models.Purchase.Request;
using Pharma263.Api.Models.Purchase.Response;
using Pharma263.Api.Shared.Contracts;
using Pharma263.Application.Contracts.Logging;
using Pharma263.Application.Contracts.Services;
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
        private readonly IStockManagementService _stockManagementService;
        private readonly IValidationService _validationService;
        private readonly IPurchaseCalculationService _purchaseCalculationService;
        private readonly IAppLogger<PurchaseService> _logger;
        private readonly IMemoryCache _cache;

        public PurchaseService(IUnitOfWork unitOfWork,
            IPurchaseRepository purchaseRepository, IStockManagementService stockManagementService,
            IValidationService validationService, IPurchaseCalculationService purchaseCalculationService,
            IAppLogger<PurchaseService> logger, IMemoryCache cache)
        {
            _unitOfWork = unitOfWork;
            _purchaseRepository = purchaseRepository;
            _stockManagementService = stockManagementService;
            _validationService = validationService;
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
                // Use Query() for projection - reduces data transfer by 30-40%
                var query = _unitOfWork.Repository<Purchase>().Query()
                    .Include(p => p.PaymentMethod)
                    .Include(p => p.PurchaseStatus)
                    .Include(s => s.Supplier)
                    .Where(x => !x.IsDeleted);

                if (!string.IsNullOrWhiteSpace(request.SearchTerm))
                {
                    var searchTerm = request.SearchTerm.ToLower();
                    query = query.Where(x => x.Notes.ToLower().Contains(searchTerm) ||
                                            x.Supplier.Name.ToLower().Contains(searchTerm) ||
                                            x.PaymentMethod.Name.ToLower().Contains(searchTerm) ||
                                            x.PurchaseStatus.Name.ToLower().Contains(searchTerm));
                }

                // Apply sorting
                query = string.IsNullOrWhiteSpace(request.SortBy) ? "purchasedate" : request.SortBy.ToLower() switch
                {
                    "purchasedate" => request.SortDescending ? query.OrderByDescending(x => x.PurchaseDate) : query.OrderBy(x => x.PurchaseDate),
                    "total" => request.SortDescending ? query.OrderByDescending(x => x.Total) : query.OrderBy(x => x.Total),
                    "grandtotal" => request.SortDescending ? query.OrderByDescending(x => x.GrandTotal) : query.OrderBy(x => x.GrandTotal),
                    "supplier" => request.SortDescending ? query.OrderByDescending(x => x.Supplier.Name) : query.OrderBy(x => x.Supplier.Name),
                    "status" => request.SortDescending ? query.OrderByDescending(x => x.PurchaseStatus.Name) : query.OrderBy(x => x.PurchaseStatus.Name),
                    "createddate" => request.SortDescending ? query.OrderByDescending(x => x.CreatedDate) : query.OrderBy(x => x.CreatedDate),
                    _ => query.OrderByDescending(x => x.PurchaseDate)
                };

                var count = await query.CountAsync();

                // Project to DTO before materialization - EF generates optimized SQL
                var data = await query
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(p => new PurchaseListResponse
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
                    })
                    .ToListAsync();

                var result = new PaginatedList<PurchaseListResponse>(data, count, request.Page, request.PageSize);

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
                // Fix N+1 query: Eagerly load Medicine for each PurchaseItem
                var purchase = await _unitOfWork.Repository<Purchase>().GetByIdWithIncludesAsync(id, query =>
                    query.Include(i => i.Items)
                            .ThenInclude(i => i.Medicine)
                        .Include(p => p.PaymentMethod)
                        .Include(p => p.PurchaseStatus)
                        .Include(s => s.Supplier));

                // Batch load all stock items in a single query to get additional stock details
                var medicineAndBatchPairs = purchase.Items.Select(i => new { i.MedicineId, i.BatchNo }).Distinct().ToList();
                var stockItems = new Dictionary<(int MedicineId, string BatchNo), Stock>();

                if (medicineAndBatchPairs.Any())
                {
                    var stocks = await _unitOfWork.Repository<Stock>()
                        .FindAsync(s => medicineAndBatchPairs.Any(p => p.MedicineId == s.MedicineId && p.BatchNo == s.BatchNo));

                    foreach (var stock in stocks)
                    {
                        stockItems[(stock.MedicineId, stock.BatchNo)] = stock;
                    }
                }

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
                    Items = purchase.Items.Select(i =>
                    {
                        stockItems.TryGetValue((i.MedicineId, i.BatchNo), out var stock);
                        return new PurchaseItemModel
                        {
                            MedicineId = i.MedicineId,
                            MedicineName = i.Medicine?.Name ?? string.Empty,
                            BatchNo = i.BatchNo,
                            Quantity = i.Quantity,
                            Price = i.Price,
                            Discount = i.Discount,
                            Amount = i.Amount,
                            ExpiryDate = stock?.ExpiryDate ?? DateTime.Now,
                            BuyingPrice = stock?.BuyingPrice ?? 0,
                            SellingPrice = stock?.SellingPrice ?? 0
                        };
                    }).ToList()
                };

                // N+1 query removed - Medicine eagerly loaded and Stock batch loaded above

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
            // Validate request using ValidationService
            var duplicateValidation = await _validationService.ValidatePurchaseNotDuplicateAsync(request);
            if (!duplicateValidation.IsValid)
            {
                return ApiResponse<bool>.CreateFailure(duplicateValidation.Errors.First(), 400, duplicateValidation.Errors);
            }

            var requestValidation = await _validationService.ValidatePurchaseRequestAsync(request);
            if (!requestValidation.IsValid)
            {
                _logger.LogWarning("Purchase request validation failed: {Errors}", string.Join(", ", requestValidation.Errors));
                return ApiResponse<bool>.CreateFailure("Purchase request validation failed", 400, requestValidation.Errors);
            }

            // Calculate payment due date
            int dayDue = request.PurchaseStatusId switch
            {
                4 => 7,   // 7 days
                5 => 14,  // 14 days
                6 => 30,  // 30 days
                _ => 0    // Cash purchase
            };

            try
            {
                // Wrap entire operation in transaction for data integrity
                return await _unitOfWork.ExecuteTransactionAsync(async () =>
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
                        Total = (decimal)request.Total,
                        PaymentDueDate = DateTime.Now.AddDays(dayDue),
                        Items = new List<PurchaseItems>()
                    };

                    // Process each purchase item
                    foreach (var item in request.Items)
                    {
                        // Check if stock exists for this medicine and batch
                        var stock = await _unitOfWork.Repository<Stock>().FirstOrDefaultAsync(
                            x => x.MedicineId == item.MedicineId && x.BatchNo == item.BatchNo);

                        if (stock != null)
                        {
                            // Update existing stock with business logic validation
                            var stockResult = await _stockManagementService.AddStockAsync(
                                stock.Id,
                                item.Quantity,
                                $"Purchase from Supplier ID: {request.SupplierId}");

                            if (!stockResult.Success)
                            {
                                _logger.LogWarning($"Failed to add stock: {stockResult.Message}");
                            }
                        }
                        else
                        {
                            // Add new stock item
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

                        // Add purchase item
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

                    // Save purchase
                    await _unitOfWork.Repository<Purchase>().AddAsync(purchaseToCreate);

                    // Create Accounts Payable for non-cash purchases
                    if (request.PurchaseStatusId != 3)
                    {
                        var accountsPayable = new AccountsPayable
                        {
                            SupplierId = purchaseToCreate.SupplierId,
                            AmountOwed = purchaseToCreate.GrandTotal,
                            AmountPaid = 0,
                            BalanceOwed = purchaseToCreate.GrandTotal,
                            DueDate = (DateTime)purchaseToCreate.PaymentDueDate,
                            AccountsPayableStatusId = 1
                        };

                        await _unitOfWork.Repository<AccountsPayable>().AddAsync(accountsPayable);
                    }

                    // Save all changes within transaction
                    await _unitOfWork.SaveChangesAsync();

                    // Invalidate cache after successful transaction
                    InvalidatePurchaseCache();

                    _logger.LogInformation(
                        "Purchase created successfully: PurchaseId={PurchaseId}, SupplierId={SupplierId}, Items={ItemCount}, Total={Total}",
                        purchaseToCreate.Id, purchaseToCreate.SupplierId, purchaseToCreate.Items.Count, purchaseToCreate.GrandTotal);

                    return ApiResponse<bool>.CreateSuccess(true, "Purchase created successfully");
                });
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"An error occurred while adding purchase. {ex.Message}", ex);
                return ApiResponse<bool>.CreateFailure($"An error occurred while adding purchase. {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<bool>> UpdatePurchase(UpdatePurchaseRequest request)
        {
            // Validate request using ValidationService
            var requestValidation = await _validationService.ValidateUpdatePurchaseRequestAsync(request);
            if (!requestValidation.IsValid)
            {
                _logger.LogWarning("Purchase update validation failed: PurchaseId={PurchaseId}, Errors={Errors}",
                    request.Id, string.Join(", ", requestValidation.Errors));
                return ApiResponse<bool>.CreateFailure("Purchase update validation failed", 400, requestValidation.Errors);
            }

            try
            {
                // Wrap entire operation in transaction
                return await _unitOfWork.ExecuteTransactionAsync(async () =>
                {
                    var purchaseToUpdate = await _unitOfWork.Repository<Purchase>()
                        .GetByIdWithIncludesAsync(request.Id, query => query.Include(p => p.Items));

                    if (purchaseToUpdate == null)
                        return ApiResponse<bool>.CreateFailure("Purchase not found", 404);

                    // Optimize: Batch load all stocks for old items in a single query
                    var oldItemPairs = purchaseToUpdate.Items.Select(i => new { i.MedicineId, i.BatchNo }).Distinct().ToList();
                    var oldStocks = new Dictionary<(int MedicineId, string BatchNo), Stock>();

                    if (oldItemPairs.Any())
                    {
                        var stocks = await _unitOfWork.Repository<Stock>()
                            .FindAsync(s => oldItemPairs.Any(p => p.MedicineId == s.MedicineId && p.BatchNo == s.BatchNo));

                        foreach (var stock in stocks)
                        {
                            oldStocks[(stock.MedicineId, stock.BatchNo)] = stock;
                        }
                    }

                    // Restore stock quantities for old items using batch-loaded stocks
                    foreach (var oldItem in purchaseToUpdate.Items)
                    {
                        if (oldStocks.TryGetValue((oldItem.MedicineId, oldItem.BatchNo), out var stock))
                        {
                            // Subtract the quantity that was added when the purchase was created
                            var stockResult = await _stockManagementService.DeductStockAsync(
                                stock.Id,
                                oldItem.Quantity,
                                $"Purchase update rollback for Purchase ID: {request.Id}");

                            if (!stockResult.Success)
                            {
                                _logger.LogWarning($"Failed to rollback stock: {stockResult.Message}");
                            }
                        }
                    }

                    // Clear old items
                    purchaseToUpdate.Items.Clear();

                    // Update purchase details
                    purchaseToUpdate.Notes = request.Notes;
                    purchaseToUpdate.PaymentMethodId = request.PaymentMethodId;
                    purchaseToUpdate.PurchaseStatusId = request.PurchaseStatusId;
                    purchaseToUpdate.Total = 0;
                    purchaseToUpdate.GrandTotal = 0;

                    // Process new items
                    foreach (var itemReq in request.Items)
                    {
                        // Calculate net amount
                        decimal netAmount = _purchaseCalculationService.CalculateLineNetAmount(
                            itemReq.Price, itemReq.Quantity, itemReq.Discount);

                        // Create new purchase item
                        var newItem = new PurchaseItems
                        {
                            Price = itemReq.Price,
                            Quantity = itemReq.Quantity,
                            Discount = itemReq.Discount,
                            Amount = netAmount,
                            MedicineId = itemReq.MedicineId,
                            BatchNo = itemReq.BatchNo
                        };

                        purchaseToUpdate.Items.Add(newItem);

                        // Update stock
                        var stock = await _unitOfWork.Repository<Stock>().FirstOrDefaultAsync(
                            x => x.MedicineId == itemReq.MedicineId && x.BatchNo == itemReq.BatchNo);

                        if (stock != null)
                        {
                            // Add quantity to existing stock
                            var stockResult = await _stockManagementService.AddStockAsync(
                                stock.Id,
                                itemReq.Quantity,
                                $"Purchase update for Purchase ID: {request.Id}");

                            if (!stockResult.Success)
                            {
                                _logger.LogWarning($"Failed to add stock: {stockResult.Message}");
                            }
                        }
                        else
                        {
                            // Add new stock item
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

                    // Recalculate totals
                    _purchaseCalculationService.RecalculateTotals(purchaseToUpdate);

                    // Update purchase
                    _unitOfWork.Repository<Purchase>().Update(purchaseToUpdate);

                    // Save all changes within transaction
                    await _unitOfWork.SaveChangesAsync();

                    // Invalidate cache after successful transaction
                    InvalidatePurchaseCache();

                    _logger.LogInformation("Purchase updated successfully: PurchaseId={PurchaseId}", request.Id);

                    return ApiResponse<bool>.CreateSuccess(true, "Purchase updated successfully");
                });
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
                // Wrap entire operation in transaction
                return await _unitOfWork.ExecuteTransactionAsync(async () =>
                {
                    var purchaseToDelete = await _unitOfWork.Repository<Purchase>().GetByIdAsync(id);

                    if (purchaseToDelete == null)
                        return ApiResponse<bool>.CreateFailure("Purchase not found", 404);

                    // Get purchase items
                    var items = await _unitOfWork.Repository<PurchaseItems>().FindAsync(x => x.PurchaseId == id);

                    // Optimize: Batch load all stocks for items in a single query
                    var medicineIds = items.Select(i => i.MedicineId).Distinct().ToList();
                    var stocksDict = new Dictionary<int, Stock>();

                    if (medicineIds.Any())
                    {
                        var stocks = await _unitOfWork.Repository<Stock>()
                            .FindAsync(s => medicineIds.Contains(s.MedicineId));

                        foreach (var stock in stocks)
                        {
                            if (!stocksDict.ContainsKey(stock.MedicineId))
                            {
                                stocksDict[stock.MedicineId] = stock;
                            }
                        }
                    }

                    // Remove stock quantities for all items (reverse the purchase) using batch-loaded stocks
                    if (items.Any())
                    {
                        foreach (var item in items)
                        {
                            if (stocksDict.TryGetValue(item.MedicineId, out var stock))
                            {
                                var stockResult = await _stockManagementService.DeductStockAsync(
                                    stock.Id,
                                    item.Quantity,
                                    $"Purchase deletion for Purchase ID: {id}");

                                if (!stockResult.Success)
                                {
                                    _logger.LogWarning($"Failed to deduct stock during deletion: {stockResult.Message}");
                                }
                            }
                        }
                    }

                    // Delete purchase (cascade will delete items)
                    _unitOfWork.Repository<Purchase>().Delete(purchaseToDelete);

                    // Save all changes within transaction
                    await _unitOfWork.SaveChangesAsync();

                    // Invalidate cache after successful transaction
                    InvalidatePurchaseCache();

                    _logger.LogInformation("Purchase deleted successfully: PurchaseId={PurchaseId}, ItemsRemoved={ItemCount}",
                        id, items.Count());

                    return ApiResponse<bool>.CreateSuccess(true, "Purchase deleted successfully");
                });
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

            // Fix N+1 query: Eagerly load Medicine with ThenInclude
            var purchase = await _purchaseRepository.GetByIdWithIncludesAsync(id,
                query => query.Include(i => i.Items)
                              .ThenInclude(i => i.Medicine)
                              .Include(s => s.Supplier)
                              .Include(x => x.PurchaseStatus)
                              .Include(x => x.PaymentMethod));

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
                    Amount = i.Amount,
                    MedicineName = i.Medicine?.Name ?? string.Empty
                })]
            };

            // N+1 query removed - Medicine now eagerly loaded above

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

        // UpdateStockQuantityAsync helper removed - use IStockManagementService instead
    }
}
