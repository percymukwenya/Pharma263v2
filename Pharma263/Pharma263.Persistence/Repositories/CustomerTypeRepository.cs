using Pharma263.Domain.Entities;
using Pharma263.Domain.Interfaces.Repository;
using Pharma263.Persistence.Contexts;
using Pharma263.Persistence.Shared;

namespace Pharma263.Persistence.Repositories
{
    public class CustomerTypeRepository : Repository<CustomerType>, ICustomerTypeRepository
    {
        public CustomerTypeRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
