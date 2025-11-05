using Pharma263.Integration.Api.Common;
using Pharma263.MVC.DTOs.PaymentMade;
using Pharma263.MVC.Services.IService;
using Pharma263.MVC.Utility;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pharma263.MVC.Services
{
    public class PaymentMadeService : IPaymentMadeService
    {
        private readonly IApiService _apiService;
        public PaymentMadeService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<ApiResponse<List<GetPaymentMadeListDto>>> GetPaymentsMade()
        {
            return await _apiService.GetApiResponseAsync<List<GetPaymentMadeListDto>>("/api/PaymentMade/GetPaymentsMade");
        }

        public async Task<ApiResponse<GetPaymentMadeDetailsDto>> GetPaymentMade(int id)
        {
            return await _apiService.GetApiResponseAsync<GetPaymentMadeDetailsDto>($"/api/PaymentMade/GetPaymentMade/{id}");
        }

        public async Task<ApiResponse<List<GetPaymentMadeListDto>>> GetPaymentsMadeToSupplier(int supplierId)
        {
            return await _apiService.GetApiResponseAsync<List<GetPaymentMadeListDto>>($"/api/PaymentMade/GetPaymentsMadeToSupplier/{supplierId}");
        }

        public async Task<ApiResponse<List<GetPaymentSummaryBySupplierDto>>> GetPaymentSummaryBySupplier(DateTime startDate, DateTime endDate)
        {
            return await _apiService.GetApiResponseAsync<List<GetPaymentSummaryBySupplierDto>>($"/api/PaymentMade/GetPaymentSummaryBySupplier?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");
        }

        public async Task<ApiResponse<bool>> AddPaymentMade(AddPaymentMadeRequest model)
        {
            return await _apiService.PostApiResponseAsync<bool>("/api/PaymentMade/AddPaymentMade", model);
        }

        public async Task<ApiResponse<bool>> UpdatePaymentMade(UpdatePaymentMadeDto model)
        {
            return await _apiService.PutApiResponseAsync<bool>("/api/PaymentMade/UpdatePaymentMade", model);
        }

        public async Task<ApiResponse<bool>> DeletePaymentMade(int id)
        {
            return await _apiService.DeleteApiResponseAsync($"/api/PaymentMade/DeletePaymentMade/{id}");
        }
    }
}
