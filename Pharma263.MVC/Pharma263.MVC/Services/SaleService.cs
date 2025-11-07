using Pharma263.Integration.Api;
using Pharma263.Integration.Api.Common;
using Pharma263.Integration.Api.Models;
using Pharma263.Integration.Api.Models.Request;
using Pharma263.MVC.DTOs.Sales;
using Pharma263.MVC.Services.IService;
using Pharma263.MVC.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharma263.MVC.Services
{
    public class SaleService : ISaleService
    {
        private readonly IApiService _apiService;

        public SaleService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<ApiResponse<List<SaleDto>>> GetSales()
        {
            var response = await _apiService.GetApiResponseAsync<List<SaleDto>>("/api/Sale/GetSales");

            return response;
        }

        public async Task<ApiResponse<SaleDetailsDto>> GetSale(int id)
        {
            var response = await _apiService.GetApiResponseAsync<SaleDetailsDto>($"/api/Sale/GetSale?id={id}");

            return response;
        }

        public async Task<ApiResponse<bool>> CreateSale(AddSaleDto dto)
        {
            var saleRequest = new CreateSaleRequest
            {
                CustomerId = dto.CustomerId,
                SalesDate = dto.SalesDate,
                PaymentMethodId = dto.PaymentMethodId,
                Total = dto.Total,
                SaleStatusId = dto.SaleStatusId,
                Notes = dto.Notes,
                Discount = dto.Discount,
                GrandTotal = dto.GrandTotal,
                QuotationId = null,
                Items = [.. dto.Items.Select(item => new SaleItemModel
                {
                    StockId = item.StockId,
                    MedicineName = item.MedicineName,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    Amount = item.Amount,
                    Discount = item.Discount
                })]
            };

            var response = await _apiService.PostApiResponseAsync<bool>("/api/Sale/CreateSale", saleRequest);

            return response;
        }

        public async Task<ApiResponse<bool>> UpdateSale(UpdateSaleDto dto)
        {
            var saleRequest = new UpdateSaleRequest
            {
                Id = dto.Id,
                CustomerId = dto.CustomerId,
                SalesDate = dto.SalesDate,
                PaymentMethodId = dto.PaymentMethodId,
                Total = (decimal)dto.Total,
                SaleStatusId = dto.SaleStatusId,
                Notes = dto.Notes,
                Discount = (decimal)dto.Discount,
                GrandTotal = (decimal)dto.GrandTotal,
                Items = [.. dto.Items.Select(item => new SaleItemModel
                {
                    StockId = item.StockId,
                    MedicineName = item.MedicineName,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    Amount = item.Amount,
                    Discount = item.Discount
                })]
            };


            var response = await _apiService.PutApiResponseAsync<bool>("/api/Sale/UpdateSale", saleRequest);

            return response;
        }

        public async Task<byte[]> GetSaleInvoice(int id)
        {
            var response = await _apiService.GetAsync<byte[]>($"/api/Sale/GetSaleInvoice?saleId={id}");

            return response;
        }

        public async Task<ApiResponse<bool>> CreateSaleFromQuotation(CreateSaleRequest request)
        {
            var response = await _apiService.PostApiResponseAsync<bool>("/api/Sale/CreateSale", request);

            return response;
        }

        public async Task<ApiResponse<bool>> DeleteSale(int id)
        {
            var response = await _apiService.DeleteApiResponseAsync($"/api/Sale?id={id}");

            return response;
        }
    }
}
