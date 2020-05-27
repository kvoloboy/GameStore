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
    public class PlatformAsyncRepository : IAsyncRepository<Platform>
    {
        private readonly ILogger _logger;
        private readonly AppDbContext _dbContext;

        public PlatformAsyncRepository(AppDbContext dbContext, ILogger logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task AddAsync(Platform entity)
        {
            _dbContext.Attach(entity);
            await _dbContext.Platforms.AddAsync(entity);

            var entry = new LogEntry<Platform>(Operation.Create, entity);
            _logger.Log(entry);
        }

        public async Task UpdateAsync(Platform entity)
        {
            var existingPlatform = await FindSingleAsync(p => p.Id == entity.Id);
            var oldValueInstance = existingPlatform.Clone();
            _dbContext.Entry(existingPlatform).CurrentValues.SetValues(entity);

            var entry = new LogEntry<Platform>(Operation.Update, oldValueInstance, entity);
            _logger.Log(entry);
        }

        public async Task DeleteAsync(string id)
        {
            var platform = await FindSingleAsync(p => p.Id == id);
            platform.IsDeleted = true;

            var entry = new LogEntry<Platform>(Operation.Delete, platform);
            _logger.Log(entry);
        }

        public Task<Platform> FindSingleAsync(Expression<Func<Platform, bool>> filter)
        {
            var platformEntity = _dbContext.Platforms
                .Include(p => p.GamePlatforms).ThenInclude(platform => platform.GameRoot)
                .FirstOrDefaultAsync(filter);

            return platformEntity;
        }

        public Task<List<Platform>> FindAllAsync(Expression<Func<Platform, bool>> filter = null)
        {
            var platforms = _dbContext.Platforms
                .Include(p => p.GamePlatforms)
                .ThenInclude(gp => gp.GameRoot)
                .AsNoTracking();

            if (filter != null)
            {
                platforms = platforms.Where(filter);
            }

            return platforms.ToListAsync();
        }

        public Task<bool> AnyAsync(Expression<Func<Platform, bool>> predicate)
        {
            var exists = _dbContext.Platforms.AnyAsync(predicate);

            return exists;
        }
    }
}