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
    public class GameRootRepository : IAsyncRepository<GameRoot>
    {
        private readonly ILogger _logger;
        private readonly AppDbContext _dbContext;

        public GameRootRepository(AppDbContext dbContext, ILogger logger)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task AddAsync(GameRoot entity)
        {
            _dbContext.Attach(entity);
            await _dbContext.GameRoots.AddAsync(entity);

            if (entity.Details != null)
            {
                _dbContext.GameDetails.Add(entity.Details);
            }

            if (entity.Localizations != null)
            {
                await _dbContext.GameLocalizations.AddRangeAsync(entity.Localizations);
            }

            var entry = new LogEntry<GameRoot>(Operation.Create, entity);
            _logger.Log(entry);
        }

        public async Task UpdateAsync(GameRoot entity)
        {
            var existingRoot = await FindSingleAsync(g => g.Id == entity.Id, true);
            var oldValueInstance = existingRoot.Clone();

            _dbContext.Entry(existingRoot).CurrentValues.SetValues(entity);
            UpdateGenres(existingRoot, entity);
            UpdatePlatforms(existingRoot, entity);

            var entry = new LogEntry<GameRoot>(Operation.Update, oldValueInstance, entity);
            _logger.Log(entry);
        }

        public async Task DeleteAsync(string id)
        {
            var existingRoot = await FindSingleAsync(r => r.Id == id, true);
            _dbContext.GameRoots.Remove(existingRoot);

            var entry = new LogEntry<GameRoot>(Operation.Delete, existingRoot);
            _logger.Log(entry);
        }

        public Task<GameRoot> FindSingleAsync(Expression<Func<GameRoot, bool>> filter)
        {
            var existingRoot = FindSingleAsync(filter, false);

            return existingRoot;
        }

        private Task<GameRoot> FindSingleAsync(Expression<Func<GameRoot, bool>> filter, bool isTracked)
        {
            var roots = isTracked ? _dbContext.GameRoots : _dbContext.GameRoots.AsNoTracking();
            var target = roots.Include(gr => gr.Comments)
                .Include(gr => gr.Details)
                .Include(gr => gr.GamePlatforms).ThenInclude(gp => gp.Platform)
                .Include(gr => gr.GameGenres).ThenInclude(gg => gg.Genre)
                .Include(gr => gr.Visit)
                .Include(gr => gr.Localizations)
                .Include(gr => gr.GameRatings)
                .Include(gr => gr.GameImages)
                .FirstOrDefaultAsync(filter);

            return target;
        }

        public Task<List<GameRoot>> FindAllAsync(Expression<Func<GameRoot, bool>> predicate = null)
        {
            var roots = _dbContext.GameRoots
                .Include(gr => gr.Comments)
                .Include(gr => gr.Details)
                .Include(gr => gr.GamePlatforms).ThenInclude(gp => gp.Platform)
                .Include(gr => gr.GameGenres).ThenInclude(gg => gg.Genre)
                .Include(gr => gr.Visit)
                .Include(gr => gr.Localizations)
                .Include(gr => gr.GameImages)
                .AsNoTracking();

            if (predicate != null)
            {
                roots = roots.Where(predicate);
            }

            return roots.ToListAsync();
        }

        public Task<bool> AnyAsync(Expression<Func<GameRoot, bool>> predicate)
        {
            var exists = _dbContext.GameRoots.AsNoTracking().AnyAsync(predicate);

            return exists;
        }

        private static void UpdateGenres(GameRoot existingGame, GameRoot gameToUpdate)
        {
            existingGame.GameGenres = gameToUpdate.GameGenres.Select(genre =>
            {
                genre.Genre = null;
                genre.GameRoot = null;

                return genre;
            }).ToList();
        }

        private static void UpdatePlatforms(GameRoot existingGame, GameRoot gameToUpdate)
        {
            existingGame.GamePlatforms = gameToUpdate.GamePlatforms.Select(platform =>
            {
                platform.Platform = null;
                platform.GameRoot = null;

                return platform;
            }).ToList();
        }
    }
}