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
    public class GenreAsyncRepository : IAsyncRepository<Genre>
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger _logger;

        public GenreAsyncRepository(AppDbContext dbContext, ILogger logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task AddAsync(Genre entity)
        {
            _dbContext.Attach(entity);
            await _dbContext.Genres.AddAsync(entity);

            var entry = new LogEntry<Genre>(Operation.Create, entity);
            _logger.Log(entry);
        }

        public async Task UpdateAsync(Genre entity)
        {
            var existingGenre = await FindSingleAsync(g => g.Id == entity.Id);
            var oldValueInstance = existingGenre.Clone();
            _dbContext.Entry(existingGenre).CurrentValues.SetValues(entity);

            var entry = new LogEntry<Genre>(Operation.Update, oldValueInstance, entity);
            _logger.Log(entry);
        }

        public async Task DeleteAsync(string id)
        {
            var genre = await FindSingleAsync(g => g.Id == id);
            genre.IsDeleted = true;

            var entry = new LogEntry<Genre>(Operation.Delete, genre);
            _logger.Log(entry);
        }

        public Task<Genre> FindSingleAsync(Expression<Func<Genre, bool>> filter)
        {
            var genreEntity = _dbContext.Genres
                .Include(g => g.Parent)
                .Include(g => g.GameGenres).ThenInclude(genre => genre.GameRoot)
                .FirstOrDefaultAsync(filter);

            return genreEntity;
        }

        public Task<List<Genre>> FindAllAsync(Expression<Func<Genre, bool>> filter = null)
        {
            var genres = _dbContext.Genres
                .Include(g => g.Parent)
                .Include(g => g.GameGenres).ThenInclude(genre => genre.GameRoot)
                .AsNoTracking();

            if (filter != null)
            {
                genres = genres.Where(filter);
            }

            return genres.ToListAsync();
        }

        public Task<bool> AnyAsync(Expression<Func<Genre, bool>> predicate)
        {
            var exists = _dbContext.Genres.AnyAsync(predicate);

            return exists;
        }
    }
}