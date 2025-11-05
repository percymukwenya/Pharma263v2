using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Pharma263.Api.Models.Quarantine;
using Pharma263.Api.Shared.Contracts;
using Pharma263.Application.Contracts.Logging;
using Pharma263.Domain.Common;
using Pharma263.Domain.Entities;
using System;
using System.Linq.Expressions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharma263.Api.Services
{
    public class QuarantineService : IScopedInjectedService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAppLogger<QuarantineService> _logger;
        private readonly IMemoryCache _memoryCache;
        private const string QuarantineStocksCacheKey = "quarantine_stocks_list";
        private const string QuarantineStockDetailsCacheKeyPrefix = "quarantine_stock_details_";
        private readonly TimeSpan CacheExpiry = TimeSpan.FromMinutes(5);

        public QuarantineService(IUnitOfWork unitOfWork, IAppLogger<QuarantineService> logger, IMemoryCache memoryCache)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _memoryCache = memoryCache;
        }

        public async Task<ApiResponse<List<QuarantineStockModel>>> GetQuarantineStocks()
        {
            try
            {
                if (_memoryCache.TryGetValue(QuarantineStocksCacheKey, out List<QuarantineStockModel> cached))
                {
                    return ApiResponse<List<QuarantineStockModel>>.CreateSuccess(cached);
                }

                var quarantineStocks = await _unitOfWork.Repository<Quarantine>().GetAllAsync(query => query.Include(m => m.Medicine));

                var quarantineDto = quarantineStocks.Select(x => new QuarantineStockModel
                {
                    Id = x.Id,
                    MedicineId = x.MedicineId,
                    MedicineName = x.Medicine.Name,
                    TotalQuantity = x.TotalQuantity,
                    DateAdded = x.CreatedDate,
                    ReturnReasonId = x.ReturnReasonId
                }).ToList();

                _memoryCache.Set(QuarantineStocksCacheKey, quarantineDto, CacheExpiry);

                return ApiResponse<List<QuarantineStockModel>>.CreateSuccess(quarantineDto);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"An error occurred while retrieving quarantine stocks. {ex.Message}", ex);

                return ApiResponse<List<QuarantineStockModel>>.CreateFailure($"An error occurred while retrieving quarantine stocks. {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<PaginatedList<QuarantineStockModel>>> GetQuarantineStocksPaged(PagedRequest request)
        {
            try
            {
                Expression<Func<Quarantine, bool>> filter = null;
                
                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    var searchTerm = request.SearchTerm.ToLower();
                    filter = x => x.Medicine.Name.ToLower().Contains(searchTerm) ||
                                  x.BatchNo.ToLower().Contains(searchTerm);
                }

                Func<IQueryable<Quarantine>, IOrderedQueryable<Quarantine>> orderBy = request.SortBy?.ToLower() switch
                {
                    "medicinename" => request.SortDescending ? (query => query.OrderByDescending(x => x.Medicine.Name)) : (query => query.OrderBy(x => x.Medicine.Name)),
                    "totalquantity" => request.SortDescending ? (query => query.OrderByDescending(x => x.TotalQuantity)) : (query => query.OrderBy(x => x.TotalQuantity)),
                    "dateadded" => request.SortDescending ? (query => query.OrderByDescending(x => x.CreatedDate)) : (query => query.OrderBy(x => x.CreatedDate)),
                    "batchno" => request.SortDescending ? (query => query.OrderByDescending(x => x.BatchNo)) : (query => query.OrderBy(x => x.BatchNo)),
                    _ => query => query.OrderByDescending(x => x.CreatedDate)
                };

                var paginatedQuarantine = await _unitOfWork.Repository<Quarantine>().GetPaginatedAsync(
                    request.Page,
                    request.PageSize,
                    filter,
                    orderBy,
                    query => query.Include(m => m.Medicine));
                
                var quarantineStocks = paginatedQuarantine.Items.ToList();

                var data = quarantineStocks.Select(x => new QuarantineStockModel
                {
                    Id = x.Id,
                    MedicineId = x.MedicineId,
                    MedicineName = x.Medicine.Name,
                    TotalQuantity = x.TotalQuantity,
                    DateAdded = x.CreatedDate,
                    ReturnReasonId = x.ReturnReasonId,
                    BatchNumber = x.BatchNo
                }).ToList();

                var paginatedResult = new PaginatedList<QuarantineStockModel>(data, paginatedQuarantine.TotalCount, paginatedQuarantine.PageIndex, request.PageSize);

                return ApiResponse<PaginatedList<QuarantineStockModel>>.CreateSuccess(paginatedResult);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"An error occurred while retrieving paged quarantine stocks. {ex.Message}", ex);

                return ApiResponse<PaginatedList<QuarantineStockModel>>.CreateFailure($"An error occurred while retrieving paged quarantine stocks. {ex.Message}", 500);
            }
        }

        public async Task<ApiResponse<QuarantineStockModel>> GetQuarantineStock(int id)
        {
            try
            {
                var cacheKey = $"{QuarantineStockDetailsCacheKeyPrefix}{id}";
                if (_memoryCache.TryGetValue(cacheKey, out QuarantineStockModel cached))
                {
                    return ApiResponse<QuarantineStockModel>.CreateSuccess(cached);
                }

                var stockItem = await _unitOfWork.Repository<Quarantine>().GetByIdWithIncludesAsync(id, query => query.Include(m => m.Medicine));

                if (stockItem == null)
                    return ApiResponse<QuarantineStockModel>.CreateFailure("Quarantine stock not found", 404);

                var itemDto = new QuarantineStockModel
                {
                    Id = stockItem.Id,
                    MedicineId = stockItem.MedicineId,
                    MedicineName = stockItem.Medicine.Name,
                    TotalQuantity = stockItem.TotalQuantity,
                    DateAdded = stockItem.CreatedDate,
                    ReturnReasonId = stockItem.ReturnReasonId,
                    BatchNumber = stockItem.BatchNo
                };

                _memoryCache.Set(cacheKey, itemDto, CacheExpiry);

                return ApiResponse<QuarantineStockModel>.CreateSuccess(itemDto);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"An error occurred while retrieving quarantine stock. {ex.Message}", ex);

                return ApiResponse<QuarantineStockModel>.CreateFailure($"An error occurred while retrieving quarantine stock. {ex.Message}", 500);
            }
        }
    }
}
