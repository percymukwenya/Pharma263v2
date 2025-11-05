using Microsoft.EntityFrameworkCore;
using Pharma263.Domain.Entities;
using Pharma263.Persistence.Contexts;
using System.Threading.Tasks;
using Pharma263.Persistence.Shared;
using Pharma263.Domain.Interfaces.Repository;

namespace Pharma263.Persistence.Repositories
{
    public class PurchaseRepository : Repository<Purchase>, IPurchaseRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public PurchaseRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> IsDuplicate(Purchase purchase)
        {
            bool isDuplicate = await _dbContext.Purchase.AnyAsync(p =>
                p.PurchaseDate == purchase.PurchaseDate &&
                p.Total == purchase.Total &&
                p.Discount == purchase.Discount &&
                p.GrandTotal == purchase.GrandTotal &&
                p.PaymentMethodId == purchase.PaymentMethodId &&
                p.PurchaseStatusId == purchase.PurchaseStatusId &&
                p.SupplierId == purchase.SupplierId);

            return isDuplicate;
        }
    }
}
