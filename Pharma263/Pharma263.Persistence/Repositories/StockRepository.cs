using Pharma263.Domain.Entities;
using Pharma263.Persistence.Contexts;
using System.Threading.Tasks;
using Pharma263.Persistence.Shared;
using Pharma263.Domain.Interfaces.Repository;

namespace Pharma263.Persistence.Repositories
{
    public class StockRepository : Repository<Stock>, IStockRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public StockRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddQuantity(int quantity, int stockId)
        {
            var item = await GetByIdAsync(stockId);

            item.TotalQuantity += quantity;

            Update(item);
        }

        public async Task SubQuantity(int quantity, int stockId)
        {
            var item = await GetByIdAsync(stockId);

            item.TotalQuantity -= quantity;

            Update(item);
        }
    }
}
