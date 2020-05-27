using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GameStore.Common.Aggregators.Interfaces;
using GameStore.Common.Models;
using GameStore.Common.Pipeline.Builders.Interfaces;
using GameStore.Common.Pipeline.Interfaces;
using GameStore.Common.Pipeline.PipelineNodes.GameRootNodes;
using GameStore.Common.Pipeline.PipelineNodes.Interfaces;
using GameStore.Common.Pipeline.PipelineNodes.ProductNodes;
using GameStore.Core.Models;
using GameStore.DataAccess.Mongo.Models;
using NamePipelineNode = GameStore.Common.Pipeline.PipelineNodes.GameRootNodes.NamePipelineNode;
using PriceRangePipelineNode = GameStore.Common.Pipeline.PipelineNodes.GameRootNodes.PriceRangePipelineNode;
using ProductNamePipelineNode = GameStore.Common.Pipeline.PipelineNodes.ProductNodes.NamePipelineNode;
using ProductPriceRangePipelineNode = GameStore.Common.Pipeline.PipelineNodes.ProductNodes.PriceRangePipelineNode;
using ProductPublishersPipelineNode = GameStore.Common.Pipeline.PipelineNodes.ProductNodes.PublishersPipelineNode;
using PublishersPipelineNode = GameStore.Common.Pipeline.PipelineNodes.GameRootNodes.PublishersPipelineNode;

namespace GameStore.Common.Aggregators
{
    public class GameRootAggregator : IAggregator<GameFilterData, IEnumerable<GameRoot>>
    {
        private readonly IBuilder<IPipeline<IEnumerable<GameRoot>>, IPipelineNode<GameRoot>> _gameRootPipelineBuilder;
        private readonly IBuilder<IPipeline<IEnumerable<Product>>, IPipelineNode<Product>> _productPipelineBuilder;
        private readonly IMapper _mapper;

        public GameRootAggregator(
            IBuilder<IPipeline<IEnumerable<GameRoot>>, IPipelineNode<GameRoot>> gameRootPipelineBuilder,
            IBuilder<IPipeline<IEnumerable<Product>>, IPipelineNode<Product>> productPipelineBuilder,
            IMapper mapper)
        {
            _gameRootPipelineBuilder = gameRootPipelineBuilder;
            _productPipelineBuilder = productPipelineBuilder;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GameRoot>> FindAllAsync(GameFilterData filterData)
        {
            var gameRoots = await GetFilteredGameRootsAsync(filterData);
            var products = await GetFilteredMongoProductsAsync(filterData, gameRoots);
            var filteredRoots = GetGameRootsWithDetails(gameRoots, products, filterData.Name);

            return filteredRoots;
        }

        private async Task<IEnumerable<Product>> GetFilteredMongoProductsAsync(
            GameFilterData filterData,
            IEnumerable<GameRoot> filteredGameRoots)
        {
            var detailsKeys = GetGameKeys(filteredGameRoots, g => g.Details != null).ToList();

            var keysWherePublishersAreInitialized =
                GetGameKeys(filteredGameRoots, g => g.PublisherEntityId != null && g.Details == null).ToList();

            var keysWhereExistLocalization =
                GetGameKeys(filteredGameRoots, root => root.Localizations.Any());

            var productsPipeline = CreateProductsPipeline(filterData,
                detailsKeys,
                keysWherePublishersAreInitialized,
                keysWhereExistLocalization);
            var products = await productsPipeline.ExecuteAsync();

            return products;
        }

        private async Task<IEnumerable<GameRoot>> GetFilteredGameRootsAsync(GameFilterData filterData)
        {
            var gameRootsPipeline = CreateGameRootPipeline(filterData);
            var filteredRoots = await gameRootsPipeline.ExecuteAsync();

            return filteredRoots;
        }

        private IPipeline<IEnumerable<GameRoot>> CreateGameRootPipeline(GameFilterData filterData)
        {
            _gameRootPipelineBuilder
                .WithNode(new GenresPipelineNode(filterData.Genres))
                .WithNode(new PlatformsPipelineNode(filterData.Platforms))
                .WithNode(new PublishersPipelineNode(filterData.Publishers))
                .WithNode(new KeyPipelineNode(filterData.Keys))
                .WithNode(new NamePipelineNode(filterData.Name))
                .WithNode(new CreationDatePipelineNode(filterData.CreationDate))
                .WithNode(new PriceRangePipelineNode(filterData.MinPrice, filterData.MaxPrice))
                .WithNode(new AreDeletedPipelineNode(filterData.AreDeleted));

            var gameRootsPipeline = _gameRootPipelineBuilder.Build();

            return gameRootsPipeline;
        }

        private IPipeline<IEnumerable<Product>> CreateProductsPipeline(
            GameFilterData filterData,
            IEnumerable<string> sqlKeys,
            IEnumerable<string> keysWherePublishersAreInitialized,
            IEnumerable<string> keysWhereExistLocalization)
        {
            _productPipelineBuilder
                .WithNode(new ProductPublishersPipelineNode(keysWherePublishersAreInitialized, filterData.Publishers))
                .WithNode(new ProductNamePipelineNode(filterData.Name, keysWhereExistLocalization))
                .WithNode(new ProductPriceRangePipelineNode(filterData.MinPrice, filterData.MaxPrice))
                .WithNode(new ExcludeKeysPipelineNode(sqlKeys));

            var productsPipeline = _productPipelineBuilder.Build();

            return productsPipeline;
        }

        private IEnumerable<GameRoot> GetGameRootsWithDetails(
            IEnumerable<GameRoot> gameRoots,
            IEnumerable<Product> products,
            string name)
        {
            var productsKeys = products.Select(p => p.Key);
            var detailsKeys = GetGameKeys(gameRoots, g => g.Details != null || g.Localizations.Any());
            var commonKeys = detailsKeys.Union(productsKeys);
            var roots = new List<GameRoot>();

            foreach (var gameRoot in gameRoots)
            {
                var shouldSkipGame =
                    !commonKeys.Contains(gameRoot.Key) ||
                    !string.IsNullOrEmpty(name) && !gameRoot.Localizations.Any(l => l.Name.Contains(name));

                if (shouldSkipGame)
                {
                    continue;
                }

                if (gameRoot.Details != null)
                {
                    roots.Add(gameRoot);
                    continue;
                }

                SetupMongoDetails(gameRoot, products);
                roots.Add(gameRoot);
            }

            return roots;
        }

        private void SetupMongoDetails(GameRoot gameRoot, IEnumerable<Product> products)
        {
            var targetProduct = products.First(p => p.Key == gameRoot.Key);

            if (gameRoot.PublisherEntityId == null)
            {
                gameRoot.PublisherEntityId = targetProduct.SupplierId;
            }

            var localization = _mapper.Map<GameLocalization>(targetProduct);
            localization.GameRootId = gameRoot.Id;
            gameRoot.Localizations.Add(localization);

            var details = _mapper.Map<GameDetails>(targetProduct);
            details.GameRootId = gameRoot.Id;
            gameRoot.Details = details;
        }

        private static IEnumerable<string> GetGameKeys(
            IEnumerable<GameRoot> gameRoots,
            Func<GameRoot, bool> predicate)
        {
            var keys = gameRoots.Where(predicate).Select(r => r.Key);

            return keys;
        }
    }
}