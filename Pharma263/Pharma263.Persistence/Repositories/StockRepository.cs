using Pharma263.Domain.Entities;
using Pharma263.Persistence.Contexts;
using Pharma263.Persistence.Shared;
using Pharma263.Domain.Interfaces.Repository;

namespace Pharma263.Persistence.Repositories
{
    /// <summary>
    /// Repository for Stock entity - data access only
    /// Business logic methods (AddQuantity/SubQuantity) moved to StockManagementService
    /// </summary>
    public class StockRepository : Repository<Stock>, IStockRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public StockRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        // Business logic methods removed - use IStockManagementService instead
    }
}
