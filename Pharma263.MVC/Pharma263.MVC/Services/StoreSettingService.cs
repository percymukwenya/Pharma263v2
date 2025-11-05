using Pharma263.Integration.Api.Common;
using Pharma263.MVC.DTOs.StoreSettings;
using Pharma263.MVC.Services.IService;
using Pharma263.MVC.Utility;
using System.Threading.Tasks;

namespace Pharma263.MVC.Services
{
    public class StoreSettingService : IStoreSettingService
    {
        private readonly IApiService _apiService;

        public StoreSettingService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<StoreSettingsDetailsDto> GetAsync()
        {
            var response = await _apiService.GetApiResponseAsync<StoreSettingsDetailsDto>("/api/StoreSettings/GetStoreSetting");

            if (response != null && response.Success)
            {
                return response.Data;
            }

            return null;
        }

        public async Task<ApiResponse<bool>> UpdateAsync(UpdateStoreSettingsDto dto)
        {
            var response  = await _apiService.PutApiResponseAsync<bool>("/api/StoreSettings/UpdateStoreSetting", dto);

            return response;
        }
    }
}
