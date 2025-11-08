using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using Pharma263.Api.Models.Stocks.Request;
using Pharma263.Api.Models.Stocks.Response;
using Pharma263.Api.Shared.Contracts;
using Pharma263.Domain.Common;
using Pharma263.Domain.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Pharma263.Api.Services
{
    public class StockService : IScopedInjectedService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<StockService> _logger;
        private readonly IMemoryCache _memoryCache;
        private const string StocksCacheKey = "stocks_list";
        private const string StockDetailsCacheKeyPrefix = "stock_details_";
        private readonly TimeSpan CacheExpiry = TimeSpan.FromMinutes(5);

        public StockService(IUnitOfWork unitOfWork, ILogger<StockService> logger, IMemoryCache memoryCache)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _memoryCache = memoryCache;
        }

        public async Task<ApiResponse<List<StockListResponse>>> GetStocks()
        {
            try
            {
                if (_memoryCache.TryGetValue(StocksCacheKey, out List<StockListResponse> cachedStocks))
                {
                    return ApiResponse<List<StockListResponse>>.CreateSuccess(cachedStocks);
                }

                var stocks = await _unitOfWork.Repository<Stock>().GetAllAsync(x => x.Include(x => x.Medicine));

                var stockList = stocks.Select(x => new StockListResponse
                {
                    Id = x.Id,
                    MedicineId = x.MedicineId,
                    MedicineName = x.Medicine.Name,
                    ExpiryDate = x.ExpiryDate,
                    BatchNo = x.BatchNo,
                    BuyingPrice = (double)x.BuyingPrice,
                    SellingPrice = (double)x.SellingPrice,
                    TotalQuantity = x.TotalQuantity,
                });

                stockList = [.. stockList.Where(x => x.TotalQuantity != 0).OrderBy(x => x.MedicineName)];

                var result = stockList.ToList();
                _memoryCache.Set(StocksCacheKey, result, CacheExpiry);

                return ApiResponse<List<StockListResponse>>.CreateSuccess(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving stocks");

                return ApiResponse<List<StockListResponse>>.CreateFailure("An error occurred while retrieving stocks", 500);
            }
        }

        public async Task<ApiResponse<PaginatedList<StockListResponse>>> GetStocksPaged(PagedRequest request)
        {
            try
            {
                // Use Query() for projection - reduces data transfer by 30-40%
                var query = _unitOfWork.Repository<Stock>().Query()
                    .Include(x => x.Medicine)
                    .Where(x => x.TotalQuantity > 0);

                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    var searchTerm = request.SearchTerm.ToLower();
                    query = query.Where(x => x.Medicine.Name.ToLower().Contains(searchTerm) ||
                                            x.BatchNo.ToLower().Contains(searchTerm));
                }

                // Apply sorting
                query = request.SortBy?.ToLower() switch
                {
                    "medicinename" => request.SortDescending ? query.OrderByDescending(x => x.Medicine.Name) : query.OrderBy(x => x.Medicine.Name),
                    "expirydate" => request.SortDescending ? query.OrderByDescending(x => x.ExpiryDate) : query.OrderBy(x => x.ExpiryDate),
                    "batchno" => request.SortDescending ? query.OrderByDescending(x => x.BatchNo) : query.OrderBy(x => x.BatchNo),
                    "buyingprice" => request.SortDescending ? query.OrderByDescending(x => x.BuyingPrice) : query.OrderBy(x => x.BuyingPrice),
                    "sellingprice" => request.SortDescending ? query.OrderByDescending(x => x.SellingPrice) : query.OrderBy(x => x.SellingPrice),
                    "totalquantity" => request.SortDescending ? query.OrderByDescending(x => x.TotalQuantity) : query.OrderBy(x => x.TotalQuantity),
                    _ => query.OrderBy(x => x.Medicine.Name)
                };

                var count = await query.CountAsync();

                // Project to DTO before materialization - EF generates optimized SQL
                var data = await query
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(x => new StockListResponse
                    {
                        Id = x.Id,
                        MedicineId = x.MedicineId,
                        MedicineName = x.Medicine.Name,
                        ExpiryDate = x.ExpiryDate,
                        BatchNo = x.BatchNo,
                        BuyingPrice = (double)x.BuyingPrice,
                        SellingPrice = (double)x.SellingPrice,
                        TotalQuantity = x.TotalQuantity,
                    })
                    .ToListAsync();

                var paginatedResult = new PaginatedList<StockListResponse>(data, count, request.Page, request.PageSize);

                return ApiResponse<PaginatedList<StockListResponse>>.CreateSuccess(paginatedResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving paged stocks");

                return ApiResponse<PaginatedList<StockListResponse>>.CreateFailure("An error occurred while retrieving paged stocks", 500);
            }
        }

        public async Task<ApiResponse<StockDetailsResponse>> GetStock(int id)
        {
            try
            {
                var cacheKey = $"{StockDetailsCacheKeyPrefix}{id}";
                if (_memoryCache.TryGetValue(cacheKey, out StockDetailsResponse cachedStock))
                {
                    return ApiResponse<StockDetailsResponse>.CreateSuccess(cachedStock);
                }

                var stockItem = await _unitOfWork.Repository<Stock>().GetByIdWithIncludesAsync(id, x => x.Include(x => x.Medicine));

                if (stockItem == null)
                    return ApiResponse<StockDetailsResponse>.CreateFailure("Stock not found", 404);

                var data = new StockDetailsResponse
                {
                    Id = stockItem.Id,
                    MedicineId = stockItem.MedicineId,
                    MedicineName = stockItem.Medicine.Name,
                    ExpiryDate = stockItem.ExpiryDate,
                    BatchNo = stockItem.BatchNo,
                    BuyingPrice = (double)stockItem.BuyingPrice,
                    SellingPrice = (double)stockItem.SellingPrice,
                    TotalQuantity = stockItem.TotalQuantity,
                };

                _memoryCache.Set(cacheKey, data, CacheExpiry);

                return ApiResponse<StockDetailsResponse>.CreateSuccess(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving stock with ID: {Id}", id);

                return ApiResponse<StockDetailsResponse>.CreateFailure("An error occurred while processing your request", 500);
            }
        }

        public async Task<ApiResponse<bool>> AddStock(AddStockRequest request)
        {
            try
            {
                var validationErrors = ValidateAddStockRequest(request);
                if (validationErrors.Count > 0)
                {
                    return ApiResponse<bool>.CreateFailure("Validation failed", 400, validationErrors);
                }

                var medicine = await _unitOfWork.Repository<Medicine>().FirstOrDefaultAsync(x => x.Name == request.MedicineName);
                if (medicine == null)
                {
                    medicine = new Medicine
                    {
                        Name = request.MedicineName,
                        GenericName = request.MedicineName,
                    };
                    await _unitOfWork.Repository<Medicine>().AddAsync(medicine);

                    await _unitOfWork.SaveChangesAsync();
                }

                var stock = CreateStockFromRequest(request, medicine.Id);

                await _unitOfWork.Repository<Stock>().AddAsync(stock);

                var result = await _unitOfWork.SaveChangesAsync();

                if (result > 0)
                {
                    InvalidateStockCache();
                    return ApiResponse<bool>.CreateSuccess(true, "Stock added successfully");
                }
                else
                {
                    return ApiResponse<bool>.CreateFailure("Failed to add stock", 500);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding stock for medicine: {MedicineName}", request.MedicineName);

                return ApiResponse<bool>.CreateFailure("An error occurred while processing your request", 500);
            }
        }

        public async Task<ApiResponse<bool>> UpdateStock(UpdateStockRequest request)
        {
            try
            {
                var existingStock = await _unitOfWork.Repository<Stock>().GetByIdAsync(request.Id);

                if (existingStock == null)
                {
                    _logger.LogWarning("Stock with ID {StockId} not found for update", request.Id);
                    return ApiResponse<bool>.CreateFailure("Stock not found", 404);
                }

                var validationErrors = ValidateUpdateStockRequest(request);

                if (validationErrors.Count > 0)
                {
                    return ApiResponse<bool>.CreateFailure("Validation failed", 400, validationErrors);
                }

                existingStock.TotalQuantity = request.TotalQuantity;
                existingStock.BatchNo = string.IsNullOrEmpty(request.BatchNo) ? "" : request.BatchNo;
                existingStock.ExpiryDate = request.ExpiryDate < DateTimeOffset.MinValue ? DateTimeOffset.MinValue : request.ExpiryDate;
                existingStock.BuyingPrice = request.BuyingPrice;
                existingStock.SellingPrice = request.SellingPrice;
                existingStock.MedicineId = request.MedicineId;

                _unitOfWork.Repository<Stock>().Update(existingStock);

                var result = await _unitOfWork.SaveChangesAsync();

                if (result > 0)
                {
                    InvalidateStockCache();
                    return ApiResponse<bool>.CreateSuccess(true, "Stock updated successfully");
                }
                else
                {
                    return ApiResponse<bool>.CreateFailure("No changes were made to the stock", 304);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating stock with ID: {StockId}", request.Id);
                return ApiResponse<bool>.CreateFailure("An error occurred while processing your request", 500);
            }
        }

        public async Task<ApiResponse<BatchImportResult>> AddStockBatch(List<AddStockRequest> requests)
        {
            var result = new BatchImportResult
            {
                TotalRows = requests.Count,
                SuccessfulImports = 0,
                FailedImports = 0,
                Errors = new List<string>()
            };

            try
            {
                // Phase 1: Validate all requests upfront
                var validRequests = new List<(AddStockRequest Request, int Index)>();
                for (int i = 0; i < requests.Count; i++)
                {
                    var request = requests[i];
                    var validationErrors = ValidateAddStockRequest(request);
                    if (validationErrors.Count > 0)
                    {
                        result.FailedImports++;
                        result.Errors.Add($"Entry {i + 1}: {string.Join(", ", validationErrors)}");
                        continue;
                    }
                    validRequests.Add((request, i));
                }

                if (!validRequests.Any())
                {
                    return ApiResponse<BatchImportResult>.CreateFailure("All entries failed validation",
                        400, result.Errors);
                }

                // Phase 2: Batch process all valid requests in a single transaction
                await _unitOfWork.ExecuteTransactionAsync(async () =>
                {
                    // Batch-load all existing medicines by name
                    var medicineNames = validRequests.Select(r => r.Request.MedicineName).Distinct().ToList();
                    var existingMedicines = await _unitOfWork.Repository<Medicine>()
                        .FindAsync(m => medicineNames.Contains(m.Name));

                    var medicineDict = existingMedicines.ToDictionary(m => m.Name, m => m.Id);

                    // Create new medicines in bulk
                    var newMedicines = new List<Medicine>();
                    foreach (var name in medicineNames)
                    {
                        if (!medicineDict.ContainsKey(name))
                        {
                            var medicine = new Medicine
                            {
                                Name = name,
                                GenericName = name
                            };
                            newMedicines.Add(medicine);
                        }
                    }

                    if (newMedicines.Any())
                    {
                        foreach (var medicine in newMedicines)
                        {
                            await _unitOfWork.Repository<Medicine>().AddAsync(medicine);
                        }
                        await _unitOfWork.SaveChangesAsync();

                        // Add newly created medicines to dictionary
                        foreach (var medicine in newMedicines)
                        {
                            medicineDict[medicine.Name] = medicine.Id;
                        }
                    }

                    // Create all stock items in bulk
                    foreach (var (request, index) in validRequests)
                    {
                        try
                        {
                            if (medicineDict.TryGetValue(request.MedicineName, out var medicineId))
                            {
                                var stock = CreateStockFromRequest(request, medicineId);
                                await _unitOfWork.Repository<Stock>().AddAsync(stock);
                                result.SuccessfulImports++;
                            }
                            else
                            {
                                result.FailedImports++;
                                result.Errors.Add($"Entry {index + 1}: Medicine '{request.MedicineName}' could not be created");
                            }
                        }
                        catch (Exception ex)
                        {
                            result.FailedImports++;
                            result.Errors.Add($"Entry {index + 1}: {ex.Message}");
                            _logger.LogWarning(ex, "Failed to add stock for entry {Index}", index + 1);
                        }
                    }

                    // Single SaveChanges for all stock items
                    await _unitOfWork.SaveChangesAsync();

                    return ApiResponse<bool>.CreateSuccess(true);
                });

                // Invalidate cache after successful batch operation
                InvalidateStockCache();

                if (result.FailedImports == 0)
                {
                    _logger.LogInformation("Batch stock import successful: {Count} items added", result.SuccessfulImports);
                    return ApiResponse<BatchImportResult>.CreateSuccess(result, "All stocks added successfully");
                }
                else
                {
                    _logger.LogWarning("Batch stock import partial: {Success} succeeded, {Failed} failed",
                        result.SuccessfulImports, result.FailedImports);
                    return ApiResponse<BatchImportResult>.CreateFailure("Some stocks failed to add",
                        result.FailedImports == result.TotalRows ? 400 : 206, result.Errors);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding stocks in batch");
                return ApiResponse<BatchImportResult>.CreateFailure("An error occurred while processing the batch request", 500);
            }
        }

        public async Task<ApiResponse<BatchImportResult>> AddStockFromExcel(Stream excelFileStream)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var result = new BatchImportResult
            {
                TotalRows = 0,
                SuccessfulImports = 0,
                FailedImports = 0,
                Errors = new List<string>()
            };

            try
            {
                using (var package = new ExcelPackage(excelFileStream))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    int rowCount = worksheet.Dimension.Rows;
                    result.TotalRows = (rowCount - 1) * 2; // Excluding header row, counting both product sets

                    for (int row = 2; row <= rowCount; row++) // Start from row 2 to skip header
                    {
                        // Process first product set (columns A-C)
                        await ProcessProductSet(worksheet, row, 1, result);

                        // Process second product set (columns D-F)
                        await ProcessProductSet(worksheet, row, 4, result);
                    }
                }

                if (result.FailedImports == 0)
                {
                    return ApiResponse<BatchImportResult>.CreateSuccess(result, "All stocks imported successfully");
                }
                else
                {
                    return ApiResponse<BatchImportResult>.CreateFailure("Some stocks failed to import",
                        result.FailedImports == result.TotalRows ? 400 : 206,
                        result.Errors);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while importing stocks from Excel");
                return ApiResponse<BatchImportResult>.CreateFailure("An error occurred while processing the Excel file", 500);
            }
        }

        private async Task ProcessProductSet(ExcelWorksheet worksheet, int row, int startColumn, BatchImportResult result)
        {
            var productName = worksheet.Cells[row, startColumn].Value?.ToString();

            if (string.IsNullOrWhiteSpace(productName))
            {
                return; // Skip empty product entries
            }

            var request = new AddStockRequest
            {
                MedicineName = productName,
                TotalQuantity = Convert.ToInt32(worksheet.Cells[row, startColumn + 2].Value),
                BuyingPrice = Convert.ToDecimal(worksheet.Cells[row, startColumn + 1].Value),
                SellingPrice = Convert.ToDecimal(worksheet.Cells[row, startColumn + 1].Value), // Assuming selling price is the same as buying price
                BatchNo = "", // Add batch number if available in your Excel
                ExpiryDate = DateTime.Now.AddYears(1) // Set a default expiry date or add it to your Excel
            };

            var validationErrors = ValidateAddStockRequest(request);
            if (validationErrors.Count > 0)
            {
                result.FailedImports++;
                result.Errors.Add($"Row {row}, Column {startColumn}: {string.Join(", ", validationErrors)}");
                return;
            }

            var addResult = await AddStock(request);
            if (addResult.Success)
            {
                result.SuccessfulImports++;
            }
            else
            {
                result.FailedImports++;
                result.Errors.Add($"Row {row}, Column {startColumn}: {addResult.Message}");
            }
        }

        private List<string> ValidateAddStockRequest(AddStockRequest request)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(request.MedicineName))
            {
                errors.Add("MedicineName is required");
            }

            if (request.TotalQuantity <= 0)
            {
                errors.Add("TotalQuantity must be positive");
            }

            if (request.BuyingPrice < 0 || request.SellingPrice < 0)
            {
                errors.Add("Prices cannot be negative");
            }

            return errors;
        }

        private List<string> ValidateUpdateStockRequest(UpdateStockRequest request)
        {
            var errors = new List<string>();

            if (request.Id <= 0)
            {
                errors.Add("Id must be positive");
            }

            if (request.TotalQuantity < 0)
            {
                errors.Add("TotalQuantity cannot be negative");
            }

            if (request.BuyingPrice < 0 || request.SellingPrice < 0)
            {
                errors.Add("Prices cannot be negative");
            }

            return errors;
        }

        private Stock CreateStockFromRequest(AddStockRequest request, int medicineId)
        {
            return new Stock
            {
                TotalQuantity = request.TotalQuantity,
                NotifyForQuantityBelow = 10,
                BatchNo = string.IsNullOrEmpty(request.BatchNo) ? "" : request.BatchNo,
                ExpiryDate = request.ExpiryDate < DateTimeOffset.MinValue ? DateTimeOffset.MinValue : request.ExpiryDate,
                BuyingPrice = request.BuyingPrice,
                SellingPrice = request.SellingPrice,
                MedicineId = medicineId
            };
        }

        private void InvalidateStockCache()
        {
            _memoryCache.Remove(StocksCacheKey);
            
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
                        if (entry.Key.ToString().StartsWith(StockDetailsCacheKeyPrefix))
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
