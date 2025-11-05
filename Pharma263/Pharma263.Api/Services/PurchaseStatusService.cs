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
    public class PurchaseStatusService : IScopedInjectedService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<PurchaseStatusService> _logger;
        private const string PurchaseStatusesCacheKey = "purchase_statuses_list";
        private const string PurchaseStatusDetailsCacheKeyPrefix = "purchase_status_details_";
        private readonly TimeSpan CacheExpiry = TimeSpan.FromMinutes(30);

        public PurchaseStatusService(IUnitOfWork unitOfWork, IMemoryCache memoryCache, ILogger<PurchaseStatusService> logger)
        {
            _unitOfWork = unitOfWork;
            _memoryCache = memoryCache;
            _logger = logger;
        }

        public async Task<ApiResponse<List<TypeStatusMethodListResponse>>> GetPurchaseStatuses()
        {
            try
            {
                if (_memoryCache.TryGetValue(PurchaseStatusesCacheKey, out List<TypeStatusMethodListResponse> cached))
                {
                    return ApiResponse<List<TypeStatusMethodListResponse>>.CreateSuccess(cached);
                }

                var obj = await _unitOfWork.Repository<PurchaseStatus>().GetAllAsync();

                var result = obj.Select(x => new TypeStatusMethodListResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                }).ToList();

                _memoryCache.Set(PurchaseStatusesCacheKey, result, CacheExpiry);

                return ApiResponse<List<TypeStatusMethodListResponse>>.CreateSuccess(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving purchase statuses");
                return ApiResponse<List<TypeStatusMethodListResponse>>.CreateFailure("An error occurred while retrieving purchase statuses", 500);
            }
        }

        public async Task<ApiResponse<TypeStatusMethodDetailsResponse>> GetPurchaseStatus(int id)
        {
            try
            {
                var cacheKey = $"{PurchaseStatusDetailsCacheKeyPrefix}{id}";
                if (_memoryCache.TryGetValue(cacheKey, out TypeStatusMethodDetailsResponse cached))
                {
                    return ApiResponse<TypeStatusMethodDetailsResponse>.CreateSuccess(cached);
                }

                var obj = await _unitOfWork.Repository<PurchaseStatus>().GetByIdAsync(id);

                if (obj == null)
                    return ApiResponse<TypeStatusMethodDetailsResponse>.CreateFailure("Purchase status not found", 404);

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
                _logger.LogError(ex, "An error occurred while retrieving purchase status with ID: {Id}", id);
                return ApiResponse<TypeStatusMethodDetailsResponse>.CreateFailure("An error occurred while retrieving purchase status", 500);
            }
        }

        public async Task<ApiResponse<int>> AddPurchaseStatus(AddTypeStatusMethodModel request)
        {
            try
            {
                var objToCreate = new PurchaseStatus
                {
                    Name = request.Name,
                    Description = request.Description
                };

                await _unitOfWork.Repository<PurchaseStatus>().AddAsync(objToCreate);

                var result = await _unitOfWork.SaveChangesAsync();

                if (result > 0)
                {
                    InvalidatePurchaseStatusCache();
                    return ApiResponse<int>.CreateSuccess(objToCreate.Id, "Purchase status added successfully");
                }
                else
                {
                    return ApiResponse<int>.CreateFailure("Failed to add purchase status", 500);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding purchase status: {Name}", request.Name);
                return ApiResponse<int>.CreateFailure("An error occurred while adding purchase status", 500);
            }
        }

        public async Task<ApiResponse<bool>> UpdatePurchaseStatus(UpdateTypeStatusMethodModel request)
        {
            try
            {
                var existingPurchaseStatus = await _unitOfWork.Repository<PurchaseStatus>().GetByIdAsync(request.Id);

                if (existingPurchaseStatus == null)
                    return ApiResponse<bool>.CreateFailure("Purchase status not found", 404);

                existingPurchaseStatus.Name = request.Name;
                existingPurchaseStatus.Description = request.Description;

                _unitOfWork.Repository<PurchaseStatus>().Update(existingPurchaseStatus);

                var result = await _unitOfWork.SaveChangesAsync();

                if (result > 0)
                {
                    InvalidatePurchaseStatusCache();
                    return ApiResponse<bool>.CreateSuccess(true, "Purchase status updated successfully");
                }
                else
                {
                    return ApiResponse<bool>.CreateFailure("No changes were made to the purchase status", 304);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating purchase status with ID: {Id}", request.Id);
                return ApiResponse<bool>.CreateFailure("An error occurred while updating purchase status", 500);
            }
        }

        public async Task<ApiResponse<bool>> DeletePurchaseStatus(int id)
        {
            try
            {
                var objToDelete = await _unitOfWork.Repository<PurchaseStatus>().GetByIdAsync(id);

                if (objToDelete == null)
                    return ApiResponse<bool>.CreateFailure("Purchase status not found", 404);

                _unitOfWork.Repository<PurchaseStatus>().Delete(objToDelete);

                var result = await _unitOfWork.SaveChangesAsync();

                if (result > 0)
                {
                    InvalidatePurchaseStatusCache();
                    return ApiResponse<bool>.CreateSuccess(true, "Purchase status deleted successfully");
                }
                else
                {
                    return ApiResponse<bool>.CreateFailure("Failed to delete purchase status", 500);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting purchase status with ID: {Id}", id);
                return ApiResponse<bool>.CreateFailure("An error occurred while deleting purchase status", 500);
            }
        }

        private void InvalidatePurchaseStatusCache()
        {
            _memoryCache.Remove(PurchaseStatusesCacheKey);
            
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
                        if (entry.Key.ToString().StartsWith(PurchaseStatusDetailsCacheKeyPrefix))
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
