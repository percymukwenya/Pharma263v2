using Pharma263.Integration.Api.Common;
using Pharma263.MVC.Services.IService;
using Pharma263.MVC.Utility;
using System.Threading.Tasks;

namespace Pharma263.MVC.Services
{
    /// <summary>
    /// Service for fetching return reason lookup data.
    /// Migrated from BaseService to IApiService for better performance and consistency.
    /// </summary>
    public class ReturnReasonService : IReturnReasonService
    {
        private readonly IApiService _apiService;

        public ReturnReasonService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<T> GetAllAsync<T>()
        {
            var response = await _apiService.GetApiResponseAsync<T>("/api/Selection/GetReturnReasons");
            return response.Data;
        }
    }
}
