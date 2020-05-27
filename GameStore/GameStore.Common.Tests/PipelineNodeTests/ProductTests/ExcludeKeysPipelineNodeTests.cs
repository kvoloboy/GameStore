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
    public class ExcludeKeysPipelineNodeKeys
    {
        private readonly IEnumerable<string> _keys = new[] {"key"};
        private ExcludeKeysPipelineNode _excludeKeysPipelineNode;

        [SetUp]
        public void Setup()
        {
            _excludeKeysPipelineNode = new ExcludeKeysPipelineNode(_keys);
        }

        [Test]
        public void Execute_ReturnsInputExpression_WhenNotValidConstructorArgument()
        {
            _excludeKeysPipelineNode = new ExcludeKeysPipelineNode(Enumerable.Empty<string>());
            Expression<Func<Product, bool>> input = product => product.ProductName.Contains(string.Empty);

            var expression = _excludeKeysPipelineNode.Execute(input);
            var areEquals = Lambda.Eq(input, expression);

            areEquals.Should().BeTrue();
        }

        [Test]
        public void Execute_ReturnsNewExpression_WhenNullParameter()
        {
            Expression<Func<Product, bool>> expected = product => !_keys.Contains(product.Key);

            var expression = _excludeKeysPipelineNode.Execute(null);
            var areEquals = Lambda.Eq(expected, expression);

            areEquals.Should().BeTrue();
        }

        [Test]
        public void Execute_ReturnsCombinedExpression_WhenNotNullParameter()
        {
            Expression<Func<Product, bool>> expectedExpression = product => true && !_keys.Contains(product.Key);

            var expression = _excludeKeysPipelineNode.Execute(product => true);
            var areEquals = Lambda.Eq(expectedExpression, expression);

            areEquals.Should().BeTrue();
        }
    }
}