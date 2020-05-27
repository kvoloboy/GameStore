using System;
using System.Linq.Expressions;
using FluentAssertions;
using GameStore.Common.Pipeline.PipelineNodes.GameRootNodes;
using GameStore.Core.Models;
using Neleus.LambdaCompare;
using NUnit.Framework;

namespace GameStore.Common.Tests.PipelineNodeTests.GameRootTests
{
    [TestFixture]
    public class PriceRangePipelineNodeTests
    {
        private const decimal MinPrice = 10;
        private const decimal MaxPrice = 20;
        private PriceRangePipelineNode _priceRangePipelineNode;

        [SetUp]
        public void Setup()
        {
            _priceRangePipelineNode = new PriceRangePipelineNode(MinPrice, MaxPrice);
        }

        [Test]
        public void Execute_ReturnsInputExpression_WhenNotValidConstructorArgument()
        {
            _priceRangePipelineNode = new PriceRangePipelineNode(0, 0);
            Expression<Func<GameRoot, bool>> input = root => root.Details.GameRootId.Contains(string.Empty);

            var expression = _priceRangePipelineNode.Execute(input);
            var areEquals = Lambda.Eq(input, expression);

            areEquals.Should().BeTrue();
        }

        [Test]
        public void Execute_ReturnsNewExpression_WhenNullParameter()
        {
            Expression<Func<GameRoot, bool>> expected = root =>
                root.Details.Price >= MinPrice && root.Details.Price <= MaxPrice || root.Details == null;

            var expression = _priceRangePipelineNode.Execute(null);
            var areEquals = Lambda.Eq(expected, expression);

            areEquals.Should().BeTrue();
        }

        [Test]
        public void Execute_ReturnsCombinedExpression_WhenNotNullParameter()
        {
            Expression<Func<GameRoot, bool>> expectedExpression =
                root => true && (root.Details.Price >= MinPrice && root.Details.Price <= MaxPrice || root.Details == null);

            var expression = _priceRangePipelineNode.Execute(game => true);
            var areEquals = Lambda.Eq(expectedExpression, expression);

            areEquals.Should().BeTrue();
        }
    }
}