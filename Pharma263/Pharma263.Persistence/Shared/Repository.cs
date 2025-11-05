using Microsoft.EntityFrameworkCore;
using Pharma263.Domain.Common;
using Pharma263.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Pharma263.Persistence.Shared
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<T> GetByIdWithIncludesAsync(int id, params Func<IQueryable<T>, IQueryable<T>>[] includes)
        {
            IQueryable<T> query = _dbSet;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => include(current));
            }

            return await query.FirstOrDefaultAsync(e => Microsoft.EntityFrameworkCore.EF.Property<int>(e, "Id") == id);
        }

        public async Task<IEnumerable<T>> GetAllAsync(Func<IQueryable<T>, IQueryable<T>> include = null)
        {
            IQueryable<T> query = _dbSet;
            if (include != null)
            {
                query = include(query);
            }
            return await query.ToListAsync();
        }

        public async Task<PaginatedList<T>> GetPaginatedAsync(int pageIndex, int pageSize, Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, IQueryable<T>> include = null)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (include != null)
            {
                query = include(query);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            var count = await query.CountAsync();
            var items = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IQueryable<T>> include = null)
        {
            IQueryable<T> query = _dbSet;
            if (include != null)
            {
                query = include(query);
            }
            return await query.Where(expression).ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            _dbSet.UpdateRange(entities);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> filter = null)
        {
            if (filter != null)
            {
                return await _dbSet.CountAsync(filter);
            }
            return await _dbSet.CountAsync();
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> filter = null)
        {
            if (filter != null)
            {
                return await _dbSet.AnyAsync(filter);
            }
            return await _dbSet.AnyAsync();
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IQueryable<T>> include = null)
        {
            IQueryable<T> query = _dbSet;
            if (include != null)
            {
                query = include(query);
            }
            return await query.FirstOrDefaultAsync(filter);
        }

        public async Task<TResult> MaxAsync<TResult>(Expression<Func<T, TResult>> selector, Expression<Func<T, bool>> filter = null)
        {
            if (filter != null)
            {
                return await _dbSet.Where(filter).MaxAsync(selector);
            }
            return await _dbSet.MaxAsync(selector);
        }

        public async Task<TResult> MinAsync<TResult>(Expression<Func<T, TResult>> selector, Expression<Func<T, bool>> filter = null)
        {
            if (filter != null)
            {
                return await _dbSet.Where(filter).MinAsync(selector);
            }
            return await _dbSet.MinAsync(selector);
        }

        public async Task<decimal> AverageAsync(Expression<Func<T, decimal>> selector, Expression<Func<T, bool>> filter = null)
        {
            if (filter != null)
            {
                return await _dbSet.Where(filter).AverageAsync(selector);
            }
            return await _dbSet.AverageAsync(selector);
        }

        public async Task<decimal> SumAsync(Expression<Func<T, decimal>> selector, Expression<Func<T, bool>> filter = null)
        {
            if (filter != null)
            {
                return await _dbSet.Where(filter).SumAsync(selector);
            }
            return await _dbSet.SumAsync(selector);
        }
    }
}
