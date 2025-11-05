using Pharma263.Integration.Api.Common;
using Pharma263.MVC.DTOs.Returns;
using Pharma263.MVC.Services.IService;
using Pharma263.MVC.Utility;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pharma263.MVC.Services
{
    public class ReturnService : IReturnService
    {
        private readonly IApiService _apiService;

        public ReturnService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<ApiResponse<List<ReturnsDto>>> GetAllAsync()
        {
            return await _apiService.GetApiResponseAsync<List<ReturnsDto>>("/api/Return/GetReturns");
        }

        public async Task<ApiResponse<ReturnsDto>> GetAsync(int id)
        {
            return await _apiService.GetApiResponseAsync<ReturnsDto>($"/api/Return/GetReturn/{id}");
        }

        public async Task<ApiResponse<bool>> ProcessReturn(ProcessReturnRequestDto request)
        {
            return await _apiService.PostApiResponseAsync<bool>("/api/Return/CreateReturn", request);
        }
    }
}
