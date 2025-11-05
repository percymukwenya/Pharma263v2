using Microsoft.EntityFrameworkCore.Storage;
using Pharma263.Domain.Common;
using Pharma263.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pharma263.Persistence.Shared
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private Dictionary<Type, object> _repositories;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            _repositories = new Dictionary<Type, object>();
        }

        public IRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            if (_repositories.ContainsKey(typeof(TEntity)))
            {
                return (IRepository<TEntity>)_repositories[typeof(TEntity)];
            }

            var repository = new Repository<TEntity>(_context);
            _repositories.Add(typeof(TEntity), repository);
            return repository;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync(IDbContextTransaction transaction)
        {
            await transaction.CommitAsync();
        }

        public async Task RollbackTransactionAsync(IDbContextTransaction transaction)
        {
            await transaction.RollbackAsync();
        }

        // Convenience method to execute an operation within a transaction with a return value
        public async Task<TResult> ExecuteTransactionAsync<TResult>(Func<Task<TResult>> operation)
        {
            using var transaction = await BeginTransactionAsync();
            try
            {
                var result = await operation();
                await CommitTransactionAsync(transaction);
                return result;
            }
            catch
            {
                await RollbackTransactionAsync(transaction);
                throw;
            }
        }

        // Convenience method to execute an operation within a transaction without a return value
        public async Task ExecuteTransactionAsync(Func<Task> operation)
        {
            using var transaction = await BeginTransactionAsync();
            try
            {
                await operation();
                await CommitTransactionAsync(transaction);
            }
            catch
            {
                await RollbackTransactionAsync(transaction);
                throw;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
