using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Pharma263.Api.Models.StoreSettings.Request;
using Pharma263.Api.Models.StoreSettings.Response;
using Pharma263.Api.Shared.Contracts;
using Pharma263.Domain.Common;
using Pharma263.Domain.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Pharma263.Api.Services
{
    public class StoreSettingsService : IScopedInjectedService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<StoreSettingsService> _logger;
        private const string StoreSettingsCacheKey = "store_settings";
        private readonly TimeSpan CacheExpiry = TimeSpan.FromHours(1); // Longer cache for settings

        public StoreSettingsService(IUnitOfWork unitOfWork, IMemoryCache memoryCache, ILogger<StoreSettingsService> logger)
        {
            _unitOfWork = unitOfWork;
            _memoryCache = memoryCache;
            _logger = logger;
        }

        public async Task<ApiResponse<StoreSettingDetailsResponse>> GetStoreSetting()
        {
            try
            {
                if (_memoryCache.TryGetValue(StoreSettingsCacheKey, out StoreSettingDetailsResponse cached))
                {
                    return ApiResponse<StoreSettingDetailsResponse>.CreateSuccess(cached);
                }

                var settings = await _unitOfWork.Repository<StoreSetting>().GetAllAsync();

                var settingsInfo = settings.FirstOrDefault();

                if (settingsInfo == null)
                    return ApiResponse<StoreSettingDetailsResponse>.CreateFailure("Store settings not found", 404);

                var mappedSettings = new StoreSettingDetailsResponse
                {
                    Id = settingsInfo.Id,
                    Logo = settingsInfo.Logo,
                    StoreName = settingsInfo.StoreName,
                    Email = settingsInfo.Email,
                    Phone = settingsInfo.Phone,
                    Currency = settingsInfo.Currency,
                    Address = settingsInfo.Address,
                    MCAZLicence = settingsInfo.MCAZLicence,
                    VATNumber = settingsInfo.VATNumber,
                    BankingDetails = settingsInfo.BankingDetails,
                    ReturnsPolicy = settingsInfo.ReturnsPolicy
                };

                _memoryCache.Set(StoreSettingsCacheKey, mappedSettings, CacheExpiry);

                return ApiResponse<StoreSettingDetailsResponse>.CreateSuccess(mappedSettings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving store settings");
                return ApiResponse<StoreSettingDetailsResponse>.CreateFailure("An error occurred while retrieving store settings", 500);
            }
        }

        public async Task<ApiResponse<bool>> UpdateStoreSettings(UpdateStoreSettingsRequest request)
        {
            try
            {
                var existingSetting = await _unitOfWork.Repository<StoreSetting>().GetByIdAsync(request.Id);

                if (existingSetting == null)
                {
                    return ApiResponse<bool>.CreateFailure("Store settings not found", 404);
                }

                existingSetting.StoreName = request.StoreName;
                existingSetting.Phone = request.Phone;
                existingSetting.Currency = request.Currency;
                existingSetting.Email = request.Email;
                existingSetting.Address = request.Address;
                existingSetting.MCAZLicence = request.MCAZLicence;
                existingSetting.VATNumber = request.VATNumber;
                existingSetting.BankingDetails = request.BankingDetails;
                existingSetting.ReturnsPolicy = request.ReturnsPolicy;

                if (!string.IsNullOrEmpty(request.Logo))
                {
                    existingSetting.Logo = request.Logo;
                }

                _unitOfWork.Repository<StoreSetting>().Update(existingSetting);

                var result = await _unitOfWork.SaveChangesAsync();

                if (result > 0)
                {
                    // Invalidate cache since settings were updated
                    _memoryCache.Remove(StoreSettingsCacheKey);
                    return ApiResponse<bool>.CreateSuccess(true, "Store settings updated successfully");
                }
                else
                {
                    return ApiResponse<bool>.CreateFailure("Failed to update store settings", 500);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating store settings with ID: {Id}", request.Id);
                return ApiResponse<bool>.CreateFailure("An error occurred while updating store settings", 500);
            }
        }
    }
}
