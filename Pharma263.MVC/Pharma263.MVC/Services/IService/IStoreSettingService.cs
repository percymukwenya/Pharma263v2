using Pharma263.Integration.Api.Common;
using Pharma263.MVC.DTOs.StoreSettings;
using System.Threading.Tasks;

namespace Pharma263.MVC.Services.IService
{
    public interface IStoreSettingService
    {
        Task<StoreSettingsDetailsDto> GetAsync();
        Task<ApiResponse<bool>> UpdateAsync(UpdateStoreSettingsDto dto);
    }
}
