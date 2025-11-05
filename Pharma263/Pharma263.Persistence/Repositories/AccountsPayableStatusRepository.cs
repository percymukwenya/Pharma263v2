using Pharma263.Domain.Entities;
using Pharma263.Domain.Interfaces.Repository;
using Pharma263.Persistence.Contexts;
using Pharma263.Persistence.Shared;

namespace Pharma263.Persistence.Repositories
{
    public class AccountsPayableStatusRepository : Repository<AccountsPayableStatus>, IAccountsPayableStatusRepository
    {
        public AccountsPayableStatusRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
