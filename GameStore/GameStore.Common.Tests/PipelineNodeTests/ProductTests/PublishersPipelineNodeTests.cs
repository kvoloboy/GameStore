using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions;
using GameStore.Common.Pipeline.PipelineNodes.ProductNodes;
using GameStore.DataAccess.Mongo.Models;
using Neleus.LambdaCompare;
using NUnit.Framework;

namespace GameStore.Common.Tests.PipelineNodeTests.ProductTests
{
    [TestFixture]
    public class PublishersPipelineNodeTests
    {
        private IEnumerable<string> _publishersId;
        private IEnumerable<string> _keysWherePublishersAreInitialized;
        private PublishersPipelineNode _publishersPipelineNode;

        [SetUp]
        public void Setup()
        {
            _keysWherePublishersAreInitialized = new[] {"1"};
            _publishersId = new[] {"Microsoft"};
            _publishersPipelineNode = new PublishersPipelineNode(_keysWherePublishersAreInitialized, _publishersId);
        }

        [Test]
        public void Execute_ReturnsInputExpression_WhenNotValidConstructorArgument()
        {
            _publishersPipelineNode =
                new PublishersPipelineNode(Enumerable.Empty<string>(), Enumerable.Empty<string>());
            Expression<Func<Product, bool>> input = product => product.Discontinued == false;

            var expression = _publishersPipelineNode.Execute(input);
            var areEquals = Lambda.Eq(input, expression);

            areEquals.Should().BeTrue();
        }

        [Test]
        public void Execute_ReturnsNewExpression_WhenNullParameter()
        {
            Expression<Func<Product, bool>> expected = product =>
                _keysWherePublishersAreInitialized.Contains(product.Key) ||
                _publishersId.Contains(product.SupplierId);

            var expression = _publishersPipelineNode.Execute(null);
            var areEquals = Lambda.Eq(expected, expression);

            areEquals.Should().BeTrue();
        }

        [Test]
        public void Execute_ReturnsCombinedExpression_WhenNotNullParameter()
        {
            Expression<Func<Product, bool>> expectedExpression = product
                => true && (_keysWherePublishersAreInitialized.Contains(product.Key) ||
                            _publishersId.Contains(product.SupplierId));

            var expression = _publishersPipelineNode.Execute(game => true);
            var areEquals = Lambda.Eq(expectedExpression, expression);

            areEquals.Should().BeTrue();
        }
    }
}