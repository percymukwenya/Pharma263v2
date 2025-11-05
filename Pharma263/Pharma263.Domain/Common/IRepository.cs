using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace Pharma263.Domain.Common
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<T> GetByIdWithIncludesAsync(int id, params Func<IQueryable<T>, IQueryable<T>>[] includes);
        Task<IEnumerable<T>> GetAllAsync(Func<IQueryable<T>, IQueryable<T>> include = null);
        Task<PaginatedList<T>> GetPaginatedAsync(int pageIndex, int pageSize, Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IQueryable<T>> include = null);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IQueryable<T>> include = null);
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        void Update(T entity);
        void UpdateRange(IEnumerable<T> entities);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
        Task<int> CountAsync(Expression<Func<T, bool>> filter = null);
        Task<bool> AnyAsync(Expression<Func<T, bool>> filter = null);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IQueryable<T>> include = null);
        Task<TResult> MaxAsync<TResult>(Expression<Func<T, TResult>> selector, Expression<Func<T, bool>> filter = null);
        Task<TResult> MinAsync<TResult>(Expression<Func<T, TResult>> selector, Expression<Func<T, bool>> filter = null);
        Task<decimal> AverageAsync(Expression<Func<T, decimal>> selector, Expression<Func<T, bool>> filter = null);
        Task<decimal> SumAsync(Expression<Func<T, decimal>> selector, Expression<Func<T, bool>> filter = null);
    }
}
