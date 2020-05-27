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
    public class OrderDetailsAsyncRepository : IAsyncRepository<OrderDetails>
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger _logger;

        public OrderDetailsAsyncRepository(AppDbContext dbContext, ILogger logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task AddAsync(OrderDetails entity)
        {
            _dbContext.Attach(entity);
            await _dbContext.OrderDetails.AddAsync(entity);

            var entry = new LogEntry<OrderDetails>(Operation.Create, entity);
            _logger.Log(entry);
        }

        public async Task UpdateAsync(OrderDetails entity)
        {
            var existingDetails = await FindSingleAsync(od => od.Id == entity.Id, true);
            var oldValueInstance = existingDetails.Clone();
            _dbContext.Entry(existingDetails).CurrentValues.SetValues(entity);

            var entry = new LogEntry<OrderDetails>(Operation.Update, oldValueInstance, entity);
            _logger.Log(entry);
        }

        public async Task DeleteAsync(string id)
        {
            var existingDetails = await FindSingleAsync(od => od.Id == id, true);
            _dbContext.OrderDetails.Remove(existingDetails);

            var entry = new LogEntry<OrderDetails>(Operation.Delete, existingDetails);
            _logger.Log(entry);
        }

        public Task<OrderDetails> FindSingleAsync(Expression<Func<OrderDetails, bool>> filter)
        {
            var existingOrderDetails = FindSingleAsync(filter, false);

            return existingOrderDetails;
        }

        private Task<OrderDetails> FindSingleAsync(Expression<Func<OrderDetails, bool>> filter, bool isTracked)
        {
            var details = isTracked ? _dbContext.OrderDetails : _dbContext.OrderDetails.AsNoTracking();
            var existingOrderDetails = details
                .Include(od => od.GameRoot).ThenInclude(gr => gr.Details)
                .Include(od => od.Order)
                .FirstOrDefaultAsync(filter);

            return existingOrderDetails;
        }

        public Task<List<OrderDetails>> FindAllAsync(Expression<Func<OrderDetails, bool>> predicate = null)
        {
            var existingOrderDetails = _dbContext.OrderDetails
                .Include(od => od.GameRoot).ThenInclude(gr => gr.Details)
                .Include(od => od.Order)
                .AsNoTracking();

            if (predicate != null)
            {
                existingOrderDetails = existingOrderDetails.Where(predicate);
            }

            return existingOrderDetails.ToListAsync();
        }

        public Task<bool> AnyAsync(Expression<Func<OrderDetails, bool>> predicate)
        {
            var exists = _dbContext.OrderDetails.AnyAsync(predicate);

            return exists;
        }
    }
}