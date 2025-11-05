using Pharma263.Integration.Api.Common;
using Pharma263.Integration.Api.Models.Request;
using Pharma263.Integration.Api.Models.Response;
using Pharma263.Integration.Api.Models.Common;
using Pharma263.MVC.DTOs.Medicine;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pharma263.MVC.Services.IService
{
    public interface IMedicineService
    {
        Task<ApiResponse<List<MedicineResponse>>> GetMedicines();
        Task<ApiResponse<PaginatedList<MedicineResponse>>> GetMedicinesPaged(PagedRequest request);
        Task<DataTableResponse<MedicineResponse>> GetMedicinesForDataTable(DataTableRequest request);
        Task<ApiResponse<MedicineDetailsResponse>> GetMedicine(int id);
        Task<List<GetMedicineListDto>> SearchMedicines(string query, int page);
        Task<ApiResponse<bool>> CreateMedicine(CreateMedicineRequest dto);
        Task<ApiResponse<bool>> UpdateMedicine(UpdateMedicineRequest dto);
        Task<ApiResponse<bool>> DeleteMedicine(int id, string token);
    }
}
