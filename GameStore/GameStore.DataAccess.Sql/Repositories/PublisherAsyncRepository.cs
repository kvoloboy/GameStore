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
    public class PublisherAsyncRepository : IAsyncRepository<Publisher>
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger _logger;

        public PublisherAsyncRepository(AppDbContext dbContext, ILogger logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task AddAsync(Publisher entity)
        {
            _dbContext.Attach(entity);
            await _dbContext.Publishers.AddAsync(entity);

            var entry = new LogEntry<Publisher>(Operation.Create, entity);
            _logger.Log(entry);
        }

        public async Task UpdateAsync(Publisher entity)
        {
            var existingPublisher = await _dbContext.Publishers.FindAsync(entity.Id);
            var oldValueInstance = existingPublisher.Clone();
            _dbContext.Entry(existingPublisher).CurrentValues.SetValues(entity);

            var entry = new LogEntry<Publisher>(Operation.Update, oldValueInstance, entity);
            _logger.Log(entry);
        }

        public async Task DeleteAsync(string id)
        {
            var existingPublisher = await FindSingleAsync(p => p.Id == id);
            existingPublisher.IsDeleted = true;

            var entry = new LogEntry<Publisher>(Operation.Delete, existingPublisher);
            _logger.Log(entry);
        }

        public async Task<Publisher> FindSingleAsync(Expression<Func<Publisher, bool>> filter)
        {
            var publisherEntity = await _dbContext.Publishers
                .AsNoTracking()
                .FirstOrDefaultAsync(filter);

            if (publisherEntity == null)
            {
                return null;
            }

            var localizations = await _dbContext.PublisherLocalizations
                .Where(l => l.PublisherEntityId == publisherEntity.Id)
                .ToListAsync();

            publisherEntity.Localizations = localizations;

            return publisherEntity;
        }

        public async Task<List<Publisher>> FindAllAsync(Expression<Func<Publisher, bool>> predicate = null)
        {
            var publishers = _dbContext.Publishers.AsNoTracking();

            if (predicate != null)
            {
                publishers = publishers.Where(predicate);
            }

            var publishersId = publishers.Select(p => p.Id);

            var localizations = await _dbContext.PublisherLocalizations
                .Where(l => publishersId.Contains(l.PublisherEntityId))
                .ToListAsync();

            foreach (var publisher in publishers)
            {
                publisher.Localizations =
                    localizations.Where(l => l.PublisherEntityId == publisher.Id).ToList();
            }

            return await publishers.ToListAsync();
        }

        public Task<bool> AnyAsync(Expression<Func<Publisher, bool>> predicate)
        {
            var exists = _dbContext.Publishers.AnyAsync(predicate);

            return exists;
        }
    }
}