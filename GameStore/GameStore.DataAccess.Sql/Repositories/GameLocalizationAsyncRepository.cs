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
    public class GameLocalizationAsyncRepository : IAsyncRepository<GameLocalization>
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger _logger;

        public GameLocalizationAsyncRepository(AppDbContext dbContext, ILogger logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public Task<GameLocalization> FindSingleAsync(Expression<Func<GameLocalization, bool>> predicate)
        {
            var localization = _dbContext.GameLocalizations.FirstOrDefaultAsync(predicate);

            return localization;
        }

        public Task<List<GameLocalization>> FindAllAsync(
            Expression<Func<GameLocalization, bool>> predicate = null)
        {
            var gameLocalizations = _dbContext.GameLocalizations.AsNoTracking();

            if (predicate != null)
            {
                gameLocalizations = gameLocalizations.Where(predicate);
            }

            return gameLocalizations.ToListAsync();
        }

        public Task<bool> AnyAsync(Expression<Func<GameLocalization, bool>> predicate)
        {
            var any = _dbContext.GameLocalizations.AnyAsync(predicate);

            return any;
        }

        public async Task AddAsync(GameLocalization entity)
        {
            _dbContext.GameLocalizations.Attach(entity);
            await _dbContext.GameLocalizations.AddAsync(entity);
            
            var entry = new LogEntry<GameLocalization>(Operation.Create, entity);
            _logger.Log(entry);
        }

        public async Task UpdateAsync(GameLocalization entity)
        {
            var existingLocalization = await FindSingleAsync(l => l.Id == entity.Id);
            var oldValueInstance = existingLocalization.Clone();
            _dbContext.Entry(existingLocalization).CurrentValues.SetValues(entity);
            
            var entry = new LogEntry<GameLocalization>(Operation.Update, oldValueInstance, existingLocalization);
            _logger.Log(entry);
        }

        public async Task DeleteAsync(string id)
        {
            var localization = await FindSingleAsync(gameLocalization => gameLocalization.Id == id);
            _dbContext.GameLocalizations.Remove(localization);
            
            var entry = new LogEntry<GameLocalization>(Operation.Delete, localization);
            _logger.Log(entry);
        }
    }
}