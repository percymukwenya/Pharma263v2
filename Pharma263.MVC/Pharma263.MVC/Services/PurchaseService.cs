using Pharma263.Integration.Api;
using Pharma263.Integration.Api.Common;
using Pharma263.Integration.Api.Models.Request;
using Pharma263.Integration.Api.Models.Response;
using Pharma263.MVC.Services.IService;
using Pharma263.MVC.Utility;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pharma263.MVC.Services
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IPharmaApiService _pharmaApiService;
        private readonly IApiService _apiService;

        public PurchaseService(IPharmaApiService pharmaApiService, IApiService apiService)
        {
            _pharmaApiService = pharmaApiService;
            _apiService = apiService;
        }

        public async Task<ApiResponse<List<PurchasesResponse>>> GetAllPurchases()
        {
            var response = await _apiService.GetApiResponseAsync<List<PurchasesResponse>>("/api/Purchase/GetPurchases");

            return response;
        }

        public async Task<ApiResponse<PurchaseDetailsResponse>> GetPurchase(int id)
        {
            var response = await _apiService.GetApiResponseAsync<PurchaseDetailsResponse>($"/api/Purchase/GetPurchase?id={id}");

            return response;
        }

        public async Task<ApiResponse<bool>> CreatePurchase(CreatePurchaseRequest request)
        {
            var response = await _apiService.PostApiResponseAsync<bool>("/api/Purchase/CreatePurchase", request);

            return response;
        }

        public async Task<ApiResponse<bool>> UpdatePurchase(UpdatePurchaseRequest request)
        {
            var response = await _apiService.PutApiResponseAsync<bool>("/api/Purchase/UpdatePurchase", request);

            return response;
        }

        public async Task<ApiResponse<bool>> DeletePurchase(int id)
        {
            var response = await _apiService.DeleteApiResponseAsync($"/api/Purchase?id={id}");

            return response;
        }

        public async Task<byte[]> GetPurchaseInvoice(int id)
        {
            var response = await _apiService.GetAsync<byte[]>($"/api/Purchase/GetPurchaseInvoice?purchaseId={id}");

            return response;
        }
    }
}
