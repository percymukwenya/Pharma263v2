using Pharma263.Integration.Api.Common;
using Pharma263.MVC.DTOs.PaymentReceived;
using Pharma263.MVC.Services.IService;
using Pharma263.MVC.Utility;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pharma263.MVC.Services
{
    public class PaymentReceivedService : IPaymentReceivedService
    {
        private readonly IApiService _apiService;
        public PaymentReceivedService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<ApiResponse<List<GetPaymentReceivedListDto>>> GetPaymentsReceived()
        {
            return await _apiService.GetApiResponseAsync<List<GetPaymentReceivedListDto>>("/api/PaymentReceived/GetPaymentsReceived");
        }

        public async Task<ApiResponse<bool>> AddPaymentReceived(AddPaymentReceivedRequest model)
        {
            return await _apiService.PostApiResponseAsync<bool>("/api/PaymentReceived/AddPaymentReceived", model);
        }

        public async Task<ApiResponse<GetPaymentReceivedDetailsDto>> GetPaymentReceived(int id)
        {
            return await _apiService.GetApiResponseAsync<GetPaymentReceivedDetailsDto>($"/api/PaymentReceived/GetPaymentReceived/{id}");
        }

        public async Task<ApiResponse<List<GetPaymentReceivedListDto>>> GetPaymentsReceivedFromCustomer(int customerId)
        {
            return await _apiService.GetApiResponseAsync<List<GetPaymentReceivedListDto>>($"/api/PaymentReceived/GetPaymentsReceivedFromCustomer/{customerId}");
        }

        public async Task<ApiResponse<List<GetPaymentSummaryByCustomerDto>>> GetPaymentSummaryByCustomer(DateTime startDate, DateTime endDate)
        {
            return await _apiService.GetApiResponseAsync<List<GetPaymentSummaryByCustomerDto>>($"/api/PaymentReceived/GetPaymentSummaryByCustomer?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");
        }

        public async Task<ApiResponse<bool>> UpdatePaymentReceived(UpdatePaymentReceivedDto model)
        {
            return await _apiService.PutApiResponseAsync<bool>("/api/PaymentReceived/UpdatePaymentReceived", model);
        }

        public async Task<ApiResponse<bool>> DeletePaymentReceived(int id)
        {
            return await _apiService.DeleteApiResponseAsync($"/api/PaymentReceived/DeletePaymentReceived/{id}");
        }
    }
}
