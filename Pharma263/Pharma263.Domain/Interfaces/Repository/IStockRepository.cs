using Pharma263.Domain.Common;
using Pharma263.Domain.Entities;
using System.Threading.Tasks;

namespace Pharma263.Domain.Interfaces.Repository
{
    public interface IStockRepository : IRepository<Stock>
    {
        Task AddQuantity(int quantity, int stockId);
        Task SubQuantity(int quantity, int stockId);
    }
}
