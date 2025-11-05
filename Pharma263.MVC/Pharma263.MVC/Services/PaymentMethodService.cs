using Microsoft.Extensions.Configuration;
using Pharma263.Integration.Api.Models.Response;
using Pharma263.MVC.Services.IService;
using Pharma263.MVC.Utility;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Pharma263.MVC.Services
{
    public class PaymentMethodService : IPaymentMethodService
    {
        private readonly IApiService _apiService;

        public PaymentMethodService(IApiService apiService, IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _apiService = apiService;
        }

        public async Task<List<SelectListResponse>> GetAllAsync()
        {
            return await _apiService.GetAsync<List<SelectListResponse>>("/api/Selection/GetPaymentMethods");
        }
    }
}
