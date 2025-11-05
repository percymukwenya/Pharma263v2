using Pharma263.Domain.Common;
using Pharma263.Domain.Entities;
using Pharma263.Domain.Models.Dtos;
using System.Collections.Generic;

namespace Pharma263.Domain.Interfaces.Repository
{
    public interface IMedicineRepository : IRepository<Medicine>
    {
        IEnumerable<MedicineDto> SearchMedicines(string query, int page, int pageSize);
    }
}
