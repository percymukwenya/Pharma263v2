using Microsoft.Extensions.Configuration;
using Pharma263.MVC.DTOs;
using Pharma263.MVC.Services.IService;
using Pharma263.MVC.Utility;
using System.Net.Http;
using System.Threading.Tasks;

namespace Pharma263.MVC.Services
{
    public class QuarantineService : BaseService, IQuarantineService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string pharmaUrl;
        public QuarantineService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            pharmaUrl = configuration.GetValue<string>("ServiceUrls:PharmaApi");
        }

        public Task<T> GetAllAsync<T>(string token)
        {
            return SendAsync<T>(new ApiRequest()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = pharmaUrl + "/api/QuarantineStock/GetQuarantineStockList",
                Token = token
            });
        }
    }
}
