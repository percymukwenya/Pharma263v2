using Microsoft.Extensions.Configuration;
using Pharma263.MVC.DTOs;
using Pharma263.MVC.Services.IService;
using Pharma263.MVC.Utility;
using System.Net.Http;
using System.Threading.Tasks;

namespace Pharma263.MVC.Services
{
    public class ReturnDestinationService : BaseService, IReturnDestinationService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string pharmaUrl;

        public ReturnDestinationService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            pharmaUrl = configuration.GetValue<string>("ServiceUrls:PharmaApi");
        }
        public Task<T> GetAllAsync<T>()
        {
            return SendAsync<T>(new ApiRequest()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = pharmaUrl + "/api/Selection/GetReturnDestinations"
            });
        }
    }
}
