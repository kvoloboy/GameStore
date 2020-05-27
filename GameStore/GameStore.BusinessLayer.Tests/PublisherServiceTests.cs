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
using GameStore.Common.Decorators.Interfaces;
using GameStore.Common.Models;
using GameStore.Core.Abstractions;
using GameStore.Core.Models;
using NUnit.Framework;

namespace GameStore.BusinessLayer.Tests
{
    [TestFixture]
    public class PublisherServiceTests
    {
        private const string Id = "1";

        private IPublisherDecorator _publisherDecorator;
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private PublisherService _publisherService;

        [SetUp]
        public void Setup()
        {
            _unitOfWork = A.Fake<IUnitOfWork>();
            _publisherDecorator = A.Fake<IPublisherDecorator>();
            _mapper = A.Fake<IMapper>();

            _publisherService = new PublisherService(_unitOfWork, _publisherDecorator, _mapper);
        }

        [Test]
        public async Task CreateAsync_ThrowException_WhenNullDto()
        {
            Func<Task> action = async () => await _publisherService.CreateAsync(null);

            await action.Should().ThrowAsync<InvalidServiceOperationException>()
                .WithMessage("Is null publisher dto");
        }

        [Test]
        public void CreateAsync_CallsRepository_WhenValidDto()
        {
            var dto = CreateTestModifyPublisherDto();

            _publisherService.CreateAsync(dto);

            A.CallTo(() => _publisherDecorator.AddAsync(A<Publisher>._)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task UpdateAsync_ThrowException_WhenNullDto()
        {
            Func<Task> action = async () => await _publisherService.UpdateAsync(null);

            await action.Should().ThrowAsync<InvalidServiceOperationException>()
                .WithMessage("Is null publisher dto");
        }

        [Test]
        public async Task UpdateAsync_ThrowsException_WhenNotExistsWithId()
        {
            var dto = CreateTestModifyPublisherDto();
            A.CallTo(() => _publisherDecorator.IsExistByIdAsync(Id))
                .Returns(false);

            Func<Task> action = async () => await _publisherService.UpdateAsync(dto);

            await action.Should().ThrowAsync<EntityNotFoundException<Publisher>>();
        }


        [Test]
        public void UpdateAsync_CallsRepository_WhenValidDto()
        {
            var dto = CreateTestModifyPublisherDto();
            A.CallTo(() => _publisherDecorator.IsExistByIdAsync(Id))
                .Returns(true);

            _publisherService.UpdateAsync(dto);

            A.CallTo(() => _publisherDecorator.UpdateAsync(A<Publisher>._)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task DeleteAsync_ThrowsException_WhenNotExists()
        {
            Func<Task> action = async () => await _publisherService.DeleteAsync(Id);

            await action.Should().ThrowAsync<EntityNotFoundException<Publisher>>()
                .WithMessage($"Entity Publisher wasn't found. Id: {Id}");
        }

        [Test]
        public void DeleteAsync_CallsRepository_WhenExistsWithId()
        {
            A.CallTo(() => _publisherDecorator.IsExistByIdAsync(Id))
                .Returns(true);

            _publisherService.DeleteAsync(Id);

            A.CallTo(() => _publisherDecorator.DeleteAsync(Id)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task GetByIdAsync_ThrowException_WhenNotFound()
        {
            A.CallTo(() => _publisherDecorator.GetByIdAsync(Id)).Returns((Publisher) null);

            Func<Task> action = async () => await _publisherService.GetByIdAsync(Id, Culture.En);

            await action.Should().ThrowAsync<EntityNotFoundException<Publisher>>()
                .WithMessage($"Entity Publisher wasn't found. Id: {Id}");
        }

        [Test]
        public void GetByIdAsync_ReturnsPublisherDto_WhenFound()
        {
            var dto = _publisherService.GetByIdAsync(Id, Culture.En);

            dto.Should().NotBeNull();
        }

        [Test]
        public void GetByIdAsync_ReturnsModifyPublisherDto_Always()
        {
            _publisherService.GetByIdAsync(Id);

            A.CallTo(() => _publisherDecorator.GetByIdAsync(Id)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void GetByUserIdAsync_ReturnsDto_WhenFound()
        {
            _publisherService.GetByUserIdAsync(Id, Culture.En);

            A.CallTo(() => _publisherDecorator.GetByUserIdAsync(Id)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void GetAllAsync_ReturnsCollection_Always()
        {
            A.CallTo(() => _publisherDecorator.GetAllAsync())
                .Returns(new List<Publisher>());

            var publishersDto = _publisherService.GetAllAsync();

            publishersDto.Should().NotBeNull();
        }

        [Test]
        public async Task GetByCompanyAsync_ThrowException_WhenEmptyCompany()
        {
            var companyName = string.Empty;

            Func<Task> action = async () => await _publisherService.GetByCompanyAsync(companyName);

            await action.Should().ThrowAsync<InvalidServiceOperationException>()
                .WithMessage("Is empty company name");
        }

        [Test]
        public void GetByCompanyAsync_ReturnsDto_WhenFound()
        {
            const string companyName = "test";
            A.CallTo(() => _publisherDecorator.GetByCompanyAsync(companyName))
                .Returns(new Publisher());

            var publisherDto = _publisherService.GetByCompanyAsync(companyName);

            publisherDto.Should().NotBeNull();
        }

        private static ModifyPublisherDto CreateTestModifyPublisherDto()
        {
            return new ModifyPublisherDto
            {
                Id = Id,
                CompanyName = "Microsoft",
                Localizations = new[]
                {
                    new PublisherLocalizationDto
                    {
                        Description = "description"
                    }
                }
            };
        }
    }
}