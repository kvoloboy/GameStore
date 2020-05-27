using System;
using System.Linq.Expressions;
using FluentAssertions;
using GameStore.Common.Pipeline.PipelineNodes.ProductNodes;
using GameStore.DataAccess.Mongo.Models;
using Neleus.LambdaCompare;
using NUnit.Framework;

namespace GameStore.Common.Tests.PipelineNodeTests.ProductTests
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
            Expression<Func<Product, bool>> input = product => product.ProductName.Contains(string.Empty);
        
            var expression = _priceRangePipelineNode.Execute(input);
            var areEquals = Lambda.Eq(input, expression);
        
            areEquals.Should().BeTrue();
        }

        [Test]
        public void Execute_ReturnsNewExpression_WhenNullParameter()
        {
            Expression<Func<Product, bool>> expected = product =>
                product.UnitPrice >= MinPrice && product.UnitPrice <= MaxPrice;

            var expression = _priceRangePipelineNode.Execute(null);
            var areEquals = Lambda.Eq(expected, expression);

            areEquals.Should().BeTrue();
        }

        [Test]
        public void Execute_ReturnsCombinedExpression_WhenNotNullParameter()
        {
            Expression<Func<Product, bool>> expectedExpression =
                product => true && (product.UnitPrice >= MinPrice && product.UnitPrice <= MaxPrice);

            var expression = _priceRangePipelineNode.Execute(game => true);
            var areEquals = Lambda.Eq(expectedExpression, expression);

            areEquals.Should().BeTrue();
        }
    }
}