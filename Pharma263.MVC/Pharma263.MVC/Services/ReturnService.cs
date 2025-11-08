using Pharma263.Integration.Api.Common;
using Pharma263.Integration.Api.Models.Common;
using Pharma263.Integration.Api.Models.Request;
using Pharma263.MVC.DTOs.Returns;
using Pharma263.MVC.Services.IService;
using Pharma263.MVC.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharma263.MVC.Services
{
    public class ReturnService : IReturnService
    {
        private readonly IApiService _apiService;

        public ReturnService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<ApiResponse<List<ReturnsDto>>> GetAllAsync()
        {
            return await _apiService.GetApiResponseAsync<List<ReturnsDto>>("/api/Return/GetReturns");
        }

        public async Task<ApiResponse<PaginatedList<ReturnsDto>>> GetReturnsPaged(PagedRequest request)
        {
            var queryString = $"Page={request.Page}&PageSize={request.PageSize}&SearchTerm={request.SearchTerm}&SortBy={request.SortBy}&SortDescending={request.SortDescending}";
            return await _apiService.GetApiResponseAsync<PaginatedList<ReturnsDto>>($"/api/Return/GetReturnsPaged?{queryString}");
        }

        public async Task<DataTableResponse<ReturnsDto>> GetReturnsForDataTable(DataTableRequest request)
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

                var apiResponse = await GetReturnsPaged(pagedRequest);

                if (apiResponse.Success && apiResponse.Data != null)
                {
                    return new DataTableResponse<ReturnsDto>
                    {
                        Draw = request.Draw,
                        RecordsTotal = apiResponse.Data.TotalCount,
                        RecordsFiltered = apiResponse.Data.TotalCount,
                        Data = apiResponse.Data.Items
                    };
                }
                else
                {
                    return new DataTableResponse<ReturnsDto>
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
                return new DataTableResponse<ReturnsDto>
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
                0 => "id",                  // Return ID column
                1 => "saleid",              // Sale ID column
                2 => "medicinename",        // Medicine column
                3 => "quantity",            // Quantity column
                4 => "returnreason",        // Return Reason column
                5 => "returndestination",   // Destination column
                6 => "returnstatus",        // Status column
                7 => "datereturned",        // Date Returned column
                _ => null
            };
        }

        public async Task<ApiResponse<ReturnsDto>> GetAsync(int id)
        {
            return await _apiService.GetApiResponseAsync<ReturnsDto>($"/api/Return/GetReturn/{id}");
        }

        public async Task<ApiResponse<bool>> ProcessReturn(ProcessReturnRequestDto request)
        {
            return await _apiService.PostApiResponseAsync<bool>("/api/Return/CreateReturn", request);
        }
    }
}
