using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using GameStore.Common.Aggregators.Interfaces;
using GameStore.Common.Decorators;
using GameStore.Common.Decorators.Interfaces;
using GameStore.Common.Models;
using GameStore.Core.Abstractions;
using GameStore.Core.Models;
using GameStore.DataAccess.Mongo.Models;
using GameStore.DataAccess.Mongo.Repositories.Interfaces;
using NUnit.Framework;
using Publisher = GameStore.Core.Models.Publisher;

namespace GameStore.Common.Tests.DecoratorsTests
{
    [TestFixture]
    public class GameDecoratorTests
    {
        private const string Id = "1";
        private const short DetailsValue = 1;

        private readonly Expression<Func<GameRoot, bool>> _testExpression = game => true;

        private IAsyncRepository<GameRoot> _sqlGameRootRepository;
        private IPublisherDecorator _publisherDecorator;
        private IProductRepository _productRepository;
        private IAsyncRepository<GameDetails> _gameDetailsRepository;
        private IAsyncRepository<GameLocalization> _gameLocalizationRepository;
        private IAggregator<GameFilterData, IEnumerable<GameRoot>> _gameRootAggregator;
        private IMapper _mapper;

        private GameDecorator _gameDecorator;

        [SetUp]
        public void Setup()
        {
            _sqlGameRootRepository = A.Fake<IAsyncRepository<GameRoot>>();
            _publisherDecorator = A.Fake<IPublisherDecorator>();
            _productRepository = A.Fake<IProductRepository>();
            _gameDetailsRepository = A.Fake<IAsyncRepository<GameDetails>>();
            _gameLocalizationRepository = A.Fake<IAsyncRepository<GameLocalization>>();
            _gameRootAggregator = A.Fake<IAggregator<GameFilterData, IEnumerable<GameRoot>>>();
            _mapper = A.Fake<IMapper>();

            _gameDecorator = new GameDecorator(
                _sqlGameRootRepository,
                _publisherDecorator,
                _productRepository,
                _gameRootAggregator,
                _gameDetailsRepository,
                _gameLocalizationRepository,
                _mapper);
        }

