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
    public class KeyPipelineNodeTests
    {
        private IEnumerable<string> _keys;
        private KeyPipelineNode _keysPipelineNode;

        [SetUp]
        public void Setup()
        {
            _keys = new[] {"1"};
            _keysPipelineNode = new KeyPipelineNode(_keys);
        }
        
        [Test]
        public void Execute_ReturnsInputExpression_WhenNotValidConstructorArgument()
        {
            _keysPipelineNode = new KeyPipelineNode(Enumerable.Empty<string>());
            Expression<Func<GameRoot, bool>> input = game => game.IsDeleted == false;
        
            var expression = _keysPipelineNode.Execute(input);
            var areEquals = Lambda.Eq(input, expression);
        
            areEquals.Should().BeTrue();
        }
        
        [Test]
        public void Execute_ReturnsNewExpression_WhenNullParameter()
        {
            Expression<Func<GameRoot, bool>> expected = game => _keys.Contains(game.Key);

            var expression = _keysPipelineNode.Execute(null);
            var areEquals = Lambda.Eq(expected, expression);
            
            areEquals.Should().BeTrue();
        }
        
        [Test]
        public void Execute_ReturnsCombinedExpression_WhenNotNullParameter()
        {
            Expression<Func<GameRoot, bool>> expectedExpression = game => true && _keys.Contains(game.Key);

            var expression = _keysPipelineNode.Execute(game => true);
            var areEquals = Lambda.Eq(expectedExpression, expression);

            areEquals.Should().BeTrue();
        }
    }
}