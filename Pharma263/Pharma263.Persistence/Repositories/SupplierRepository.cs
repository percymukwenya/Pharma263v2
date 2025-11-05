using Dapper;
using Pharma263.Domain.Entities;
using Pharma263.Domain.Interfaces.Repository;
using Pharma263.Domain.Models.Dtos;
using Pharma263.Persistence.Contexts;
using Pharma263.Persistence.Shared;
using System.Collections.Generic;

namespace Pharma263.Persistence.Repositories
{
    public class SupplierRepository : Repository<Supplier>, ISupplierRepository
    {
        private readonly DapperContext _context;

        public SupplierRepository(ApplicationDbContext dbContext, DapperContext context) : base(dbContext)
        {
            _context = context;
        }

        public IEnumerable<SupplierDto> SearchSuppliers(string query, int page, int pageSize)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                query = "";
            }

            int skip = (page - 1) * pageSize;

            // Corrected SQL query - removed TOP and used OFFSET/FETCH
            const string sql = @"
                SELECT Id, Name, Email, Phone, Address, MCAZLicence
                FROM Pharma263.Supplier
                WHERE (Name LIKE @Query + '%' OR Name LIKE '% ' + @Query + '%' 
                       OR BusinessPartnerNumber LIKE @Query + '%')
                AND IsDeleted = 0
                ORDER BY 
                    CASE 
                        WHEN Name LIKE @Query + '%' THEN 0 
                        WHEN Name LIKE '% ' + @Query + '%' THEN 1
                        WHEN BusinessPartnerNumber LIKE @Query + '%' THEN 2
                        ELSE 3 
                    END, Name
                OFFSET @Skip ROWS
                FETCH NEXT @PageSize ROWS ONLY";

            using (var connection = _context.CreateConnection())
            {
                connection.Open();
                return connection.Query<SupplierDto>(sql, new
                {
                    Query = query,
                    Skip = skip,
                    PageSize = pageSize
                });
            }
        }
    }
}
