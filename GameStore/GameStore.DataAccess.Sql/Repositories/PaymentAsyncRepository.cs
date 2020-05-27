using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GameStore.Core.Abstractions;
using GameStore.Core.Models;
using GameStore.DataAccess.Sql.Context;
using GameStore.Infrastructure.Extensions;
using GameStore.Infrastructure.Logging.Interfaces;
using GameStore.Infrastructure.Logging.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.DataAccess.Sql.Repositories
{
    public class PaymentAsyncRepository : IAsyncRepository<PaymentType>
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger _logger;

        public PaymentAsyncRepository(AppDbContext dbContext, ILogger logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task AddAsync(PaymentType entity)
        {
            _dbContext.Attach(entity);
            await _dbContext.Payments.AddAsync(entity);

            var entry = new LogEntry<PaymentType>(Operation.Create, entity);
            _logger.Log(entry);
        }

        public async Task UpdateAsync(PaymentType entity)
        {
            var existingPayment = await FindSingleAsync(e => e.Id == entity.Id);
            var oldValueInstance = existingPayment.Clone();
            _dbContext.Entry(existingPayment).CurrentValues.SetValues(entity);

            var entry = new LogEntry<PaymentType>(Operation.Update, oldValueInstance, entity);
            _logger.Log(entry);
        }

        public async Task DeleteAsync(string id)
        {
            var existingPayment = await FindSingleAsync(e => e.Id == id);
            _dbContext.Payments.Remove(existingPayment);

            var entry = new LogEntry<PaymentType>(Operation.Delete, existingPayment);
            _logger.Log(entry);
        }

        public Task<PaymentType> FindSingleAsync(Expression<Func<PaymentType, bool>> filter)
        {
            var existingPayment = _dbContext.Payments
                .Include(p => p.ImagePath)
                .FirstOrDefaultAsync(filter);

            return existingPayment;
        }

        public Task<List<PaymentType>> FindAllAsync(Expression<Func<PaymentType, bool>> predicate = null)
        {
            var payments = _dbContext.Payments
                .AsNoTracking();

            if (predicate != null)
            {
                payments = payments.Where(predicate);
            }

            return payments.ToListAsync();
        }

        public Task<bool> AnyAsync(Expression<Func<PaymentType, bool>> predicate)
        {
            var any = _dbContext.Payments.AnyAsync(predicate);

            return any;
        }
    }
}