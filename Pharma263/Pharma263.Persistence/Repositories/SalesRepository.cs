using Microsoft.EntityFrameworkCore;
using Pharma263.Domain.Entities;
using Pharma263.Persistence.Contexts;
using System.Threading.Tasks;
using Pharma263.Persistence.Shared;
using Pharma263.Domain.Interfaces.Repository;

namespace Pharma263.Persistence.Repositories
{
    public class SalesRepository : Repository<Sales>, ISalesRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public SalesRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> IsDuplicate(Sales sales)
        {
            bool isDuplicate = await _dbContext.Sales.AnyAsync(p =>
                p.SalesDate == sales.SalesDate &&
                p.Total == sales.Total &&
                p.Discount == sales.Discount &&
                p.GrandTotal == sales.GrandTotal &&
                p.PaymentMethodId == sales.PaymentMethodId &&
                p.SaleStatusId == sales.SaleStatusId &&
                p.CustomerId == sales.CustomerId);

            return isDuplicate;
        }
    }
}
