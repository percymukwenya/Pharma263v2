using Pharma263.Domain.Common;
using Pharma263.Domain.Entities;

namespace Pharma263.Domain.Interfaces.Repository
{
    /// <summary>
    /// Repository for Stock entity - data access only
    /// For stock operations with business logic, use IStockManagementService
    /// </summary>
    public interface IStockRepository : IRepository<Stock>
    {
        // Business logic methods (AddQuantity/SubQuantity) moved to IStockManagementService
    }
}
