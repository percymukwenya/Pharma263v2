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
    public class SupplierService : ISupplierService
    {
        private readonly IApiService _apiService;

        public SupplierService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<ApiResponse<List<SupplierResponse>>> GetAllAsync()
        {
            var response = await _apiService.GetApiResponseAsync<List<SupplierResponse>>("/api/Supplier/GetSuppliers");

            return response;
        }

        public async Task<ApiResponse<SupplierDetailsResponse>> GetAsync(int id)
        {
            var response = await _apiService.GetApiResponseAsync<SupplierDetailsResponse>($"/api/Supplier/GetSupplier?id={id}");

            return response;
        }

        public async Task<ApiResponse<bool>> CreateSupplier(CreateSupplierRequest request)
        {
            var response = await _apiService.PostApiResponseAsync<bool>("/api/Supplier/CreateSupplier", request);

            return response;
        }

        public async Task<ApiResponse<bool>> UpdateSupplier(UpdateSupplierRequest request)
        {
            var response = await _apiService.PutApiResponseAsync<bool>("/api/Supplier/UpdateSupplier", request);

            return response;
        }

        public async Task<ApiResponse<bool>> DeleteSupplier(int id)
        {
            var response = await _apiService.DeleteApiResponseAsync($"/api/Supplier?id={id}");

            return response;
        }

        public async Task<ApiResponse<PaginatedList<SupplierResponse>>> GetSuppliersPaged(PagedRequest request)
        {
            var queryString = $"Page={request.Page}&PageSize={request.PageSize}&SearchTerm={request.SearchTerm}&SortBy={request.SortBy}&SortDescending={request.SortDescending}";
            return await _apiService.GetApiResponseAsync<PaginatedList<SupplierResponse>>($"/api/Supplier/GetSuppliersPaged?{queryString}");
        }

        public async Task<DataTableResponse<SupplierResponse>> GetSuppliersForDataTable(DataTableRequest request)
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

                var apiResponse = await GetSuppliersPaged(pagedRequest);
                
                if (apiResponse.Success && apiResponse.Data != null)
                {
                    return new DataTableResponse<SupplierResponse>
                    {
                        Draw = request.Draw,
                        RecordsTotal = apiResponse.Data.TotalCount,
                        RecordsFiltered = apiResponse.Data.TotalCount,
                        Data = apiResponse.Data.Items
                    };
                }
                else
                {
                    return new DataTableResponse<SupplierResponse>
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
                return new DataTableResponse<SupplierResponse>
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
