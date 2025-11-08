using Pharma263.Integration.Api;
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

        public async Task<ApiResponse<PaginatedList<PurchasesResponse>>> GetPurchasesPaged(PagedRequest request)
        {
            var queryString = $"Page={request.Page}&PageSize={request.PageSize}&SearchTerm={System.Uri.EscapeDataString(request.SearchTerm ?? "")}&SortBy={System.Uri.EscapeDataString(request.SortBy ?? "")}&SortDescending={request.SortDescending.ToString().ToLowerInvariant()}";
            return await _apiService.GetApiResponseAsync<PaginatedList<PurchasesResponse>>($"/api/Purchase/GetPurchasesPaged?{queryString}");
        }

        public async Task<DataTableResponse<PurchasesResponse>> GetPurchasesForDataTable(DataTableRequest request)
        {
            try
            {
                var pagedRequest = new PagedRequest
                {
                    Page = (request.Start / request.Length) + 1,
                    PageSize = request.Length,
                    SearchTerm = request.Search?.Value,
                    SortDescending = request.Order?.FirstOrDefault()?.Dir == "desc",
                    SortBy = GetSortColumn(request)
                };

                var apiResponse = await GetPurchasesPaged(pagedRequest);

                if (apiResponse.Success && apiResponse.Data != null)
                {
                    return new DataTableResponse<PurchasesResponse>
                    {
                        Draw = request.Draw,
                        RecordsTotal = apiResponse.Data.TotalCount,
                        RecordsFiltered = apiResponse.Data.TotalCount,
                        Data = apiResponse.Data.Items
                    };
                }
                else
                {
                    return new DataTableResponse<PurchasesResponse>
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
                return new DataTableResponse<PurchasesResponse>
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

            // Map DataTable column index to API property names
            return orderColumn.Column switch
            {
                0 => "id",              // ID column
                1 => "supplier",        // Supplier column
                2 => "purchasedate",    // Purchase Date column
                3 => "paymentmethod",   // Payment Method column
                4 => "grandtotal",      // Grand Total column
                5 => "purchasestatus",  // Purchase Status column
                _ => null
            };
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
