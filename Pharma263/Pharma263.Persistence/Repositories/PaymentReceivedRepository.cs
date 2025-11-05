using Pharma263.Domain.Entities;
using Pharma263.Domain.Interfaces.Repository;
using Pharma263.Persistence.Contexts;
using Pharma263.Persistence.Shared;

namespace Pharma263.Persistence.Repositories
{
    public class PaymentReceivedRepository : Repository<PaymentReceived>, IPaymentReceivedRepository
    {
        public PaymentReceivedRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
