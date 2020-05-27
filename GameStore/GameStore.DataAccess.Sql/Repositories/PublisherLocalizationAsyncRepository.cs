using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using GameStore.Core.Abstractions;
using GameStore.Core.Models;
using GameStore.DataAccess.Sql.Context;
using GameStore.Infrastructure.Extensions;
using GameStore.Infrastructure.Logging.Interfaces;
using GameStore.Infrastructure.Logging.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.DataAccess.Sql.Repositories
{
    public class PublisherLocalizationRepository : IAsyncRepository<PublisherLocalization>
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger _logger;

        public PublisherLocalizationRepository(AppDbContext dbContext, ILogger logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public Task<PublisherLocalization> FindSingleAsync(
            Expression<Func<PublisherLocalization, bool>> predicate)
        {
            var detailsLocalization = FindSingleAsync(predicate, false);

            return detailsLocalization;
        }

        public Task<List<PublisherLocalization>> FindAllAsync(
            Expression<Func<PublisherLocalization, bool>> predicate = null)
        {
            var detailsLocalizations = _dbContext.PublisherLocalizations
                .AsNoTracking();

            if (predicate != null)
            {
                detailsLocalizations = detailsLocalizations.Where(predicate);
            }

            return detailsLocalizations.ToListAsync();
        }

        public Task<bool> AnyAsync(Expression<Func<PublisherLocalization, bool>> predicate)
        {
            var any = _dbContext.PublisherLocalizations.AnyAsync(predicate);

            return any;
        }

        public async Task AddAsync(PublisherLocalization entity)
        {
            _dbContext.PublisherLocalizations.Attach(entity);
            await _dbContext.PublisherLocalizations.AddAsync(entity);

            var entry = new LogEntry<PublisherLocalization>(Operation.Create, entity);
            _logger.Log(entry);
        }

        public async Task UpdateAsync(PublisherLocalization entity)
        {
            var existingDetails = await FindSingleAsync(r => r.Id == entity.Id, true);
            var oldValueInstance = existingDetails.Clone();
            _dbContext.Entry(existingDetails).CurrentValues.SetValues(entity);

            var entry = new LogEntry<PublisherLocalization>(Operation.Update, oldValueInstance, existingDetails);
            _logger.Log(entry);
        }

        public async Task DeleteAsync(string id)
        {
            var details = await FindSingleAsync(r => r.Id == id, true);
            _dbContext.PublisherLocalizations.Remove(details);

            var entry = new LogEntry<PublisherLocalization>(Operation.Delete, details);
            _logger.Log(entry);
        }

        private Task<PublisherLocalization> FindSingleAsync(
            Expression<Func<PublisherLocalization, bool>> predicate,
            bool isTracked)
        {
            var details = isTracked
                ? _dbContext.PublisherLocalizations
                : _dbContext.PublisherLocalizations.AsNoTracking();
            var targetDetails = details.FirstOrDefaultAsync(predicate);

            return targetDetails;
        }
    }
}