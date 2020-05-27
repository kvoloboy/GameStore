using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using GameStore.Common.Decorators.Interfaces;
using GameStore.Common.Models;
using GameStore.Core.Abstractions;
using GameStore.Core.Models;
using MongoPublisher = GameStore.DataAccess.Mongo.Models.Publisher;

namespace GameStore.Common.Decorators
{
    public class PublisherDecorator : IPublisherDecorator
    {
        private readonly IAsyncRepository<Publisher> _sqlPublisherRepository;
        private readonly IAsyncReadonlyRepository<MongoPublisher> _mongoPublisherRepository;
        private readonly IAsyncRepository<GameRoot> _gameRootRepository;
        private readonly IAsyncRepository<PublisherLocalization> _publisherLocalizationRepository;
        private readonly IMapper _mapper;

        public PublisherDecorator(
            IAsyncRepository<Publisher> sqlPublisherRepository,
            IAsyncReadonlyRepository<MongoPublisher> mongoPublisherRepository,
            IAsyncRepository<GameRoot> gameRootRepository,
            IAsyncRepository<PublisherLocalization> publisherLocalizationRepository,
            IMapper mapper)
        {
            _sqlPublisherRepository = sqlPublisherRepository;
            _mongoPublisherRepository = mongoPublisherRepository;
            _gameRootRepository = gameRootRepository;
            _publisherLocalizationRepository = publisherLocalizationRepository;
            _mapper = mapper;
        }

        public async Task AddAsync(Publisher entity)
        {
            await _sqlPublisherRepository.AddAsync(entity);
        }

        public async Task UpdateAsync(Publisher entity)
        {
            var isSqlPublisher = await _sqlPublisherRepository.AnyAsync(p => p.Id == entity.Id);

            if (isSqlPublisher)
            {
                await _sqlPublisherRepository.UpdateAsync(entity);
                await UpdateLocalizationsAsync(entity.Localizations);

                return;
            }

            var mongoPublisher = await _mongoPublisherRepository.FindSingleAsync(p => p.Id == entity.Id);
            var mongoPublisherAsEntity = _mapper.Map<Publisher>(mongoPublisher);
            var shouldInsertToSql = !AreEqualPublishers(entity, mongoPublisherAsEntity);

            if (shouldInsertToSql)
            {
                await UpdateGameRootsAndInsert(entity);
            }
            else
            {
                var localizations = entity.Localizations.Where(localization => localization.CultureName != Culture.En);
                await UpdateLocalizationsAsync(localizations);
            }
        }

        public async Task DeleteAsync(string id)
        {
            await _sqlPublisherRepository.DeleteAsync(id);
        }

        public async Task<Publisher> GetByIdAsync(string id)
        {
            var sqlPublisher = await _sqlPublisherRepository.FindSingleAsync(p => p.Id == id);

            if (sqlPublisher != null)
            {
                sqlPublisher.CanBeUsed = true;

                return sqlPublisher;
            }

            var mongoPublisher = await _mongoPublisherRepository.FindSingleAsync(p => p.Id == id);

            if (mongoPublisher == null)
            {
                return null;
            }

            var domainPublisher = (await MapToDomainModelsAsync(mongoPublisher)).FirstOrDefault();

            return domainPublisher;
        }

        public async Task<Publisher> GetByUserIdAsync(string userId)
        {
            var publisher = await _sqlPublisherRepository.FindSingleAsync(p => p.UserId == userId);

            return publisher;
        }

        public async Task<Publisher> GetByCompanyAsync(string companyName)
        {
            var sqlPublisher = await _sqlPublisherRepository.FindSingleAsync(p => p.CompanyName == companyName);

            if (sqlPublisher != null)
            {
                return sqlPublisher;
            }

            var mongoPublisher = await _mongoPublisherRepository.FindSingleAsync(p => p.CompanyName == companyName);
            var domainPublisher = (await MapToDomainModelsAsync(mongoPublisher)).FirstOrDefault();

            return domainPublisher;
        }

        public async Task<IEnumerable<Publisher>> GetAllAsync()
        {
            var sqlPublishers = await _sqlPublisherRepository.FindAllAsync();
            SetupCanBeUsedOption(sqlPublishers, true);

            var mongoPublishers = (await _mongoPublisherRepository.FindAllAsync()).ToArray();
            var mongoDomainPublishers = await MapToDomainModelsAsync(mongoPublishers);

            var commonPublishers = sqlPublishers.Concat(mongoDomainPublishers);

            return commonPublishers;
        }

        public async Task<bool> IsExistByIdAsync(string id)
        {
            var existInSql = await _sqlPublisherRepository.AnyAsync(p => p.Id == id);

            if (existInSql)
            {
                return true;
            }

            var existInMongo = await _mongoPublisherRepository.AnyAsync(p => p.Id == id);

            return existInMongo;
        }

        private static void SetupCanBeUsedOption(IEnumerable<Publisher> publishers, bool canBeUsed)
        {
            foreach (var publisher in publishers)
            {
                publisher.CanBeUsed = canBeUsed;
            }
        }

        private async Task UpdateGameRootsAndInsert(Publisher entity)
        {
            var newPublisherId = Guid.NewGuid().ToString();
            await UpdateGamePublishersAsync(entity.Id, newPublisherId);
            entity.Id = newPublisherId;
            await _sqlPublisherRepository.AddAsync(entity);
            await UpdateLocalizationsAsync(entity.Localizations, newPublisherId);
        }

        private async Task UpdateGamePublishersAsync(string oldId, string newId)
        {
            Expression<Func<GameRoot, bool>> predicate = g => g.PublisherEntityId == oldId && g.Details == null;
            var gameRootsToUpdate = await _gameRootRepository.FindAllAsync(predicate);

            foreach (var gameRoot in gameRootsToUpdate)
            {
                gameRoot.PublisherEntityId = newId;
                await _gameRootRepository.UpdateAsync(gameRoot);
            }
        }

        private async Task<IEnumerable<Publisher>> MapToDomainModelsAsync(params MongoPublisher[] mongoPublishers)
        {
            var publishersId = mongoPublishers.Select(p => p.Id);

            var localizations = await _publisherLocalizationRepository
                .FindAllAsync(p => publishersId.Contains(p.PublisherEntityId));

            var publishers = _mapper.Map<IEnumerable<Publisher>>(mongoPublishers);

            foreach (var publisher in publishers)
            {
                var publisherLocalizations = localizations.Where(l => l.PublisherEntityId == publisher.Id);

                foreach (var localization in publisherLocalizations)
                {
                    publisher.Localizations.Add(localization);
                }
            }

            return publishers;
        }

        private async Task UpdateLocalizationsAsync(
            IEnumerable<PublisherLocalization> localizations,
            string publisherId = null)
        {
            foreach (var localization in localizations)
            {
                if (publisherId != default)
                {
                    localization.PublisherEntityId = publisherId;
                }

                if (localization.Id == default)
                {
                    await _publisherLocalizationRepository.AddAsync(localization);
                    continue;
                }

                await _publisherLocalizationRepository.UpdateAsync(localization);
            }
        }

        private static bool AreEqualPublishers(Publisher first, Publisher second)
        {
            var firstDefaultLocalization = first.Localizations.FirstOrDefault(l => l.CultureName == Culture.En);
            var secondDefaultLocalization = second.Localizations.FirstOrDefault(l => l.CultureName == Culture.En);
            var areEqualDefaultLocalizations = Equals(firstDefaultLocalization, secondDefaultLocalization);
            var areEqualPublishers = Equals(first, second);

            return areEqualPublishers && areEqualDefaultLocalizations;
        }
    }
}