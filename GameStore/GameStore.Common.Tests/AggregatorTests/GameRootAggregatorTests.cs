using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using GameStore.Common.Aggregators;
using GameStore.Common.Models;
using GameStore.Common.Pipeline.Builders.Interfaces;
using GameStore.Common.Pipeline.Interfaces;
using GameStore.Common.Pipeline.PipelineNodes.Interfaces;
using GameStore.Core.Abstractions;
using GameStore.Core.Models;
using GameStore.DataAccess.Mongo.Models;
using NUnit.Framework;

namespace GameStore.Common.Tests.AggregatorTests
{
    [TestFixture]
    public class GameAggregatorTests
    {
        private const string Id = "1";
        private const string Key = "Key";

        private IBuilder<IPipeline<IEnumerable<GameRoot>>, IPipelineNode<GameRoot>> _gameRootPipelineBuilder;
        private IBuilder<IPipeline<IEnumerable<Product>>, IPipelineNode<Product>> _productPipelineBuilder;
        private IPipeline<IEnumerable<GameRoot>> _gameRootPipeline;
        private IPipeline<IEnumerable<GameDetails>> _gameDetailsPipeline;
        private IPipeline<IEnumerable<Product>> _productPipeline;
        private IMapper _mapper;

        private GameRootAggregator _gameRootAggregator;

        [SetUp]
        public void Setup()
        {
            _gameRootPipelineBuilder = A.Fake<IBuilder<IPipeline<IEnumerable<GameRoot>>, IPipelineNode<GameRoot>>>();
            _productPipelineBuilder = A.Fake<IBuilder<IPipeline<IEnumerable<Product>>, IPipelineNode<Product>>>();
            _mapper = A.Fake<IMapper>();
            _gameRootPipeline = A.Fake<IPipeline<IEnumerable<GameRoot>>>();
            _gameDetailsPipeline = A.Fake<IPipeline<IEnumerable<GameDetails>>>();
            _productPipeline = A.Fake<IPipeline<IEnumerable<Product>>>();

            A.CallTo(() => _gameRootPipelineBuilder.Build()).Returns(_gameRootPipeline);
            A.CallTo(() => _productPipelineBuilder.Build()).Returns(_productPipeline);

            _gameRootAggregator = new GameRootAggregator(
                _gameRootPipelineBuilder,
                _productPipelineBuilder,
                _mapper);
        }

        [Test]
        public void FindAllAsync_ReturnsFilteredGameRootsWithDetails_WhenFound()
        {
            var result = _gameRootAggregator.FindAllAsync(new GameFilterData());

            result.Should().NotBeNull();
        }

        [Test]
        public void FindAllAsync_AppendsDetailsFromSqlToGameRoots_WhenFound()
        {
            var testRoots = CreateGameRoots(new GameDetails());
            var details = CreateGameDetails();
            A.CallTo(() => _gameRootPipeline.ExecuteAsync()).Returns(testRoots);
            A.CallTo(() => _gameDetailsPipeline.ExecuteAsync()).Returns(details);

            var roots = _gameRootAggregator.FindAllAsync(new GameFilterData()).Result;
            var allNotEmptyDetails = roots.All(gr => gr.Details != null);

            allNotEmptyDetails.Should().BeTrue();
        }

        [Test]
        public void FindAllAsync_AppendsMongoProductsToGameRoots_WhenNotFoundDetailsInSql()
        {
            var testRoots = CreateGameRoots();
            var products = CreateProducts();
            A.CallTo(() => _gameRootPipeline.ExecuteAsync()).Returns(testRoots);
            A.CallTo(() => _productPipeline.ExecuteAsync()).Returns(products);
            A.CallTo(() => _mapper.Map<GameDetails>(A<Product>._)).Returns(new GameDetails());

            var roots = _gameRootAggregator.FindAllAsync(new GameFilterData()).Result;
            var allNotEmptyDetails = roots.All(gr => gr.Details != null);

            allNotEmptyDetails.Should().BeTrue();
        }

        private static IEnumerable<GameRoot> CreateGameRoots(GameDetails details = null)
        {
            var roots = new[]
            {
                new GameRoot {Id = Id, Key = Key, Details = details},
                new GameRoot {Id = Id, Key = Key, Details = details},
                new GameRoot {Id = Id, Key = string.Empty, Details = details},
                new GameRoot {Id = Id, Key = Key, Details = details},
                new GameRoot {Id = Id, Key = Key, Details = details}
            };

            return roots;
        }

        private static IEnumerable<GameDetails> CreateGameDetails()
        {
            var root = new GameRoot {Id = Id, Key = Key};

            var details = new[]
            {
                new GameDetails {Id = Id, GameRoot = root},
                new GameDetails {Id = Id, GameRoot = root},
                new GameDetails {Id = Id, GameRoot = root},
                new GameDetails {Id = Id, GameRoot = root},
                new GameDetails {Id = Id, GameRoot = root}
            };

            return details;
        }

        private static IEnumerable<Product> CreateProducts()
        {
            var products = new[]
            {
                new Product {Id = Id, Key = Key},
                new Product {Id = Id, Key = Key},
                new Product {Id = Id, Key = Key},
                new Product {Id = Id, Key = Key},
                new Product {Id = Id, Key = Key},
                new Product {Id = Id, Key = Key}
            };

            return products;
        }
    }
}