        [Test]
        public void AddAsync_CallsRepository_Always()
        {
            var gameRoot = new GameRoot();

            _gameDecorator.AddAsync(gameRoot);

            A.CallTo(() => _sqlGameRootRepository.AddAsync(gameRoot)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void DeleteAsync_CallsRepository_Always()
        {
            _gameDecorator.DeleteAsync(Id);

            A.CallTo(() => _sqlGameRootRepository.DeleteAsync(Id)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void UpdateAsync_UpdatesGameDetails_WhenChanged()
        {
            var gameRoot = CreateGameRoot(details: new GameDetails());
            A.CallTo(() => _sqlGameRootRepository.FindSingleAsync(A<Expression<Func<GameRoot, bool>>>._))
                .Returns(gameRoot);

            _gameDecorator.UpdateAsync(new GameRoot());

            A.CallTo(() => _gameDetailsRepository.UpdateAsync(A<GameDetails>._)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void UpdateAsync_UpdatesKeyInMongoRepository_WhenDetailsFromMongoAndKeyAreChanged()
        {
            var existingRoot = CreateGameRoot();
            var entityToUpdate = CreateGameRoot("key");
            A.CallTo(() => _sqlGameRootRepository.FindSingleAsync(A<Expression<Func<GameRoot, bool>>>._))
                .Returns(existingRoot);

            _gameDecorator.UpdateAsync(entityToUpdate);

            A.CallTo(() => _productRepository.UpdateKeyAsync(A<string>._, entityToUpdate.Key))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public void UpdateAsync_CallsGameRootRepository_Always()
        {
            _gameDecorator.UpdateAsync(new GameRoot());

            A.CallTo(() => _sqlGameRootRepository.UpdateAsync(A<GameRoot>._)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void FindSingleAsync_SetupsMongoDetails_WhenSqlDetailsAreNull()
        {
            var gameRoot = _gameDecorator.FindSingleAsync(_testExpression).Result;

            gameRoot.Details.Should().NotBeNull();
        }

        [Test]
        public void FindSingleAsync_ReturnsGameRootWithInitializedPublisher_WhenFoundPublisher()
        {
            var publisher = new Publisher {Id = Id};
            A.CallTo(() => _publisherDecorator.GetByIdAsync(A<string>._)).Returns(publisher);

            var gameRoot = _gameDecorator.FindSingleAsync(_testExpression).Result;

            gameRoot.Publisher.Should().Be(publisher);
        }

        [Test]
        public void FindSingleAsync_SetsMongoDetailsPublisherIdToGameRoot_WhenPublisherIdIsNull()
        {
            var product = new Product {SupplierId = Id};
            A.CallTo(() => _productRepository.FindSingleAsync(A<Expression<Func<Product, bool>>>._)).Returns(product);

            var gameRoot = _gameDecorator.FindSingleAsync(_testExpression).Result;

            gameRoot.PublisherEntityId.Should().Be(Id);
        }

        [Test]
        public void FindAllAsync_CallsSqlRepository_Always()
        {
            _gameDecorator.FindAllAsync();

            A.CallTo(() => _sqlGameRootRepository.FindAllAsync(A<Expression<Func<GameRoot, bool>>>._))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public void FindAllAsync_CallsGameRootAggregator_Always()
        {
            var filterData = new GameFilterData();

            _gameDecorator.FindAllAsync(filterData);

            A.CallTo(() => _gameRootAggregator.FindAllAsync(filterData))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public void AnyAsync_ReturnsResultFromSqlRepository_Always()
        {
            const bool expectedResult = true;
            A.CallTo(() => _sqlGameRootRepository.AnyAsync(A<Expression<Func<GameRoot, bool>>>._))
                .Returns(expectedResult);

            var any = _gameDecorator.AnyAsync(g => true).Result;

            any.Should().Be(expectedResult);
        }

        [Test]
        public void UpdateUnitsInStockAsync_CallsMongoRepository_WhenSqlDetailsNotFound()
        {
            var gameRoot = new GameRoot {Details = null};
            A.CallTo(() => _sqlGameRootRepository.FindSingleAsync(A<Expression<Func<GameRoot, bool>>>._))
                .Returns(gameRoot);

            _gameDecorator.UpdateUnitsInStockAsync(Id, DetailsValue);

            A.CallTo(() => _productRepository.UpdateUnitsInStockAsync(Id, DetailsValue))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public void UpdateUnitsInStock_CallsSqlRepository_WhenDetailsFound()
        {
            var gameRoot = CreateGameRoot(details: new GameDetails());
            A.CallTo(() => _sqlGameRootRepository.FindSingleAsync(A<Expression<Func<GameRoot, bool>>>._))
                .Returns(gameRoot);

            _gameDecorator.UpdateUnitsInStockAsync(Id, DetailsValue);

            A.CallTo(() => _gameDetailsRepository.UpdateAsync(A<GameDetails>._)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void CountAsync_ReturnsSqlGameRootsCount_Always()
        {
            const int expectedGameRootsCount = 2;
            var testRoots = CreateRootsCollection();
            A.CallTo(() => _sqlGameRootRepository.FindAllAsync(A<Expression<Func<GameRoot, bool>>>._)).Returns(testRoots);

            var count = _gameDecorator.CountAsync().Result;

            count.Should().Be(expectedGameRootsCount);
        }

        private static GameRoot CreateGameRoot(
            string key = "Key",
            GameDetails details = null,
            string publisherId = null)
        {
            var root = new GameRoot
            {
                Key = key,
                Details = details,
                PublisherEntityId = publisherId,
                Localizations = new[]
                {
                    new GameLocalization
                    {
                        Id = Id,
                        CultureName = Culture.En
                    },
                    new GameLocalization
                    {
                        CultureName = Culture.Ru
                    }
                }
            };

            return root;
        }

        private static List<GameRoot> CreateRootsCollection()
        {
            var gameRoots = new[]
            {
                new GameRoot(),
                new GameRoot()
            };
            return gameRoots.ToList();
        }
    }
}