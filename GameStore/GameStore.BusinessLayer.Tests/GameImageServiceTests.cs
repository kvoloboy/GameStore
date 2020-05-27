using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using GameStore.BusinessLayer.DTO;
using GameStore.BusinessLayer.Exceptions;
using GameStore.BusinessLayer.Services;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Core.Abstractions;
using GameStore.Core.Models;
using NUnit.Framework;

namespace GameStore.BusinessLayer.Tests
{
    [TestFixture]
    public class GameImageServiceTests
    {
        private const string Id = "1";

        private IUnitOfWork _unitOfWork;
        private IGameService _gameService;
        private IMapper _mapper;
        private IAsyncRepository<GameImage> _gameImageRepository;

        private IGameImageService _gameImageService;

        [SetUp]
        public void Setup()
        {
            _unitOfWork = A.Fake<IUnitOfWork>();
            _gameService = A.Fake<IGameService>();
            _mapper = A.Fake<IMapper>();
            _gameImageRepository = A.Fake<IAsyncRepository<GameImage>>();
            A.CallTo(() => _unitOfWork.GetRepository<IAsyncRepository<GameImage>>()).Returns(_gameImageRepository);

            _gameImageService = new GameImageService(_unitOfWork, _gameService, _mapper);
        }

        [Test]
        public async Task GetByGameKeyAsync_ThrowsException_WhenEmptyGameKey()
        {
            Func<Task> action = async () => await _gameImageService.GetByGameKeyAsync(string.Empty);

            await action.Should().ThrowAsync<InvalidServiceOperationException>();
        }

        [Test]
        public void GetByGameKeyAsync_CallsRepository_WhenValidGameKey()
        {
            _gameImageService.GetByGameKeyAsync(Id);

            A.CallTo(() => _gameImageRepository.FindAllAsync(A<Expression<Func<GameImage, bool>>>._))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task GetByIdAsync_ThrowsException_WhenNotFound()
        {
            A.CallTo(() => _gameImageRepository.FindSingleAsync(A<Expression<Func<GameImage, bool>>>._))
                .Returns((GameImage) null);

            Func<Task> action = async () => await _gameImageService.GetByIdAsync(Id);

            await action.Should().ThrowAsync<EntityNotFoundException<GameImage>>();
        }

        [Test]
        public void GetByIdAsync_ReturnsGameImageDto_WhenFound()
        {
            var imageDto = _gameImageService.GetByIdAsync(Id).Result;

            imageDto.Should().NotBeNull();
        }

        [Test]
        public async Task CreateAsync_ThrowsException_WhenEmptyImageContent()
        {
            var dto = GetGameImageDto(Id);

            Func<Task> action = async () => await _gameImageService.CreateAsync(dto);

            await action.Should().ThrowAsync<InvalidServiceOperationException>()
                .WithMessage("Is empty image");
        }

        [Test]
        public async Task CreateAsync_ThrowsException_WhenEmptyGameKey()
        {
            var dto = GetGameImageDto(content: new byte[] {1});

            Func<Task> action = async () => await _gameImageService.CreateAsync(dto);

            await action.Should().ThrowAsync<InvalidServiceOperationException>()
                .WithMessage("Is empty game key");
        }

        [Test]
        public void CreateAsync_SavesToRepository_WhenValid()
        {
            var dto = GetGameImageDto(Id, new byte[] {1});

            _gameImageService.CreateAsync(dto);

            A.CallTo(() => _gameImageRepository.AddAsync(A<GameImage>._)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task DeleteAsync_ThrowsException_WhenNotExist()
        {
            Func<Task> action = async () => await _gameImageService.DeleteAsync(Id);

            await action.Should().ThrowAsync<EntityNotFoundException<GameImage>>();
        }

        [Test]
        public void DeleteAsync_CallsRepository_WhenExist()
        {
            A.CallTo(() => _gameImageRepository.AnyAsync(A<Expression<Func<GameImage, bool>>>._)).Returns(true);

            _gameImageService.DeleteAsync(Id);

            A.CallTo(() => _gameImageRepository.DeleteAsync(Id)).MustHaveHappenedOnceExactly();
        }

        private static GameImageDto GetGameImageDto(string gameKey = "", byte[] content = null)
        {
            var dto = new GameImageDto
            {
                GameKey = gameKey,
                Content = content
            };

            return dto;
        }
    }
}