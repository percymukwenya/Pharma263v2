using Pharma263.Domain.Entities;
using Pharma263.Domain.Interfaces.Repository;
using Pharma263.Persistence.Contexts;
using Pharma263.Persistence.Shared;

namespace Pharma263.Persistence.Repositories
{
    public class ReturnRepository : Repository<Returns>, IReturnRepository
    {
        public ReturnRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
