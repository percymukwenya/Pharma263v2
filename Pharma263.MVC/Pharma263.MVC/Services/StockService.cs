using Microsoft.AspNetCore.Http;
using Pharma263.Integration.Api.Common;
using Pharma263.Integration.Api.Models.Request;
using Pharma263.Integration.Api.Models.Response;
using Pharma263.Integration.Api.Models.Common;
using Pharma263.MVC.Services.IService;
using Pharma263.MVC.Utility;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Pharma263.MVC.Services
{
    public class StockService : IStockService
    {
        private readonly IApiService _apiService;

        public StockService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<ApiResponse<bool>> AddStock(List<AddStockRequest> dto)
        {
            var response = await _apiService.PostApiResponseAsync<bool>("/api/Stock/AddStock", dto);

            return response;
        }

        public async Task<ApiResponse<bool>> ImportStockFromExcel(IFormFile file)
        {
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            stream.Position = 0;
            var streamPart = new Refit.StreamPart(stream, file.FileName, file.ContentType);

            var response = await _apiService.PostApiResponseAsync<bool>("/api/Stock/import", stream);

            return response;
        }

        public async Task<ApiResponse<StockDetailsResponse>> GetStock(int id)
        {
            var response = await _apiService.GetApiResponseAsync<StockDetailsResponse>($"/api/Stock/GetStockItem?id={id}");

            return response;
        }

        public async Task<ApiResponse<List<StockListResponse>>> GetStocks()
        {
            var response = await _apiService.GetApiResponseAsync<List<StockListResponse>>("/api/Stock/GetStockList");

            return response;
        }

        public async Task<ApiResponse<bool>> UpdateStock(UpdateStockRequest dto)
        {
            var response = await _apiService.PutApiResponseAsync<bool>("/api/Stock/UpdateStock", dto);

            return response;
        }

        public async Task<ApiResponse<PaginatedList<StockListResponse>>> GetStocksPaged(PagedRequest request)
        {
            var queryString = $"Page={request.Page}&PageSize={request.PageSize}&SearchTerm={System.Uri.EscapeDataString(request.SearchTerm ?? "")}&SortBy={System.Uri.EscapeDataString(request.SortBy ?? "")}&SortDescending={request.SortDescending.ToString().ToLowerInvariant()}";
            return await _apiService.GetApiResponseAsync<PaginatedList<StockListResponse>>($"/api/Stock/GetStocksPaged?{queryString}");
        }

        public async Task<DataTableResponse<StockListResponse>> GetStocksForDataTable(DataTableRequest request)
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

                var apiResponse = await GetStocksPaged(pagedRequest);
                
                if (apiResponse.Success && apiResponse.Data != null)
                {
                    return new DataTableResponse<StockListResponse>
                    {
                        Draw = request.Draw,
                        RecordsTotal = apiResponse.Data.TotalCount,
                        RecordsFiltered = apiResponse.Data.TotalCount,
                        Data = apiResponse.Data.Items
                    };
                }
                else
                {
                    return new DataTableResponse<StockListResponse>
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
                return new DataTableResponse<StockListResponse>
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
            
            return orderColumn.Column switch
            {
                0 => "medicinename",
                1 => "batchno", 
                2 => "expirydate",
                3 => "buyingprice",
                4 => "sellingprice",
                5 => "totalquantity",
                _ => null
            };
        }
    }
}
