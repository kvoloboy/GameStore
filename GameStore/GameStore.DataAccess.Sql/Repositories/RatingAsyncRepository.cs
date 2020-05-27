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
    public class RatingAsyncRepository : IAsyncRepository<Rating>
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger _logger;

        public RatingAsyncRepository(AppDbContext dbContext, ILogger logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public Task<Rating> FindSingleAsync(Expression<Func<Rating, bool>> predicate)
        {
            var rating = _dbContext.Ratings.FirstOrDefaultAsync(predicate);

            return rating;
        }

        public Task<List<Rating>> FindAllAsync(Expression<Func<Rating, bool>> predicate = null)
        {
            var ratings = _dbContext.Ratings.AsNoTracking();

            if (predicate != null)
            {
                ratings = ratings.Where(predicate);
            }

            return ratings.ToListAsync();
        }

        public Task<bool> AnyAsync(Expression<Func<Rating, bool>> predicate)
        {
            var any = _dbContext.Ratings.AnyAsync(predicate);

            return any;
        }

        public async Task AddAsync(Rating entity)
        {
            _dbContext.Attach(entity);
            await _dbContext.Ratings.AddAsync(entity);

            var entry = new LogEntry<Rating>(Operation.Create, entity);
            _logger.Log(entry);
        }

        public async Task UpdateAsync(Rating entity)
        {
            var existingRating = await FindSingleAsync(rating => rating.Id == entity.Id);
            var oldEntityVersion = existingRating.Clone();
            _dbContext.Entry(existingRating).CurrentValues.SetValues(entity);

            var entry = new LogEntry<Rating>(Operation.Update, oldEntityVersion, entity);
            _logger.Log(entry);
        }

        public async Task DeleteAsync(string id)
        {
            var existingRating = await FindSingleAsync(rating => rating.Id == id);
            _dbContext.Ratings.Remove(existingRating);

            var logEntry = new LogEntry<Rating>(Operation.Delete, existingRating);
            _logger.Log(logEntry);
        }
    }
}