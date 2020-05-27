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
    public class GameDetailsAsyncRepository : IAsyncRepository<GameDetails>
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger _logger;

        public GameDetailsAsyncRepository(AppDbContext dbContext, ILogger logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task AddAsync(GameDetails entity)
        {
            _dbContext.Attach(entity);
            await _dbContext.GameDetails.AddAsync(entity);

            var entry = new LogEntry<GameDetails>(Operation.Create, entity);
            _logger.Log(entry);
        }

        public async Task UpdateAsync(GameDetails entity)
        {
            var existingDetails = await FindSingleAsync(g => g.Id == entity.Id, true);
            var oldValueInstance = existingDetails.Clone();
            _dbContext.Entry(existingDetails).CurrentValues.SetValues(entity);

            var entry = new LogEntry<GameDetails>(Operation.Update, oldValueInstance, entity);
            _logger.Log(entry);
        }

        public async Task DeleteAsync(string id)
        {
            var game = await FindSingleAsync(g => g.Id == id, true);
            _dbContext.GameDetails.Remove(game);

            var entry = new LogEntry<GameDetails>(Operation.Delete, game);
            _logger.Log(entry);
        }

        public Task<GameDetails> FindSingleAsync(Expression<Func<GameDetails, bool>> filter)
        {
            var gameDetails = FindSingleAsync(filter, false);

            return gameDetails;
        }

        private Task<GameDetails> FindSingleAsync(Expression<Func<GameDetails, bool>> filter, bool isTracked)
        {
            var details = isTracked ? _dbContext.GameDetails : _dbContext.GameDetails.AsNoTracking();
            var gameEntity = details
                .Include(g => g.GameRoot)
                .FirstOrDefaultAsync(filter);

            return gameEntity;
        }

        public Task<List<GameDetails>> FindAllAsync(Expression<Func<GameDetails, bool>> filter = null)
        {
            var games = _dbContext.GameDetails
                .Include(g => g.GameRoot)
                .AsNoTracking();

            if (filter != null)
            {
                games = games.Where(filter);
            }

            return games.ToListAsync();
        }

        public Task<bool> AnyAsync(Expression<Func<GameDetails, bool>> predicate)
        {
            var exists = _dbContext.GameDetails.AnyAsync(predicate);

            return exists;
        }
    }
}