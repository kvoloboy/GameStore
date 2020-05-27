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
    public class GenresPipelineNodeTests
    {
        private IEnumerable<string> _genreNames;
        private GenresPipelineNode _genresPipelineNode;

        [SetUp]
        public void Setup()
        {
            _genreNames = new[] {"Strategy"};
            _genresPipelineNode = new GenresPipelineNode(_genreNames);
        }

        [Test]
        public void Execute_ReturnsInputExpression_WhenNotValidConstructorArgument()
        {
            _genresPipelineNode = new GenresPipelineNode(Enumerable.Empty<string>());
            Expression<Func<GameRoot, bool>> input = game => game.IsDeleted == false;
        
            var expression = _genresPipelineNode.Execute(input);
            var areEquals = Lambda.Eq(input, expression);
        
            areEquals.Should().BeTrue();
        }

        [Test]
        public void Execute_ReturnsNewExpression_WhenNullParameter()
        {
            Expression<Func<GameRoot, bool>> expected = game =>
                game.GameGenres.Any(gameGenre => _genreNames.Contains(gameGenre.GenreId));
            
            var expression = _genresPipelineNode.Execute(null);
            var areEquals = Lambda.Eq(expected, expression);
            
            areEquals.Should().BeTrue();
        }
        
        [Test]
        public void Execute_ReturnsCombinedExpression_WhenNotNullParameter()
        {
            Expression<Func<GameRoot, bool>> expectedExpression = game => true 
                && game.GameGenres.Any(gameGenre => _genreNames.Contains(gameGenre.GenreId));
            
            var expression = _genresPipelineNode.Execute(game => true);
            var areEquals = Lambda.Eq(expectedExpression, expression);

            areEquals.Should().BeTrue();
        }
    }
}