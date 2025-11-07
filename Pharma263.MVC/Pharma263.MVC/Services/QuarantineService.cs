using Pharma263.Integration.Api.Common;
using Pharma263.MVC.Services.IService;
using Pharma263.MVC.Utility;
using System.Threading.Tasks;

namespace Pharma263.MVC.Services
{
    /// <summary>
    /// Service for fetching quarantine stock data.
    /// Migrated from BaseService to IApiService for better performance and consistency.
    /// </summary>
    public class QuarantineService : IQuarantineService
    {
        private readonly IApiService _apiService;

        public QuarantineService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<T> GetAllAsync<T>(string token)
        {
            // Note: Token parameter kept for interface compatibility but not used
            // IApiService automatically injects token via ITokenService
            var response = await _apiService.GetApiResponseAsync<T>("/api/QuarantineStock/GetQuarantineStockList");
            return response.Data;
        }
    }
}
