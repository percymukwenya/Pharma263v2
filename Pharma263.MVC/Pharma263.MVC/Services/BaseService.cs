using Pharma263.MVC.DTOs;
using Pharma263.MVC.Services.IService;
using Pharma263.MVC.Utility;
using System.Collections.Generic;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Pharma263.MVC.Services
{
    /// <summary>
    /// [DEPRECATED] Legacy HTTP client service using Newtonsoft.Json and manual token passing.
    ///
    /// MIGRATION NOTE: Please use IApiService (ApiService.cs) for new code instead:
    /// - Uses modern System.Text.Json
    /// - Automatic token injection via ITokenService
    /// - Better 401/403 handling with BaseController integration
    /// - Cleaner API with GetApiResponseAsync/PostApiResponseAsync methods
    ///
    /// MIGRATION STATUS: ALL services have been successfully migrated to IApiService as of Phase 3.
    /// - Phase 2: SaleStatusService, QuarantineService, ReturnDestinationService, ReturnReasonService, PurchaseStatusService, CustomerTypeService
    /// - Phase 3: ReportService, CalculationService
    ///
    /// This service is now UNUSED and scheduled for removal in the next major version.
    /// </summary>
    [Obsolete("Use IApiService instead. BaseService is UNUSED and will be removed in the next major version.", true)]
    public class BaseService : IBaseService
    {
        public ApiResponse responseModel { get; set; }
        public IHttpClientFactory _httpClient;
        public BaseService(IHttpClientFactory httpClient)
        {
            responseModel = new ApiResponse();
            _httpClient = httpClient;
        }

        public async Task<T> SendAsync<T>(ApiRequest apiRequest)
        {
            try
            {
                var client = _httpClient.CreateClient("CliqApi");
                HttpRequestMessage message = new HttpRequestMessage();
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(apiRequest.Url);

                if (apiRequest.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data),
                        Encoding.UTF8, "application/json");
                }

                switch (apiRequest.ApiType)
                {
                    case StaticDetails.ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;

                    case StaticDetails.ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;

                    case StaticDetails.ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;

                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }

                HttpResponseMessage apiResponse = null;

                if (!string.IsNullOrEmpty(apiRequest.Token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiRequest.Token);
                }

                apiResponse = await client.SendAsync(message);

                // Handle authentication failures explicitly
                if (apiResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized || 
                    apiResponse.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    var authFailureResponse = new ApiResponse
                    {
                        StatusCode = apiResponse.StatusCode,
                        IsSuccess = false,
                        ErrorMessages = new List<string> { "Authentication failed. Please login again." }
                    };
                    var res = JsonConvert.SerializeObject(authFailureResponse);
                    var returnObj = JsonConvert.DeserializeObject<T>(res);
                    return returnObj;
                }

                var apiContent = await apiResponse.Content.ReadAsStringAsync();
                try
                {
                    ApiResponse ApiResponse = JsonConvert.DeserializeObject<ApiResponse>(apiContent);
                    if (ApiResponse != null && (apiResponse.StatusCode == System.Net.HttpStatusCode.BadRequest
                        || apiResponse.StatusCode == System.Net.HttpStatusCode.NotFound))
                    {
                        ApiResponse.StatusCode = apiResponse.StatusCode;
                        ApiResponse.IsSuccess = false;
                        var res = JsonConvert.SerializeObject(ApiResponse);
                        var returnObj = JsonConvert.DeserializeObject<T>(res);
                        return returnObj;
                    }
                }
                catch (Exception e)
                {
                    var exceptionResponse = JsonConvert.DeserializeObject<T>(apiContent);
                    return exceptionResponse;
                }
                var APIResponse = JsonConvert.DeserializeObject<T>(apiContent);
                return APIResponse;
            }
            catch (Exception e)
            {
                var dto = new ApiResponse
                {
                    ErrorMessages = new List<string> { Convert.ToString(e.Message) },
                    IsSuccess = false
                };

                var res = JsonConvert.SerializeObject(dto);
                var APIResponse = JsonConvert.DeserializeObject<T>(res);

                return APIResponse;
            }
        }
    }
}