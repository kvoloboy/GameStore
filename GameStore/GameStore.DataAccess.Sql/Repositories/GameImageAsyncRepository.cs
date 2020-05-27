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
    public class GameImageAsyncRepository : IAsyncRepository<GameImage>
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger _logger;

        public GameImageAsyncRepository(AppDbContext dbContext, ILogger logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public Task<GameImage> FindSingleAsync(Expression<Func<GameImage, bool>> predicate)
        {
            var image = _dbContext.GameImages
                .Include(gameImage => gameImage.GameRoot)
                .FirstOrDefaultAsync(predicate);

            return image;
        }

        public Task<List<GameImage>> FindAllAsync(Expression<Func<GameImage, bool>> predicate)
        {
            var images = _dbContext.GameImages
                .Include(image => image.GameRoot)
                .AsNoTracking();

            if (predicate != null)
            {
                images = images. Where(predicate);
            }

            return images.ToListAsync();
        }

        public async Task AddAsync(GameImage item)
        {
            _dbContext.Attach(item);
            await _dbContext.GameImages.AddAsync(item);

            var entry = new LogEntry<GameImage>(Operation.Create, item);
            _logger.Log(entry);
        }

        public async Task UpdateAsync(GameImage item)
        {
            item.GameRoot = null;

            var image = await _dbContext.GameImages.FindAsync(item.Id);
            var existingEntityClone = image.Clone();
            _dbContext.Entry(image).CurrentValues.SetValues(item);

            var entry = new LogEntry<GameImage>(Operation.Update, existingEntityClone, item);
            _logger.Log(entry);
        }

        public async Task DeleteAsync(string id)
        {
            var image = await _dbContext.GameImages.FindAsync(id);
            _dbContext.GameImages.Remove(image);

            var entry = new LogEntry<GameImage>(Operation.Delete, image);
            _logger.Log(entry);
        }

        public Task<bool> AnyAsync(Expression<Func<GameImage, bool>> predicate)
        {
            var any = _dbContext.GameImages.AnyAsync(predicate);

            return any;
        }
    }
}