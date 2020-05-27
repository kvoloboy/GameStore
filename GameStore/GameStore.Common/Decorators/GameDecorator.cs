using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using GameStore.Common.Aggregators.Interfaces;
using GameStore.Common.Decorators.Interfaces;
using GameStore.Common.Models;
using GameStore.Core.Abstractions;
using GameStore.Core.Models;
using GameStore.DataAccess.Mongo.Repositories.Interfaces;
using Publisher = GameStore.Core.Models.Publisher;

namespace GameStore.Common.Decorators
{
    public class GameDecorator : IGameDecorator
    {
        private readonly IAsyncRepository<GameRoot> _sqlGameRootRepository;
        private readonly IPublisherDecorator _publisherDecorator;
        private readonly IProductRepository _productRepository;
        private readonly IAggregator<GameFilterData, IEnumerable<GameRoot>> _gameRootAggregator;
        private readonly IAsyncRepository<GameDetails> _gameDetailsRepository;
        private readonly IAsyncRepository<GameLocalization> _gameLocalizationRepository;
        private readonly IMapper _mapper;

        public GameDecorator(
            IAsyncRepository<GameRoot> sqlGameRootRepository,
            IPublisherDecorator publisherDecorator,
            IProductRepository productRepository,
            IAggregator<GameFilterData, IEnumerable<GameRoot>> gameRootAggregator,
            IAsyncRepository<GameDetails> gameDetailsRepository,
            IAsyncRepository<GameLocalization> gameLocalizationRepository,
            IMapper mapper)
        {
            _sqlGameRootRepository = sqlGameRootRepository;
            _publisherDecorator = publisherDecorator;
            _productRepository = productRepository;
            _gameRootAggregator = gameRootAggregator;
            _gameDetailsRepository = gameDetailsRepository;
            _gameLocalizationRepository = gameLocalizationRepository;
            _mapper = mapper;
        }

        public async Task AddAsync(GameRoot entity)
        {
            await _sqlGameRootRepository.AddAsync(entity);
        }

        public async Task UpdateAsync(GameRoot entity)
        {
            var existingGameRoot = await _sqlGameRootRepository.FindSingleAsync(r => r.Id == entity.Id);
            var isMongoDetails = existingGameRoot.Details == null;

            if (isMongoDetails)
            {
                await VerifyIfDetailsHaveChangedAsync(entity, existingGameRoot);
            }

            else
            {
                var areChangedDetails = !Equals(existingGameRoot.Details, entity.Details);

                if (areChangedDetails)
                {
                    await _gameDetailsRepository.UpdateAsync(entity.Details);
                }

                await UpdateLocalizationsAsync(entity.Localizations);
            }

            await _sqlGameRootRepository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(string id)
        {
            await _sqlGameRootRepository.DeleteAsync(id);
        }

        public async Task<GameRoot> FindSingleAsync(Expression<Func<GameRoot, bool>> predicate)
        {
            var root = await GetGameRootAsync(predicate);

            return root;
        }

        public async Task<List<GameRoot>> FindAllAsync(Expression<Func<GameRoot, bool>> predicate = null)
        {
            var roots = await _sqlGameRootRepository.FindAllAsync(predicate);

            return roots;
        }

        public async Task<IEnumerable<GameRoot>> FindAllAsync(GameFilterData filterData)
        {
            var roots = await _gameRootAggregator.FindAllAsync(filterData);

            return roots;
        }

        public async Task<bool> AnyAsync(Expression<Func<GameRoot, bool>> predicate)
        {
            var any = await _sqlGameRootRepository.AnyAsync(predicate);

            return any;
        }

        public async Task UpdateUnitsInStockAsync(string key, short newValue)
        {
            var gamerRoot = await _sqlGameRootRepository.FindSingleAsync(gr => gr.Key == key);

            if (gamerRoot.Details == null)
            {
                await _productRepository.UpdateUnitsInStockAsync(key, newValue);
            }
            else
            {
                gamerRoot.Details.UnitsInStock = newValue;
                await _gameDetailsRepository.UpdateAsync(gamerRoot.Details);
            }
        }

        public async Task<int> CountAsync()
        {
            var gamesCount = (await _sqlGameRootRepository.FindAllAsync())?.Count() ?? 0;

            return gamesCount;
        }

        private async Task VerifyIfDetailsHaveChangedAsync(GameRoot gameRootToUpdate, GameRoot existingGameRoot)
        {
            var product = await _productRepository.FindSingleAsync(p => p.Key == existingGameRoot.Key);
            var productAsGameRoot = _mapper.Map<GameRoot>(product);
            var isChangedKey = gameRootToUpdate.Key != existingGameRoot.Key;

            var isChanged = !AreEqualRoots(gameRootToUpdate, productAsGameRoot);

            if (isChanged)
            {
                await _gameDetailsRepository.AddAsync(gameRootToUpdate.Details);
            }

            var localizations = isChanged
                ? gameRootToUpdate.Localizations
                : gameRootToUpdate.Localizations.Where(localization => localization.CultureName != Culture.En);

            await UpdateLocalizationsAsync(localizations);

            if (isChangedKey)
            {
                await _productRepository.UpdateKeyAsync(product.Id, gameRootToUpdate.Key);
            }
        }

        private async Task<GameRoot> GetGameRootAsync(Expression<Func<GameRoot, bool>> predicate)
        {
            var root = await _sqlGameRootRepository.FindSingleAsync(predicate);

            if (root == null)
            {
                return null;
            }
            
            if (root.Details == null)
            {
                await SetupDetailsAsync(root);
            }

            root.Publisher = await GetPublisherAsync(root.PublisherEntityId);

            return root;
        }

        private async Task SetupDetailsAsync(GameRoot gameRoot)
        {
            var product = await _productRepository.FindSingleAsync(p => p.Key == gameRoot.Key);

            if (gameRoot.PublisherEntityId == null)
            {
                gameRoot.PublisherEntityId = product.SupplierId;
            }

            var localization = _mapper.Map<GameLocalization>(product);
            localization.GameRootId = gameRoot.Id;
            gameRoot.Localizations.Add(localization);

            var details = _mapper.Map<GameDetails>(product);
            details.GameRootId = gameRoot.Id;
            gameRoot.Details = details;
        }

        private async Task<Publisher> GetPublisherAsync(string id)
        {
            var publisher = await _publisherDecorator.GetByIdAsync(id) ?? Publisher.None;

            return publisher;
        }

        private async Task UpdateLocalizationsAsync(IEnumerable<GameLocalization> localizations)
        {
            foreach (var localization in localizations)
            {
                if (localization.Id == default)
                {
                    await _gameLocalizationRepository.AddAsync(localization);
                    continue;
                }

                await _gameLocalizationRepository.UpdateAsync(localization);
            }
        }

        private static bool AreEqualRoots(GameRoot first, GameRoot second)
        {
            var areEqualRoots = Equals(first, second);
            
            var firstDefaultLocalization = first.Localizations.FirstOrDefault(l => l.CultureName == Culture.En);
            var secondDefaultLocalization = second.Localizations.FirstOrDefault(l => l.CultureName == Culture.En);
            var areEqualDefaultLocalizations = Equals(firstDefaultLocalization, secondDefaultLocalization);

            return areEqualRoots && areEqualDefaultLocalizations;
        }
    }
}