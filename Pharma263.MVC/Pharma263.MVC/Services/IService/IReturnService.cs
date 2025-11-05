using Pharma263.Integration.Api.Common;
using Pharma263.MVC.DTOs.Returns;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pharma263.MVC.Services.IService
{
    public interface IReturnService
    {
        Task<ApiResponse<List<ReturnsDto>>> GetAllAsync();
        Task<ApiResponse<ReturnsDto>> GetAsync(int id);
        Task<ApiResponse<bool>> ProcessReturn(ProcessReturnRequestDto request);
    }
}
