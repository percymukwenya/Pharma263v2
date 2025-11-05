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
    public class PaymentMethodService : IScopedInjectedService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<PaymentMethodService> _logger;
        private const string PaymentMethodsCacheKey = "payment_methods_list";
        private const string PaymentMethodDetailsCacheKeyPrefix = "payment_method_details_";
        private readonly TimeSpan CacheExpiry = TimeSpan.FromMinutes(30);

        public PaymentMethodService(IUnitOfWork unitOfWork, IMemoryCache memoryCache, ILogger<PaymentMethodService> logger)
        {
            _unitOfWork = unitOfWork;
            _memoryCache = memoryCache;
            _logger = logger;
        }

        public async Task<ApiResponse<List<TypeStatusMethodListResponse>>> GetPaymentMethods()
        {
            try
            {
                if (_memoryCache.TryGetValue(PaymentMethodsCacheKey, out List<TypeStatusMethodListResponse> cached))
                {
                    return ApiResponse<List<TypeStatusMethodListResponse>>.CreateSuccess(cached);
                }

                var obj = await _unitOfWork.Repository<PaymentMethod>().GetAllAsync();

                var result = obj.Select(x => new TypeStatusMethodListResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                }).ToList();

                _memoryCache.Set(PaymentMethodsCacheKey, result, CacheExpiry);

                return ApiResponse<List<TypeStatusMethodListResponse>>.CreateSuccess(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving payment methods");
                return ApiResponse<List<TypeStatusMethodListResponse>>.CreateFailure("An error occurred while retrieving payment methods", 500);
            }
        }

        public async Task<ApiResponse<TypeStatusMethodDetailsResponse>> GetPaymentMethod(int id)
        {
            try
            {
                var cacheKey = $"{PaymentMethodDetailsCacheKeyPrefix}{id}";
                if (_memoryCache.TryGetValue(cacheKey, out TypeStatusMethodDetailsResponse cached))
                {
                    return ApiResponse<TypeStatusMethodDetailsResponse>.CreateSuccess(cached);
                }

                var obj = await _unitOfWork.Repository<PaymentMethod>().GetByIdAsync(id);

                if (obj == null)
                    return ApiResponse<TypeStatusMethodDetailsResponse>.CreateFailure("Payment method not found", 404);

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
                _logger.LogError(ex, "An error occurred while retrieving payment method with ID: {Id}", id);
                return ApiResponse<TypeStatusMethodDetailsResponse>.CreateFailure("An error occurred while retrieving payment method", 500);
            }
        }

        public async Task<ApiResponse<int>> AddPaymentMethod(AddTypeStatusMethodModel request)
        {
            try
            {
                var objToCreate = new PaymentMethod
                {
                    Name = request.Name,
                    Description = request.Description
                };

                await _unitOfWork.Repository<PaymentMethod>().AddAsync(objToCreate);

                var result = await _unitOfWork.SaveChangesAsync();

                if (result > 0)
                {
                    InvalidatePaymentMethodCache();
                    return ApiResponse<int>.CreateSuccess(objToCreate.Id, "Payment method added successfully");
                }
                else
                {
                    return ApiResponse<int>.CreateFailure("Failed to add payment method", 500);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding payment method: {Name}", request.Name);
                return ApiResponse<int>.CreateFailure("An error occurred while adding payment method", 500);
            }
        }

        public async Task<ApiResponse<bool>> UpdatePaymentMethod(UpdateTypeStatusMethodModel request)
        {
            try
            {
                var existingPaymentMethod = await _unitOfWork.Repository<PaymentMethod>().GetByIdAsync(request.Id);

                if (existingPaymentMethod == null)
                    return ApiResponse<bool>.CreateFailure("Payment method not found", 404);

                existingPaymentMethod.Name = request.Name;
                existingPaymentMethod.Description = request.Description;

                _unitOfWork.Repository<PaymentMethod>().Update(existingPaymentMethod);

                var result = await _unitOfWork.SaveChangesAsync();

                if (result > 0)
                {
                    InvalidatePaymentMethodCache();
                    return ApiResponse<bool>.CreateSuccess(true, "Payment method updated successfully");
                }
                else
                {
                    return ApiResponse<bool>.CreateFailure("No changes were made to the payment method", 304);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating payment method with ID: {Id}", request.Id);
                return ApiResponse<bool>.CreateFailure("An error occurred while updating payment method", 500);
            }
        }

        public async Task<ApiResponse<bool>> DeletePaymentMethod(int id)
        {
            try
            {
                var objToDelete = await _unitOfWork.Repository<PaymentMethod>().GetByIdAsync(id);

                if (objToDelete == null)
                    return ApiResponse<bool>.CreateFailure("Payment method not found", 404);

                _unitOfWork.Repository<PaymentMethod>().Delete(objToDelete);

                var result = await _unitOfWork.SaveChangesAsync();

                if (result > 0)
                {
                    InvalidatePaymentMethodCache();
                    return ApiResponse<bool>.CreateSuccess(true, "Payment method deleted successfully");
                }
                else
                {
                    return ApiResponse<bool>.CreateFailure("Failed to delete payment method", 500);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting payment method with ID: {Id}", id);
                return ApiResponse<bool>.CreateFailure("An error occurred while deleting payment method", 500);
            }
        }

        private void InvalidatePaymentMethodCache()
        {
            _memoryCache.Remove(PaymentMethodsCacheKey);
            
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
                        if (entry.Key.ToString().StartsWith(PaymentMethodDetailsCacheKeyPrefix))
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
