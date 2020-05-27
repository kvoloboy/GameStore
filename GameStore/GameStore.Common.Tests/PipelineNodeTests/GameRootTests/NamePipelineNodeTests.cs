using System;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions;
using GameStore.Common.Pipeline.PipelineNodes.GameRootNodes;
using GameStore.Core.Models;
using Neleus.LambdaCompare;
using NUnit.Framework;

namespace GameStore.Common.Tests.PipelineNodeTests.GameRootTests
{
    [TestFixture]
    public class NamePipelineNodeTests
    {
        private const string Name = "Name";
        private NamePipelineNode _namePipelineNode;

        [SetUp]
        public void Setup()
        {
            _namePipelineNode = new NamePipelineNode(Name);
        }

        [Test]
        public void Execute_ReturnsInputExpression_WhenNotValidConstructorArgument()
        {
            _namePipelineNode = new NamePipelineNode(string.Empty);
            Expression<Func<GameRoot, bool>> input = root => root.Details.Id.Contains(Name);

            var expression = _namePipelineNode.Execute(input);
            var areEquals = Lambda.Eq(input, expression);

            areEquals.Should().BeTrue();
        }

        [Test]
        public void Execute_ReturnsNewExpression_WhenNullParameter()
        {
            Expression<Func<GameRoot, bool>>
                expected = root => root.Localizations.Any(l => l.Name.Contains(Name)) || root.Details == null;

            var expression = _namePipelineNode.Execute(null);
            var areEquals = Lambda.Eq(expected, expression);

            areEquals.Should().BeTrue();
        }

        [Test]
        public void Execute_ReturnsCombinedExpression_WhenNotNullParameter()
        {
            Expression<Func<GameRoot, bool>> expectedExpression =
                root => true && (root.Localizations.Any(l => l.Name.Contains(Name)) || root.Details == null);

            var expression = _namePipelineNode.Execute(game => true);
            var areEquals = Lambda.Eq(expectedExpression, expression);

            areEquals.Should().BeTrue();
        }
    }
}