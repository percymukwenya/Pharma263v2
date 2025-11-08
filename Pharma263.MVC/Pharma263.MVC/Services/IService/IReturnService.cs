using Pharma263.Integration.Api.Common;
using Pharma263.Integration.Api.Models.Common;
using Pharma263.Integration.Api.Models.Request;
using Pharma263.MVC.DTOs.Returns;
using Pharma263.MVC.Utility;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pharma263.MVC.Services.IService
{
    public interface IReturnService
    {
        Task<ApiResponse<List<ReturnsDto>>> GetAllAsync();
        Task<ApiResponse<PaginatedList<ReturnsDto>>> GetReturnsPaged(PagedRequest request);
        Task<DataTableResponse<ReturnsDto>> GetReturnsForDataTable(DataTableRequest request);
        Task<ApiResponse<ReturnsDto>> GetAsync(int id);
        Task<ApiResponse<bool>> ProcessReturn(ProcessReturnRequestDto request);
    }
}
