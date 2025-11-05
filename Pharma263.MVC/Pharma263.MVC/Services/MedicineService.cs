using Pharma263.Integration.Api.Common;
using Pharma263.Integration.Api.Models.Request;
using Pharma263.Integration.Api.Models.Response;
using Pharma263.Integration.Api.Models.Common;
using Pharma263.MVC.DTOs.Medicine;
using Pharma263.MVC.Services.IService;
using Pharma263.MVC.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharma263.MVC.Services
{
    public class MedicineService : IMedicineService
    {
        private readonly IApiService _apiService;

        public MedicineService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<ApiResponse<MedicineDetailsResponse>> GetMedicine(int id)
        {
            var response = await _apiService.GetApiResponseAsync<MedicineDetailsResponse>($"/api/Medicine/GetMedicine?id={id}");

            return response;
        }

        public async Task<ApiResponse<List<MedicineResponse>>> GetMedicines()
        {
            var response = await _apiService.GetApiResponseAsync<List<MedicineResponse>>("/api/Medicine/GetMedicines");

            return response;
        }

        public async Task<ApiResponse<bool>> CreateMedicine(CreateMedicineRequest request)
        {
            var response = await _apiService.PostApiResponseAsync<bool>("/api/Medicine/CreateMedicine", request);

            return response;
        }

        public async Task<ApiResponse<bool>> UpdateMedicine(UpdateMedicineRequest request)
        {
            var response = await _apiService.PutApiResponseAsync<bool>("/api/Medicine/UpdateMedicine", request);

            return response;
        }

        public async Task<ApiResponse<bool>> DeleteMedicine(int id, string token)
        {
            var response = await _apiService.DeleteApiResponseAsync($"/api/Medicine?id={id}");

            return response;
        }

        public async Task<List<GetMedicineListDto>> SearchMedicines(string query, int page)
        {
            var response = await _apiService.GetAllAsync<GetMedicineListDto>($"/api/Medicine/api/medicines/search?query={query}&page={page}");
            return response;
        }

        public async Task<ApiResponse<PaginatedList<MedicineResponse>>> GetMedicinesPaged(PagedRequest request)
        {
            var queryString = $"Page={request.Page}&PageSize={request.PageSize}&SearchTerm={request.SearchTerm}&SortBy={request.SortBy}&SortDescending={request.SortDescending}";
            return await _apiService.GetApiResponseAsync<PaginatedList<MedicineResponse>>($"/api/Medicine/GetMedicinesPaged?{queryString}");
        }

        public async Task<DataTableResponse<MedicineResponse>> GetMedicinesForDataTable(DataTableRequest request)
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

                var apiResponse = await GetMedicinesPaged(pagedRequest);
                
                if (apiResponse.Success && apiResponse.Data != null)
                {
                    return new DataTableResponse<MedicineResponse>
                    {
                        Draw = request.Draw,
                        RecordsTotal = apiResponse.Data.TotalCount,
                        RecordsFiltered = apiResponse.Data.TotalCount,
                        Data = apiResponse.Data.Items
                    };
                }
                else
                {
                    return new DataTableResponse<MedicineResponse>
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
                return new DataTableResponse<MedicineResponse>
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
                0 => "name",
                1 => "genericname", 
                2 => "dosageform",
                3 => "quantityperunit",
                _ => null
            };
        }
    }
}
