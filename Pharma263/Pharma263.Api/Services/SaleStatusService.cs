using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Pharma263.Api.Models.Shared;
using Pharma263.Api.Shared.Contracts;
using Pharma263.Domain.Common;
using Pharma263.Domain.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharma263.Api.Services
{
    public class SaleStatusService : IScopedInjectedService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<SaleStatusService> _logger;
        private const string SaleStatusesCacheKey = "sale_statuses_list";
        private const string SaleStatusDetailsCacheKeyPrefix = "sale_status_details_";
        private readonly TimeSpan CacheExpiry = TimeSpan.FromMinutes(30);

        public SaleStatusService(IUnitOfWork unitOfWork, IMemoryCache memoryCache, ILogger<SaleStatusService> logger)
        {
            _unitOfWork = unitOfWork;
            _memoryCache = memoryCache;
            _logger = logger;
        }

        public async Task<ApiResponse<List<TypeStatusMethodListResponse>>> GetSaleStatuses()
        {
            try
            {
                if (_memoryCache.TryGetValue(SaleStatusesCacheKey, out List<TypeStatusMethodListResponse> cached))
                {
                    return ApiResponse<List<TypeStatusMethodListResponse>>.CreateSuccess(cached);
                }

                var obj = await _unitOfWork.Repository<SaleStatus>().GetAllAsync();

                var result = obj.Select(x => new TypeStatusMethodListResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                }).ToList();

                _memoryCache.Set(SaleStatusesCacheKey, result, CacheExpiry);

                return ApiResponse<List<TypeStatusMethodListResponse>>.CreateSuccess(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving sale statuses");
                return ApiResponse<List<TypeStatusMethodListResponse>>.CreateFailure("An error occurred while retrieving sale statuses", 500);
            }
        }

        public async Task<ApiResponse<TypeStatusMethodDetailsResponse>> GetSaleStatus(int id)
        {
            try
            {
                var cacheKey = $"{SaleStatusDetailsCacheKeyPrefix}{id}";
                if (_memoryCache.TryGetValue(cacheKey, out TypeStatusMethodDetailsResponse cached))
                {
                    return ApiResponse<TypeStatusMethodDetailsResponse>.CreateSuccess(cached);
                }

                var obj = await _unitOfWork.Repository<SaleStatus>().GetByIdAsync(id);

                if (obj == null)
                    return ApiResponse<TypeStatusMethodDetailsResponse>.CreateFailure("Sale status not found", 404);

                var result = new TypeStatusMethodDetailsResponse
                {
                    Id = obj.Id,
                    Name = obj.Name,
                    Description = obj.Description
                };

                _memoryCache.Set(cacheKey, result, CacheExpiry);

                return ApiResponse<TypeStatusMethodDetailsResponse>.CreateSuccess(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving sale status with ID: {Id}", id);
                return ApiResponse<TypeStatusMethodDetailsResponse>.CreateFailure("An error occurred while retrieving sale status", 500);
            }
        }

        public async Task<ApiResponse<int>> AddSaleStatus(AddTypeStatusMethodModel request)
        {
            try
            {
                var objToCreate = new SaleStatus
                {
                    Name = request.Name,
                    Description = request.Description
                };

                await _unitOfWork.Repository<SaleStatus>().AddAsync(objToCreate);

                var result = await _unitOfWork.SaveChangesAsync();

                if (result > 0)
                {
                    InvalidateSaleStatusCache();
                    return ApiResponse<int>.CreateSuccess(objToCreate.Id, "Sale status added successfully");
                }
                else
                {
                    return ApiResponse<int>.CreateFailure("Failed to add sale status", 500);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding sale status: {Name}", request.Name);
                return ApiResponse<int>.CreateFailure("An error occurred while adding sale status", 500);
            }
        }

        public async Task<ApiResponse<bool>> UpdateSaleStatus(UpdateTypeStatusMethodModel request)
        {
            try
            {
                var existingSaleStatus = await _unitOfWork.Repository<SaleStatus>().GetByIdAsync(request.Id);

                if (existingSaleStatus == null)
                    return ApiResponse<bool>.CreateFailure("Sale status not found", 404);

                existingSaleStatus.Name = request.Name;
                existingSaleStatus.Description = request.Description;

                _unitOfWork.Repository<SaleStatus>().Update(existingSaleStatus);

                var result = await _unitOfWork.SaveChangesAsync();

                if (result > 0)
                {
                    InvalidateSaleStatusCache();
                    return ApiResponse<bool>.CreateSuccess(true, "Sale status updated successfully");
                }
                else
                {
                    return ApiResponse<bool>.CreateFailure("No changes were made to the sale status", 304);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating sale status with ID: {Id}", request.Id);
                return ApiResponse<bool>.CreateFailure("An error occurred while updating sale status", 500);
            }
        }

        public async Task<ApiResponse<bool>> DeleteSaleStatus(int id)
        {
            try
            {
                var objToDelete = await _unitOfWork.Repository<SaleStatus>().GetByIdAsync(id);

                if (objToDelete == null)
                    return ApiResponse<bool>.CreateFailure("Sale status not found", 404);

                _unitOfWork.Repository<SaleStatus>().Delete(objToDelete);

                var result = await _unitOfWork.SaveChangesAsync();

                if (result > 0)
                {
                    InvalidateSaleStatusCache();
                    return ApiResponse<bool>.CreateSuccess(true, "Sale status deleted successfully");
                }
                else
                {
                    return ApiResponse<bool>.CreateFailure("Failed to delete sale status", 500);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting sale status with ID: {Id}", id);
                return ApiResponse<bool>.CreateFailure("An error occurred while deleting sale status", 500);
            }
        }

        private void InvalidateSaleStatusCache()
        {
            _memoryCache.Remove(SaleStatusesCacheKey);
            
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
                        if (entry.Key.ToString().StartsWith(SaleStatusDetailsCacheKeyPrefix))
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