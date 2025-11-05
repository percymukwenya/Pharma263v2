using Pharma263.Domain.Common;
using Pharma263.Domain.Entities;
using System.Threading.Tasks;

namespace Pharma263.Domain.Interfaces.Repository
{
    public interface ISalesRepository : IRepository<Sales>
    {
        Task<bool> IsDuplicate(Sales sales);
    }
}
