using System;
using System.Collections.Generic;
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
    public class PublishersPipelineNodeTests
    {
        private IEnumerable<string> _publisherIds;
        private PublishersPipelineNode _publishersPipelineNode;

        [SetUp]
        public void Setup()
        {
            _publisherIds = new[] {"Microsoft"};
            _publishersPipelineNode = new PublishersPipelineNode(_publisherIds);
        }

        [Test]
        public void Execute_ReturnsInputExpression_WhenNotValidConstructorArgument()
        {
            _publishersPipelineNode = new PublishersPipelineNode(Enumerable.Empty<string>());
            Expression<Func<GameRoot, bool>> input = game => game.IsDeleted == false;
        
            var expression = _publishersPipelineNode.Execute(input);
            var areEquals = Lambda.Eq(input, expression);
        
            areEquals.Should().BeTrue();
        }

        [Test]
        public void Execute_ReturnsNewExpression_WhenNullParameter()
        {
            Expression<Func<GameRoot, bool>> expected = gameRoot =>
                _publisherIds.Contains(gameRoot.PublisherEntityId) ||
                gameRoot.PublisherEntityId == null && gameRoot.Details == null;
            
            var expression = _publishersPipelineNode.Execute(null);
            var areEquals = Lambda.Eq(expected, expression);

            areEquals.Should().BeTrue();
        }

        [Test]
        public void Execute_ReturnsCombinedExpression_WhenNotNullParameter()
        {
            Expression<Func<GameRoot, bool>> expectedExpression =
                game => true && (_publisherIds.Contains(game.PublisherEntityId) ||
                        game.PublisherEntityId == null && game.Details == null);

            var expression = _publishersPipelineNode.Execute(game => true);
            var areEquals = Lambda.Eq(expectedExpression, expression);

            areEquals.Should().BeTrue();
        }
    }
}