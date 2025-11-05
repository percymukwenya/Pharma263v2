using Pharma263.Integration.Api.Common;
using Pharma263.Integration.Api.Models.Request;
using Pharma263.Integration.Api.Models.Response;
using Pharma263.Integration.Api.Models.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pharma263.MVC.Services.IService
{
    public interface ISupplierService
    {
        Task<ApiResponse<List<SupplierResponse>>> GetAllAsync();
        Task<ApiResponse<PaginatedList<SupplierResponse>>> GetSuppliersPaged(PagedRequest request);
        Task<DataTableResponse<SupplierResponse>> GetSuppliersForDataTable(DataTableRequest request);
        Task<ApiResponse<SupplierDetailsResponse>> GetAsync(int id);
        Task<ApiResponse<bool>> CreateSupplier(CreateSupplierRequest dto);
        Task<ApiResponse<bool>> UpdateSupplier(UpdateSupplierRequest dto);
        Task<ApiResponse<bool>> DeleteSupplier(int id);
    }
}
