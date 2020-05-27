using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using GameStore.BusinessLayer.DTO;
using GameStore.Common.Decorators;
using GameStore.Common.Models;
using GameStore.Core.Abstractions;
using GameStore.Core.Models;
using NUnit.Framework;
using MongoPublisher = GameStore.DataAccess.Mongo.Models.Publisher;

namespace GameStore.Common.Tests.DecoratorsTests
{
    [TestFixture]
    public class PublisherDecoratorTests
    {
        private const string Id = "1";
        private const string Name = "Name";

        private IAsyncRepository<Publisher> _sqlPublisherRepository;
        private IAsyncReadonlyRepository<MongoPublisher> _mongoPublisherRepository;
        private IAsyncRepository<GameRoot> _gameRootRepository;
        private IAsyncRepository<PublisherLocalization> _localizationRepository;
        private IMapper _mapper;
        private PublisherDecorator _publisherDecorator;

        [SetUp]
        public void Setup()
        {
            _sqlPublisherRepository = A.Fake<IAsyncRepository<Publisher>>();
            _mongoPublisherRepository = A.Fake<IAsyncReadonlyRepository<MongoPublisher>>();
            _gameRootRepository = A.Fake<IAsyncRepository<GameRoot>>();
            _localizationRepository = A.Fake<IAsyncRepository<PublisherLocalization>>();
            _mapper = A.Fake<IMapper>();

            _publisherDecorator = new PublisherDecorator(
                _sqlPublisherRepository,
                _mongoPublisherRepository,
                _gameRootRepository,
                _localizationRepository,
                _mapper);
        }

