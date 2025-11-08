using Pharma263.Integration.Api.Common;
using Pharma263.Integration.Api.Models.Request;
using Pharma263.Integration.Api.Models.Common;
using Pharma263.MVC.DTOs.Sales;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pharma263.MVC.Services.IService
{
    public interface ISaleService
    {
        Task<ApiResponse<List<SaleDto>>> GetSales();
        Task<ApiResponse<PaginatedList<SaleDto>>> GetSalesPaged(PagedRequest request);
        Task<DataTableResponse<SaleDto>> GetSalesForDataTable(DataTableRequest request);
        Task<ApiResponse<SaleDetailsDto>> GetSale(int id);
        Task<ApiResponse<bool>> CreateSale(AddSaleDto dto);
        Task<ApiResponse<bool>> UpdateSale(UpdateSaleDto dto);
        Task<ApiResponse<bool>> DeleteSale(int id);
        Task<byte[]> GetSaleInvoice(int id);
        Task<ApiResponse<bool>> CreateSaleFromQuotation(CreateSaleRequest request);
    }
}
