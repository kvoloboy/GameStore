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
    public class VisitAsyncRepository : IAsyncRepository<Visit>
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger _logger;

        public VisitAsyncRepository(AppDbContext dbContext, ILogger logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task AddAsync(Visit entity)
        {
            _dbContext.Attach(entity);
            await _dbContext.Visits.AddAsync(entity);

            var entry = new LogEntry<Visit>(Operation.Create, entity);
            _logger.Log(entry);
        }

        public async Task UpdateAsync(Visit entity)
        {
            var existingEntity = await FindSingleAsync(v => v.Id == entity.Id);
            var oldValueInstance = existingEntity.Clone();
            _dbContext.Entry(existingEntity).CurrentValues.SetValues(entity);

            var entry = new LogEntry<Visit>(Operation.Update, oldValueInstance, entity);
            _logger.Log(entry);
        }

        public async Task DeleteAsync(string id)
        {
            var existingEntity = await FindSingleAsync(v => v.Id == id);
            _dbContext.Visits.Remove(existingEntity);

            var entry = new LogEntry<Visit>(Operation.Delete, existingEntity);
            _logger.Log(entry);
        }

        public Task<Visit> FindSingleAsync(Expression<Func<Visit, bool>> filter)
        {
            var existingEntity = _dbContext.Visits
                .AsNoTracking()
                .Include(v => v.GameRoot)
                .FirstOrDefaultAsync(filter);

            return existingEntity;
        }

        public Task<List<Visit>> FindAllAsync(Expression<Func<Visit, bool>> predicate = null)
        {
            var visits = _dbContext.Visits
                .Include(v => v.GameRoot)
                .AsNoTracking();

            if (predicate != null)
            {
                visits = visits.Where(predicate);
            }

            return visits.ToListAsync();
        }

        public Task<bool> AnyAsync(Expression<Func<Visit, bool>> predicate)
        {
            var exists = _dbContext.Visits.AnyAsync(predicate);

            return exists;
        }
    }
}