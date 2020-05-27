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
    public class PlatformsPipelineNodeTests
    {
        private IEnumerable<string> _platformNames;
        private PlatformsPipelineNode _platformsPipelineNode;

        [SetUp]
        public void Setup()
        {
            _platformNames = new[] {"PC"};
            _platformsPipelineNode = new PlatformsPipelineNode(_platformNames);
        }

        [Test]
        public void Execute_ReturnsInputExpression_WhenNotValidConstructorArgument()
        {
            _platformsPipelineNode = new PlatformsPipelineNode(Enumerable.Empty<string>());
            Expression<Func<GameRoot, bool>> input = game => game.IsDeleted == false;
        
            var expression = _platformsPipelineNode.Execute(input);
            var areEquals = Lambda.Eq(input, expression);
        
            areEquals.Should().BeTrue();
        }

        [Test]
        public void Execute_ReturnsNewExpression_WhenNullParameter()
        {
            Expression<Func<GameRoot, bool>> expected = game =>
                game.GamePlatforms.Any(gamePlatform => _platformNames.Contains(gamePlatform.PlatformId));
            
            var expression = _platformsPipelineNode.Execute(null);
            var areEquals = Lambda.Eq(expected, expression);
            
            areEquals.Should().BeTrue();
        }
        
        [Test]
        public void Execute_ReturnsCombinedExpression_WhenNotNullParameter()
        {
            Expression<Func<GameRoot, bool>> expectedExpression = game => true &&
                game.GamePlatforms.Any(gamePlatform => _platformNames.Contains(gamePlatform.PlatformId));

            var expression = _platformsPipelineNode.Execute(game => true);
            var areEquals = Lambda.Eq(expectedExpression, expression);

            areEquals.Should().BeTrue();
        }
    }
}