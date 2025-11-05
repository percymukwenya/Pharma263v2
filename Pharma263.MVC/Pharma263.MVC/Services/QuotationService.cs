using AutoMapper;
using Newtonsoft.Json;
using Pharma263.Integration.Api;
using Pharma263.Integration.Api.Common;
using Pharma263.Integration.Api.Models;
using Pharma263.Integration.Api.Models.Request;
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
        private readonly IMapper _mapper;

        public QuotationService(IApiService apiService, IPharmaApiService pharmaApiService, IMapper mapper)
        {
            _apiService = apiService;
            _pharmaApiService = pharmaApiService;
            _mapper = mapper;
        }

        public async Task<ApiResponse<List<QuotationDto>>> GetQuotations()
        {
            var response = await _apiService.GetApiResponseAsync<List<QuotationDto>>("/api/Quotation/GetQuotations");

            return response;
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
