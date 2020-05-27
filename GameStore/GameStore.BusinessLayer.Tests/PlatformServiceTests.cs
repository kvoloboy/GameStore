using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using GameStore.BusinessLayer.DTO;
using GameStore.BusinessLayer.Exceptions;
using GameStore.BusinessLayer.Services;
using GameStore.Core.Abstractions;
using GameStore.Core.Models;
using NUnit.Framework;

namespace GameStore.BusinessLayer.Tests
{
    [TestFixture]
    public class PlatformServiceTests
    {
        private const string Id = "1";
        private const string PlatformName = "Name";

        private IUnitOfWork _unitOfWork;
        private IAsyncRepository<Platform> _platformsRepository;
        private IMapper _mapper;
        private PlatformService _platformService;

        [SetUp]
        public void Setup()
        {
            _mapper = A.Fake<IMapper>();
            _platformsRepository = A.Fake<IAsyncRepository<Platform>>();
            _unitOfWork = A.Fake<IUnitOfWork>();
            A.CallTo(() => _unitOfWork.GetRepository<IAsyncRepository<Platform>>()).Returns(_platformsRepository);

            _platformService = new PlatformService(_unitOfWork, _mapper);
        }

        [Test]
        public async Task CreateAsync_ThrowsException_WhenNullDto()
        {
            Func<Task> action = async () => await _platformService.CreateAsync(null);

            await action.Should().ThrowAsync<InvalidServiceOperationException>().WithMessage("Is null platform dto");
        }

        [Test]
        public async Task CreateAsync_ThrowsException_WhenExistsWithSameName()
        {
            var testPlatformDto = CreateTestPlatformDto(PlatformName);
            A.CallTo(() => _platformsRepository.AnyAsync(A<Expression<Func<Platform, bool>>>._))
                .Returns(true);

            Func<Task> action = async () => await _platformService.CreateAsync(testPlatformDto);

            await action.Should().ThrowAsync<EntityExistsWithKeyValueException<Platform>>()
                .WithMessage($"Entity Platform with Name : {PlatformName} already exists.");
        }

        [Test]
        public void CreateAsync_CallsRepository_WhenNotExistingPlatformWithSameName()
        {
            A.CallTo(() => _platformsRepository.FindSingleAsync(A<Expression<Func<Platform, bool>>>._))
                .Returns((Platform) null);
            var dto = CreateTestPlatformDto(PlatformName);

            _platformService.CreateAsync(dto);

            A.CallTo(() => _platformsRepository.AddAsync(A<Platform>._)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task UpdateAsync_ThrowsException_WhenNullDto()
        {
            Func<Task> action = async () => await _platformService.UpdateAsync(null);

            await action.Should().ThrowAsync<InvalidOperationException>().WithMessage("Is null platform dto");
        }

        [Test]
        public async Task UpdateAsync_ThrowsException_WhenNotExistsWithId()
        {
            var dto = CreateTestPlatformDto(PlatformName);
            A.CallTo(() => _platformsRepository.FindSingleAsync(A<Expression<Func<Platform, bool>>>._))
                .Returns((Platform) null);

            Func<Task> action = async () => await _platformService.UpdateAsync(dto);

            await action.Should().ThrowAsync<EntityNotFoundException<Platform>>()
                .WithMessage($"Entity Platform wasn't found. Id: {dto.Id}");
        }

        [Test]
        public async Task UpdateAsync_ThrowsException_WhenChangedNameAndNameExists()
        {
            var platform = CreateTestPlatform(PlatformName);
            var testPlatformDto = CreateTestPlatformDto(PlatformName);
            A.CallTo(() => _platformsRepository.FindSingleAsync(A<Expression<Func<Platform, bool>>>._))
                .Returns(platform);
            A.CallTo(() => _platformsRepository.AnyAsync(A<Expression<Func<Platform, bool>>>._))
                .Returns(true);

            Func<Task> action = async () => await _platformService.UpdateAsync(testPlatformDto);

            await action.Should().ThrowAsync<EntityExistsWithKeyValueException<Platform>>();
        }

        [Test]
        public void UpdateAsync_CallsRepository_WhenNotChangedName()
        {
            var testPlatformDto = CreateTestPlatformDto(PlatformName);
            var testPlatform = CreateTestPlatform(PlatformName);
            A.CallTo(() => _platformsRepository.FindSingleAsync(A<Expression<Func<Platform, bool>>>._))
                .Returns(testPlatform);
            A.CallTo(() => _mapper.Map<PlatformDto>(testPlatform)).Returns(testPlatformDto);

            _platformService.UpdateAsync(testPlatformDto);

            A.CallTo(() => _platformsRepository.UpdateAsync(A<Platform>._)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task DeleteAsync_ThrowsException_WhenNotExists()
        {
            A.CallTo(() => _platformsRepository.FindSingleAsync(A<Expression<Func<Platform, bool>>>._))
                .Returns((Platform) null);

            Func<Task> action = async () => await _platformService.DeleteAsync(Id);

            await action.Should().ThrowAsync<EntityNotFoundException<Genre>>()
                .WithMessage($"Entity Genre wasn't found. Id: {Id}");
        }

        [Test]
        public void DeleteAsync_CallsRepository_WhenExistsWithId()
        {
            A.CallTo(() => _platformsRepository.AnyAsync(A<Expression<Func<Platform, bool>>>._))
                .Returns(true);

            _platformService.DeleteAsync(Id);

            A.CallTo(() => _platformsRepository.DeleteAsync(Id)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void GetAllAsync_ReturnsCollection_Always()
        {
            A.CallTo(() => _platformsRepository.FindAllAsync(A<Expression<Func<Platform, bool>>>._))
                .Returns(new List<Platform>());

            var platformsDto = _platformService.GetAllAsync();

            platformsDto.Should().NotBeNull();
        }

        [Test]
        public async Task GetByIdAsync_ThrowsException_WhenNotFound()
        {
            A.CallTo(() => _platformsRepository.FindSingleAsync(A<Expression<Func<Platform, bool>>>._))
                .Returns((Platform) null);

            Func<Task> action = async () => await  _platformService.GetByIdAsync(Id);

            await action.Should().ThrowAsync<EntityNotFoundException<Platform>>()
                .WithMessage($"Entity Platform wasn't found. Id: {Id}");
        }

        [Test]
        public void GetByIdAsync_ReturnsDto_WhenFound()
        {
            var genre = CreateTestPlatform(PlatformName);
            var expectedGenreDto = CreateTestPlatformDto(PlatformName);
            A.CallTo(() => _platformsRepository.FindSingleAsync(A<Expression<Func<Platform, bool>>>._))
                .Returns(genre);
            A.CallTo(() => _mapper.Map<PlatformDto>(genre)).Returns(expectedGenreDto);

            var dto = _platformService.GetByIdAsync(Id).Result;

            dto.Should().BeEquivalentTo(expectedGenreDto);
        }

        private static Platform CreateTestPlatform(string name, string id = "1")
        {
            var platform = new Platform
            {
                Id = id,
                Name = name
            };

            return platform;
        }

        private static PlatformDto CreateTestPlatformDto(string name, string id = "1")
        {
            var platformDto = new PlatformDto
            {
                Id = id,
                Name = name
            };

            return platformDto;
        }
    }
}