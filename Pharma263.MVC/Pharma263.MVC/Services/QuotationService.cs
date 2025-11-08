using Pharma263.Integration.Api;
using Pharma263.Integration.Api.Common;
using Pharma263.Integration.Api.Models;
using Pharma263.Integration.Api.Models.Request;
using Pharma263.Integration.Api.Models.Common;
using Pharma263.Integration.Api.Models.Response;
using Pharma263.MVC.DTOs.Quotation;
using Pharma263.MVC.Services.IService;
using Pharma263.MVC.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharma263.MVC.Services
{
    public class QuotationService : IQuotationService
    {
        private readonly IApiService _apiService;
        private readonly IPharmaApiService _pharmaApiService;

        public QuotationService(IApiService apiService, IPharmaApiService pharmaApiService)
        {
            _apiService = apiService;
            _pharmaApiService = pharmaApiService;
        }

        public async Task<ApiResponse<List<QuotationDto>>> GetQuotations()
        {
            var response = await _apiService.GetApiResponseAsync<List<QuotationDto>>("/api/Quotation/GetQuotations");

            return response;
        }

        public async Task<ApiResponse<PaginatedList<QuotationListResponse>>> GetQuotationsPaged(PagedRequest request)
        {
            var queryString = $"Page={request.Page}&PageSize={request.PageSize}&SearchTerm={Uri.EscapeDataString(request.SearchTerm ?? "")}&SortBy={Uri.EscapeDataString(request.SortBy ?? "")}&SortDescending={request.SortDescending.ToString().ToLowerInvariant()}";
            return await _apiService.GetApiResponseAsync<PaginatedList<QuotationListResponse>>($"/api/Quotation/GetQuotationsPaged?{queryString}");
        }

        public async Task<DataTableResponse<QuotationListResponse>> GetQuotationsForDataTable(DataTableRequest request)
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

                var apiResponse = await GetQuotationsPaged(pagedRequest);

                if (apiResponse.Success && apiResponse.Data != null)
                {
                    return new DataTableResponse<QuotationListResponse>
                    {
                        Draw = request.Draw,
                        RecordsTotal = apiResponse.Data.TotalCount,
                        RecordsFiltered = apiResponse.Data.TotalCount,
                        Data = apiResponse.Data.Items
                    };
                }
                else
                {
                    return new DataTableResponse<QuotationListResponse>
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
                return new DataTableResponse<QuotationListResponse>
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
                0 => "id",                  // ID column
                1 => "customername",        // Customer column
                2 => "quotationdate",       // Quotation Date column
                3 => "total",               // Total column
                4 => "discount",            // Discount column
                5 => "grandtotal",          // Grand Total column
                6 => "quotationstatus",     // Quotation Status column
                _ => null
            };
        }

        public async Task<ApiResponse<QuotationDetailsDto>> GetQuotation(int id)
        {
            var response = await _apiService.GetApiResponseAsync<QuotationDetailsDto>($"/api/Quotation/GetQuotation?id={id}");

            return response;
        }

        public async Task<ApiResponse<bool>> CreateQuotation(AddQuotationDto dto)
        {
            var quotationDto = new CreateQuotationRequest
            {
                CustomerId = dto.CustomerId,
                QuotationDate = dto.QuotationDate,
                Total = dto.Total,
                QuoteStatus = dto.QuotationStatus,
                Notes = dto.Notes,
                Discount = dto.Discount,
                GrandTotal = dto.GrandTotal,
                Items = [.. dto.Items.Select(item => new QuotationItemModel
                {
                    StockId = item.StockId,
                    MedicineName = item.MedicineName,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    Amount = item.Amount,
                    Discount = item.Discount
                })]
            };

            var response = await _apiService.PostApiResponseAsync<bool>("/api/Quotation/CreateQuotation", quotationDto);
            
            return response;
        }

        public async Task<ApiResponse<bool>> UpdateQuotation(UpdateQuotationDto dto)
        {
            var quotationDto = new UpdateQuotationRequest
            {
                Id = dto.Id,
                CustomerId = dto.CustomerId,
                QuotationDate = dto.QuotationDate,
                Total = dto.Total,
                QuoteStatusId = dto.QuotationStatus,
                Notes = dto.Notes,
                Discount = dto.Discount,
                GrandTotal = dto.GrandTotal,
                Items = [.. dto.Items.Select(item => new QuotationItemModel
                {
                    StockId = item.StockId,
                    MedicineName = item.MedicineName,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    Amount = item.Amount,
                    Discount = item.Discount
                })]
            };

            var response = await _apiService.PutApiResponseAsync<bool>("/api/Quotation/UpdateQuotation", quotationDto);

            return response;
        }

        public async Task<ApiResponse<bool>> DeleteQuotation(int id)
        {
            var response = await _apiService.DeleteApiResponseAsync($"/api/Quotation?id={id}");

            return response;
        }

        public async Task<byte[]> GetQoutationDoc(int id)
        {
            return await _apiService.GetAsync<byte[]>($"/api/Quotation/GenerateQuotationDoc?quotationId={id}");
        }
    }
}
