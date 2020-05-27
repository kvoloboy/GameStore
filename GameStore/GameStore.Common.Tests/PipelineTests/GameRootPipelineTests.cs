using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FakeItEasy;
using FluentAssertions;
using GameStore.Common.Pipeline;
using GameStore.Common.Pipeline.PipelineNodes.GameRootNodes;
using GameStore.Common.Pipeline.PipelineNodes.Interfaces;
using GameStore.Core.Abstractions;
using GameStore.Core.Models;
using NUnit.Framework;

namespace GameStore.Common.Tests.PipelineTests
{
    [TestFixture]
    public class GameRootPipelineTests
    {
        private const string TestName = "Strategy";
        
        private GameRootPipeline _gameRootPipeline;
        private IAsyncRepository<GameRoot> _gameRootRepository;

        [SetUp]
        public void Setup()
        {
            _gameRootRepository = A.Fake<IAsyncRepository<GameRoot>>();
            _gameRootPipeline = new GameRootPipeline(CreateTestNodes(), _gameRootRepository);
        }

        [Test]
        public void ExecuteAsync_ReturnsFilteredCollection_Always()
        {
            var testGames = CreateTestGamesCollection();
            A.CallTo(() => _gameRootRepository.FindAllAsync(A<Expression<Func<GameRoot, bool>>>.Ignored)).Returns(testGames);

            var games = _gameRootPipeline.ExecuteAsync().Result;

            games.Should().BeEquivalentTo(testGames);
        }

        private static IEnumerable<IPipelineNode<GameRoot>> CreateTestNodes()
        {
            var nodes = new IPipelineNode<GameRoot>[]
            {
                new KeyPipelineNode(new[] {TestName}),
                new GenresPipelineNode(new[] {TestName}),
                new PlatformsPipelineNode(new[] {TestName}),
                new PublishersPipelineNode(new []{TestName}) 
            };

            return nodes;
        }

        private static List<GameRoot> CreateTestGamesCollection()
        {
            var firstTestGame = new GameRoot();
            firstTestGame.GameGenres = CreateTestGameGenres(firstTestGame);

            var secondTestGame = new GameRoot();
            var games = new[] {firstTestGame, secondTestGame};

            return games.ToList();
        }

        private static ICollection<GameGenre> CreateTestGameGenres(GameRoot gameRoot)
        {
            var gameGenres = new[]
            {
                new GameGenre {GameRoot = gameRoot, Genre = new Genre {Name = "Strategy"}}
            };

            return gameGenres;
        }
    }
}