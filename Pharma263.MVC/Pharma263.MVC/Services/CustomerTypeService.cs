using Pharma263.Integration.Api.Common;
using Pharma263.MVC.Services.IService;
using Pharma263.MVC.Utility;
using System.Threading.Tasks;

namespace Pharma263.MVC.Services
{
    /// <summary>
    /// Service for fetching customer type lookup data.
    /// Migrated from BaseService to IApiService for better performance and consistency.
    /// </summary>
    public class CustomerTypeService : ICustomerTypeService
    {
        private readonly IApiService _apiService;

        public CustomerTypeService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<T> GetAllAsync<T>(string token)
        {
            // Note: Token parameter kept for interface compatibility but not used
            // IApiService automatically injects token via ITokenService
            var response = await _apiService.GetApiResponseAsync<T>("/api/Selection/GetCustomerTypes");
            return response.Data;
        }
    }
}
