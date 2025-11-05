using Pharma263.Integration.Api.Common;
using Pharma263.Integration.Api.Models.Request;
using Pharma263.Integration.Api.Models.Response;
using Pharma263.Integration.Api.Models.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pharma263.MVC.Services.IService
{
    public interface ICustomerService
    {
        Task<ApiResponse<List<CustomerResponse>>> GetCustomers();
        Task<ApiResponse<PaginatedList<CustomerResponse>>> GetCustomersPaged(PagedRequest request);
        Task<DataTableResponse<CustomerResponse>> GetCustomersForDataTable(DataTableRequest request);
        Task<ApiResponse<CustomerDetailsResponse>> GetCustomer(int id);
        Task<ApiResponse<bool>> CreateCustomer(CreateCustomerRequest dto);
        Task<ApiResponse<bool>> UpdateCustomer(UpdateCustomerRequest dto);
        Task<ApiResponse<bool>> DeleteCustomer(int id);
    }
}
