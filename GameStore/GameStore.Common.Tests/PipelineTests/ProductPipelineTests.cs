using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FakeItEasy;
using FluentAssertions;
using GameStore.Common.Pipeline;
using GameStore.Common.Pipeline.Interfaces;
using GameStore.Common.Pipeline.PipelineNodes.Interfaces;
using GameStore.Common.Pipeline.PipelineNodes.ProductNodes;
using GameStore.DataAccess.Mongo.Models;
using GameStore.DataAccess.Mongo.Repositories.Interfaces;
using NUnit.Framework;

namespace GameStore.Common.Tests.PipelineTests
{
    [TestFixture]
    public class ProductPipelineTests
    {
        private const string Id = "temp";

        private IProductRepository _productRepository;
        private IPipeline<IEnumerable<Product>> _productsPipeline;

        [SetUp]
        public void Setup()
        {
            _productRepository = A.Fake<IProductRepository>();
            _productsPipeline = new ProductPipeline(CreateTestNodes(), _productRepository);
        }

        [Test]
        public void ExecuteAsync_CallsRepository_Always()
        {
            _productsPipeline.ExecuteAsync();

            A.CallTo(() => _productRepository.FindAllAsync(A<Expression<Func<Product, bool>>>._))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public void Execute_ReturnsGameDetails_Always()
        {
            const int expectedGamesCount = 10;
            var testDetails = CreateTestCollection();
            A.CallTo(() => _productRepository.FindAllAsync(A<Expression<Func<Product, bool>>>._))
                .Returns(testDetails);

            var details = _productsPipeline.ExecuteAsync().Result;

            details.Count().Should().Be(expectedGamesCount);
        }

        private static IEnumerable<IPipelineNode<Product>> CreateTestNodes()
        {
            var nodes = new IPipelineNode<Product>[]
            {
                new NamePipelineNode(Id, Enumerable.Empty<string>()),
                new PriceRangePipelineNode(100, 200),
                new PublishersPipelineNode(Enumerable.Empty<string>(), Enumerable.Empty<string>()),
                new ExcludeKeysPipelineNode(Enumerable.Empty<string>())
            };

            return nodes;
        }

        private static List<Product> CreateTestCollection()
        {
            return Enumerable.Range(1, 10).Select(g => new Product()).ToList();
        }
    }
}