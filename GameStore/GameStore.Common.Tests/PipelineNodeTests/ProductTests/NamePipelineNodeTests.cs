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
    public class NamePipelineNodeTests
    {
        private const string Name = "Name";

        private readonly IEnumerable<string> _keysWhereExistLocalization = new[] {"1"};
        private NamePipelineNode _namePipelineNode;

        [SetUp]
        public void Setup()
        {
            _namePipelineNode = new NamePipelineNode(Name, _keysWhereExistLocalization);
        }

        [Test]
        public void Execute_ReturnsInputExpression_WhenNotValidConstructorArgument()
        {
            _namePipelineNode = new NamePipelineNode(string.Empty, _keysWhereExistLocalization);
            Expression<Func<Product, bool>> input = product => product.ProductName.Contains(Name);

            var expression = _namePipelineNode.Execute(input);
            var areEquals = Lambda.Eq(input, expression);

            areEquals.Should().BeTrue();
        }

        [Test]
        public void Execute_ReturnsNewExpression_WhenNullParameter()
        {
            Expression<Func<Product, bool>> expected = product =>
                product.ProductName.Contains(Name) || _keysWhereExistLocalization.Contains(product.Key);

            var expression = _namePipelineNode.Execute(null);
            var areEquals = Lambda.Eq(expected, expression);

            areEquals.Should().BeTrue();
        }

        [Test]
        public void Execute_ReturnsCombinedExpression_WhenNotNullParameter()
        {
            Expression<Func<Product, bool>> expectedExpression = product => true &&
                (product.ProductName.Contains(Name) || _keysWhereExistLocalization.Contains(product.Key));

            var expression = _namePipelineNode.Execute(game => true);
            var areEquals = Lambda.Eq(expectedExpression, expression);

            areEquals.Should().BeTrue();
        }
    }
}