        [Test]
        public void AddAsync_InsertsToSqlRepository_Always()
        {
            _publisherDecorator.AddAsync(new Publisher());

            A.CallTo(() => _sqlPublisherRepository.AddAsync(A<Publisher>._)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void UpdateAsync_AddsToSqlDatabase_WhenPublisherDetailsAreChanged()
        {
            var publisher = GetPublisher();
            A.CallTo(() => _mapper.Map<Publisher>(A<MongoPublisher>._)).Returns(GetPublisher("new name"));

            _publisherDecorator.UpdateAsync(publisher);

            A.CallTo(() => _sqlPublisherRepository.AddAsync(publisher)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void UpdateAsync_UpdateOnlyLocalizations_WhenPublisherDetailsAreNotChanged()
        {
            var publisher = GetPublisher();
            A.CallTo(() => _mapper.Map<Publisher>(A<MongoPublisher>._)).Returns(GetPublisher());

            _publisherDecorator.UpdateAsync(publisher);

            A.CallTo(() => _localizationRepository.AddAsync(A<PublisherLocalization>._)).MustHaveHappened();
        }

        [Test]
        public void UpdateAsync_UpdatesExistingPublisher_WhenFound()
        {
            var publisher = GetPublisher();
            A.CallTo(() => _sqlPublisherRepository.AnyAsync(A<Expression<Func<Publisher, bool>>>._)).Returns(true);

            _publisherDecorator.UpdateAsync(publisher);

            A.CallTo(() => _sqlPublisherRepository.UpdateAsync(publisher)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void DeleteAsync_CallsSqlRepository_Always()
        {
            _publisherDecorator.DeleteAsync(Id);

            A.CallTo(() => _sqlPublisherRepository.DeleteAsync(Id)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void GetByIdAsync_ReturnsResultFromSql_WhenFound()
        {
            _publisherDecorator.GetByCompanyAsync(Name);

            A.CallTo(() => _sqlPublisherRepository.FindSingleAsync(A<Expression<Func<Publisher, bool>>>._))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public void GetByIdAsync_DoesNotCallMongoRepository_WhenFoundInSql()
        {
            _publisherDecorator.GetByIdAsync(Id);

            A.CallTo(() => _mongoPublisherRepository.FindSingleAsync(A<Expression<Func<MongoPublisher, bool>>>._))
                .MustNotHaveHappened();
        }

        [Test]
        public void GetByIdAsync_ReturnsResultFromMongo_WhenNotFoundInSql()
        {
            A.CallTo(() => _sqlPublisherRepository.FindSingleAsync(A<Expression<Func<Publisher, bool>>>._))
                .Returns((Publisher) null);

            _publisherDecorator.GetByIdAsync(Id);

            A.CallTo(() => _mongoPublisherRepository.FindSingleAsync(A<Expression<Func<MongoPublisher, bool>>>._))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public void GetByUserIdAsync_ReturnsResultFromSql_Always()
        {
            _publisherDecorator.GetByUserIdAsync(Id);

            A.CallTo(() => _sqlPublisherRepository.FindSingleAsync(A<Expression<Func<Publisher, bool>>>._))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public void GetByCompanyAsync_ReturnsResultFromSql_WhenFound()
        {
            _publisherDecorator.GetByCompanyAsync(Name);

            A.CallTo(() => _sqlPublisherRepository.FindSingleAsync(A<Expression<Func<Publisher, bool>>>._))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public void GetByCompanyAsync_DoesNotCallMongoRepository_WhenFoundInSql()
        {
            _publisherDecorator.GetByCompanyAsync(Name);

            A.CallTo(() => _mongoPublisherRepository.FindSingleAsync(A<Expression<Func<MongoPublisher, bool>>>._))
                .MustNotHaveHappened();
        }

        [Test]
        public void GetByCompanyAsync_ReturnsResultFromMongo_WhenNotFoundInSql()
        {
            A.CallTo(() => _sqlPublisherRepository.FindSingleAsync(A<Expression<Func<Publisher, bool>>>._))
                .Returns((Publisher) null);

            _publisherDecorator.GetByCompanyAsync(Name);

            A.CallTo(() => _mongoPublisherRepository.FindSingleAsync(A<Expression<Func<MongoPublisher, bool>>>._))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public void GetAllAsync_CallsSqlRepositories_Always()
        {
            _publisherDecorator.GetAllAsync();

            A.CallTo(() => _sqlPublisherRepository.FindAllAsync(A<Expression<Func<Publisher, bool>>>._))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public void GetAllAsync_CallsMongoRepositories_Always()
        {
            _publisherDecorator.GetAllAsync();

            A.CallTo(() => _mongoPublisherRepository.FindAllAsync(A<Expression<Func<MongoPublisher, bool>>>._))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public void GetAllAsync_ReturnsDataFromTwoSources_Always()
        {
            const int expectedPublishersCount = 2;
            var publishers = new List<Publisher> {GetPublisher()};
            var mongoPublishers = new List<MongoPublisher> {new MongoPublisher()};
            A.CallTo(() => _sqlPublisherRepository.FindAllAsync(A<Expression<Func<Publisher, bool>>>._))
                .Returns(publishers);
            A.CallTo(() => _mongoPublisherRepository.FindAllAsync(A<Expression<Func<MongoPublisher, bool>>>._))
                .Returns(mongoPublishers);
            A.CallTo(() => _mapper.Map<IEnumerable<Publisher>>(A<IEnumerable<MongoPublisher>>._))
                .Returns(publishers);

            var publishersResult = _publisherDecorator.GetAllAsync().Result;
            var publishersCount = publishersResult.Count();

            publishersCount.Should().Be(expectedPublishersCount);
        }

        [Test]
        public void IsExistByIdAsync_ReturnsTrue_WhenExistInSql()
        {
            A.CallTo(() => _sqlPublisherRepository.AnyAsync(A<Expression<Func<Publisher, bool>>>._)).Returns(true);

            var result = _publisherDecorator.IsExistByIdAsync(Id).Result;

            result.Should().BeTrue();
        }

        [Test]
        public void IsExistByIdAsync_ReturnsTrue_WhenExistInMongo()
        {
            A.CallTo(() => _mongoPublisherRepository.AnyAsync(A<Expression<Func<MongoPublisher, bool>>>._))
                .Returns(true);

            var result = _publisherDecorator.IsExistByIdAsync(Id).Result;

            result.Should().BeTrue();
        }

        private static Publisher GetPublisher(string company = Name)
        {
            var publisher = new Publisher
            {
                Id = Id,
                CompanyName = company,
                Localizations = new[]
                {
                    new PublisherLocalization {Id = Id, CultureName = Culture.En},
                    new PublisherLocalization {CultureName = Culture.Ru}
                }
            };

            return publisher;
        }
    }
}