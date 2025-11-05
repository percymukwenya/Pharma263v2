using Pharma263.Integration.Api.Common;
using Pharma263.Integration.Api.Models.Response;
using Pharma263.MVC.Services.IService;
using Pharma263.MVC.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharma263.MVC.Services
{
    public class StatementService : IStatementService
    {
        private readonly IApiService _apiService;

        public StatementService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<ApiResponse<CustomerStatementResponse>> GetCustomerStatementAsync(int customerId, DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                var queryParams = new List<string>();
                if (startDate.HasValue)
                    queryParams.Add($"startDate={startDate.Value:yyyy-MM-dd}");
                if (endDate.HasValue)
                    queryParams.Add($"endDate={endDate.Value:yyyy-MM-dd}");

                var queryString = queryParams.Any() ? "?" + string.Join("&", queryParams) : "";
                var url = $"api/AccountsReceivable/GetCustomerStatement/{customerId}{queryString}";

                var response = await _apiService.GetApiResponseAsync<CustomerStatementResponse>(url);

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving customer statement: {ex.Message}", ex);
            }
            
        }

        public async Task<ApiResponse<SupplierStatementResponse>> GetSupplierStatementAsync(int supplierId, DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                var queryParams = new List<string>();
                if (startDate.HasValue)
                    queryParams.Add($"startDate={startDate.Value:yyyy-MM-dd}");
                if (endDate.HasValue)
                    queryParams.Add($"endDate={endDate.Value:yyyy-MM-dd}");

                var queryString = queryParams.Any() ? "?" + string.Join("&", queryParams) : "";
                var url = $"api/AccountsPayable/GetSupplierStatement/{supplierId}{queryString}";

                var response = await _apiService.GetApiResponseAsync<SupplierStatementResponse>(url);

                return response;
            }
            catch (Exception ex)
            {
                // Log exception
                throw new Exception($"Error retrieving supplier statement: {ex.Message}", ex);
            }
        }

        public async Task<byte[]> GetCustomerStatementPdfAsync(int customerId, DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                var queryParams = new List<string>();
                if (startDate.HasValue)
                    queryParams.Add($"startDate={startDate.Value:yyyy-MM-dd}");
                if (endDate.HasValue)
                    queryParams.Add($"endDate={endDate.Value:yyyy-MM-dd}");

                var queryString = queryParams.Any() ? "?" + string.Join("&", queryParams) : "";
                var url = $"api/AccountsReceivable/GetCustomerStatementPdf/{customerId}{queryString}";

                // Use your existing GetAsync method with byte[] type
                var pdfBytes = await _apiService.GetAsync<byte[]>(url);
                return pdfBytes;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving customer statement PDF: {ex.Message}", ex);
            }
        }

        public async Task<byte[]> GetSupplierStatementPdfAsync(int supplierId, DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                var queryParams = new List<string>();
                if (startDate.HasValue)
                    queryParams.Add($"startDate={startDate.Value:yyyy-MM-dd}");
                if (endDate.HasValue)
                    queryParams.Add($"endDate={endDate.Value:yyyy-MM-dd}");

                var queryString = queryParams.Any() ? "?" + string.Join("&", queryParams) : "";
                var url = $"api/AccountsPayable/GetSupplierStatementPdf/{supplierId}{queryString}";

                // Use your existing GetAsync method with byte[] type
                var pdfBytes = await _apiService.GetAsync<byte[]>(url);
                return pdfBytes;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving supplier statement PDF: {ex.Message}", ex);
            }
        }
    }
}
