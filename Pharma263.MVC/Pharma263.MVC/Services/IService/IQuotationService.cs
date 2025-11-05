using Pharma263.Integration.Api.Common;
using Pharma263.MVC.DTOs.Quotation;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pharma263.MVC.Services.IService
{
    public interface IQuotationService
    {
        Task<ApiResponse<List<QuotationDto>>> GetQuotations();
        Task<ApiResponse<QuotationDetailsDto>> GetQuotation(int id);
        Task<ApiResponse<bool>> CreateQuotation(AddQuotationDto dto);
        Task<ApiResponse<bool>> UpdateQuotation(UpdateQuotationDto dto);
        Task<ApiResponse<bool>> DeleteQuotation(int id);
        Task<byte[]> GetQoutationDoc(int id);
    }
}
