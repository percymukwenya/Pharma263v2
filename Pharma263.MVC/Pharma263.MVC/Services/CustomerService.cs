using Pharma263.Integration.Api.Common;
using Pharma263.Integration.Api.Models.Request;
using Pharma263.Integration.Api.Models.Response;
using Pharma263.Integration.Api.Models.Common;
using Pharma263.MVC.Services.IService;
using Pharma263.MVC.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharma263.MVC.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IApiService _apiService;

        public CustomerService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<ApiResponse<CustomerDetailsResponse>> GetCustomer(int id)
        {
            var response = await _apiService.GetApiResponseAsync<CustomerDetailsResponse>($"/api/Customer/GetCustomer?id={id}");

            return response;
        }

        public async Task<ApiResponse<List<CustomerResponse>>> GetCustomers()
        {
            var response = await _apiService.GetApiResponseAsync<List<CustomerResponse>>("/api/Customer/GetCustomers");

            return response;
        }

        public async Task<ApiResponse<bool>> CreateCustomer(CreateCustomerRequest model)
        {
            return await _apiService.PostApiResponseAsync<bool>("/api/Customer/CreateCustomer", model);
        }

        public async Task<ApiResponse<bool>> UpdateCustomer(UpdateCustomerRequest dto)
        {
            return await _apiService.PutApiResponseAsync<bool>("/api/Customer/UpdateCustomer", dto);
        }

        public async Task<ApiResponse<bool>> DeleteCustomer(int id)
        {
            return await _apiService.DeleteApiResponseAsync($"/api/Customer?id={id}");
        }

        public async Task<ApiResponse<PaginatedList<CustomerResponse>>> GetCustomersPaged(PagedRequest request)
        {
            var queryString = $"Page={request.Page}&PageSize={request.PageSize}&SearchTerm={System.Uri.EscapeDataString(request.SearchTerm ?? "")}&SortBy={System.Uri.EscapeDataString(request.SortBy ?? "")}&SortDescending={request.SortDescending.ToString().ToLowerInvariant()}";
            return await _apiService.GetApiResponseAsync<PaginatedList<CustomerResponse>>($"/api/Customer/GetCustomersPaged?{queryString}");
        }

        public async Task<DataTableResponse<CustomerResponse>> GetCustomersForDataTable(DataTableRequest request)
        {
            try
            {
                // Convert DataTable request to PagedRequest
                var pagedRequest = new PagedRequest
                {
                    Page = (request.Start / request.Length) + 1,
                    PageSize = request.Length,
                    SearchTerm = request.Search?.Value,
                    SortDescending = request.Order?.FirstOrDefault()?.Dir == "desc",
                    SortBy = GetSortColumn(request)
                };

                var apiResponse = await GetCustomersPaged(pagedRequest);
                
                if (apiResponse.Success && apiResponse.Data != null)
                {
                    return new DataTableResponse<CustomerResponse>
                    {
                        Draw = request.Draw,
                        RecordsTotal = apiResponse.Data.TotalCount,
                        RecordsFiltered = apiResponse.Data.TotalCount,
                        Data = apiResponse.Data.Items
                    };
                }
                else
                {
                    return new DataTableResponse<CustomerResponse>
                    {
                        Draw = request.Draw,
                        RecordsTotal = 0,
                        RecordsFiltered = 0,
                        Error = apiResponse.Message
                    };
                }
            }
            catch (System.Exception ex)
            {
                return new DataTableResponse<CustomerResponse>
                {
                    Draw = request.Draw,
                    RecordsTotal = 0,
                    RecordsFiltered = 0,
                    Error = ex.Message
                };
            }
        }

        private string GetSortColumn(DataTableRequest request)
        {
            if (request.Order == null || !request.Order.Any()) 
                return null;

            var orderColumn = request.Order.First();
            
            // Map DataTable column indices to API sort field names
            return orderColumn.Column switch
            {
                0 => "name",
                1 => "email", 
                2 => "phone",
                _ => null
            };
        }
    }
}
