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
    public class OrderAsyncRepository : IAsyncRepository<Order>
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger _logger;

        public OrderAsyncRepository(AppDbContext dbContext, ILogger logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task AddAsync(Order entity)
        {
            _dbContext.Attach(entity);
            await _dbContext.Orders.AddAsync(entity);

            var entry = new LogEntry<Order>(Operation.Create, entity);
            _logger.Log(entry);
        }

        public async Task UpdateAsync(Order entity)
        {
            var existingOrder = await _dbContext.Orders.FindAsync(entity.Id);
            var oldValueInstance = existingOrder.Clone();
            _dbContext.Entry(existingOrder).CurrentValues.SetValues(entity);

            foreach (var details in entity.Details)
            {
                details.Order = null;
                details.GameRoot = null;

                if (string.IsNullOrEmpty(details.Id))
                {
                    await _dbContext.OrderDetails.AddAsync(details);

                    continue;
                }

                _dbContext.OrderDetails.Update(details);
            }

            var entry = new LogEntry<Order>(Operation.Update, oldValueInstance, entity);
            _logger.Log(entry);
        }

        public async Task DeleteAsync(string id)
        {
            var existingOrder = await FindSingleAsync(o => o.Id == id);
            existingOrder.IsDeleted = true;
            var entry = new LogEntry<Order>(Operation.Delete, existingOrder);
            _logger.Log(entry);
        }

        public Task<Order> FindSingleAsync(Expression<Func<Order, bool>> filter)
        {
            var order = FindSingleAsync(filter, false);

            return order;
        }

        private Task<Order> FindSingleAsync(Expression<Func<Order, bool>> filter, bool isTracked)
        {
            var orders = isTracked ? _dbContext.Orders : _dbContext.Orders.AsNoTracking();
            var order = orders
                .Include(o => o.Details)
                .ThenInclude(od => od.GameRoot)
                .ThenInclude(gd => gd.Details)
                .Include(o => o.Details)
                .ThenInclude(od => od.GameRoot)
                .ThenInclude(gr => gr.Localizations)
                .Include(o => o.Details)
                .ThenInclude(od => od.GameRoot)
                .ThenInclude(gd => gd.GamePlatforms)
                .ThenInclude(platform => platform.Platform)
                .Include(o => o.Details)
                .ThenInclude(od => od.GameRoot)
                .ThenInclude(gr => gr.GameImages)
                .FirstOrDefaultAsync(filter);

            return order;
        }

        public Task<List<Order>> FindAllAsync(Expression<Func<Order, bool>> predicate = null)
        {
            var orders = _dbContext.Orders
                .Include(o => o.Details)
                .ThenInclude(od => od.GameRoot)
                .ThenInclude(gd => gd.Details)
                .Include(o => o.Details)
                .ThenInclude(od => od.GameRoot)
                .ThenInclude(gd => gd.Localizations)
                .AsNoTracking();

            if (predicate != null)
            {
                orders = orders.Where(predicate);
            }

            return orders.ToListAsync();
        }

        public Task<bool> AnyAsync(Expression<Func<Order, bool>> predicate)
        {
            var exists = _dbContext.Orders.AnyAsync(predicate);

            return exists;
        }
    }
}