using Microsoft.AspNetCore.Http;
using Pharma263.Integration.Api.Common;
using Pharma263.Integration.Api.Models.Request;
using Pharma263.Integration.Api.Models.Response;
using Pharma263.Integration.Api.Models.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pharma263.MVC.Services.IService
{
    public interface IStockService
    {
        Task<ApiResponse<StockDetailsResponse>> GetStock(int id);
        Task<ApiResponse<List<StockListResponse>>> GetStocks();
        Task<ApiResponse<PaginatedList<StockListResponse>>> GetStocksPaged(PagedRequest request);
        Task<DataTableResponse<StockListResponse>> GetStocksForDataTable(DataTableRequest request);
        Task<ApiResponse<bool>> AddStock(List<AddStockRequest> dto);
        Task<ApiResponse<bool>> UpdateStock(UpdateStockRequest dto);
        Task<ApiResponse<bool>> ImportStockFromExcel(IFormFile file);
    }
}
