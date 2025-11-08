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
        private readonly ICacheService _cacheService;

        // Cache key constants for medicines (Phase 2.2: Caching)
        private const string CACHE_KEY_ALL_MEDICINES = "medicines:all";
        private const string CACHE_KEY_MEDICINE_PREFIX = "medicines:id:";

        public MedicineService(IApiService apiService, ICacheService cacheService)
        {
            _apiService = apiService;
            _cacheService = cacheService;
        }

        public async Task<ApiResponse<MedicineDetailsResponse>> GetMedicine(int id)
        {
            // Phase 2.2: Cache individual medicine (high read, low write)
            var cacheKey = $"{CACHE_KEY_MEDICINE_PREFIX}{id}";

            var response = await _cacheService.GetOrCreateAsync(
                cacheKey,
                async () => await _apiService.GetApiResponseAsync<MedicineDetailsResponse>($"/api/Medicine/GetMedicine?id={id}"),
                absoluteExpirationMinutes: 30,
                slidingExpirationMinutes: 10
            );

            return response;
        }

        public async Task<ApiResponse<List<MedicineResponse>>> GetMedicines()
        {
            // Phase 2.2: Cache all medicines (high read, low write - reference data)
            var response = await _cacheService.GetOrCreateAsync(
                CACHE_KEY_ALL_MEDICINES,
                async () => await _apiService.GetApiResponseAsync<List<MedicineResponse>>("/api/Medicine/GetMedicines"),
                absoluteExpirationMinutes: 30,
                slidingExpirationMinutes: 10
            );

            return response;
        }

        public async Task<ApiResponse<bool>> CreateMedicine(CreateMedicineRequest request)
        {
            var response = await _apiService.PostApiResponseAsync<bool>("/api/Medicine/CreateMedicine", request);

            // Phase 2.2: Invalidate medicine cache on create
            if (response.Success)
            {
                _cacheService.RemoveByPattern("medicines:*");
            }

            return response;
        }

        public async Task<ApiResponse<bool>> UpdateMedicine(UpdateMedicineRequest request)
        {
            var response = await _apiService.PutApiResponseAsync<bool>("/api/Medicine/UpdateMedicine", request);

            // Phase 2.2: Invalidate medicine cache on update
            if (response.Success)
            {
                _cacheService.RemoveByPattern("medicines:*");
            }

            return response;
        }

        public async Task<ApiResponse<bool>> DeleteMedicine(int id, string token)
        {
            var response = await _apiService.DeleteApiResponseAsync($"/api/Medicine?id={id}");

            // Phase 2.2: Invalidate medicine cache on delete
            if (response.Success)
            {
                _cacheService.RemoveByPattern("medicines:*");
            }

            return response;
        }

        public async Task<List<GetMedicineListDto>> SearchMedicines(string query, int page)
        {
            var response = await _apiService.GetAllAsync<GetMedicineListDto>($"/api/Medicine/api/medicines/search?query={query}&page={page}");
            return response;
        }

        public async Task<ApiResponse<PaginatedList<MedicineResponse>>> GetMedicinesPaged(PagedRequest request)
        {
            var queryString = $"Page={request.Page}&PageSize={request.PageSize}&SearchTerm={System.Uri.EscapeDataString(request.SearchTerm ?? "")}&SortBy={System.Uri.EscapeDataString(request.SortBy ?? "")}&SortDescending={request.SortDescending.ToString().ToLowerInvariant()}";
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
