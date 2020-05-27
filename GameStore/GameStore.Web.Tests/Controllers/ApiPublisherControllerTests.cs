using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using GameStore.BusinessLayer.DTO;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Web.Controllers;
using GameStore.Web.Models.ViewModels.PublisherViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace GameStore.Web.Tests.Controllers
{
    [TestFixture]
    public class ApiPublisherControllerTests
    {
        private const string Id = "1";
        
        private IPublisherService _publisherServices;
        private IGameService _gameService;
        private ILogger<ApiPublisherController> _logger;
        private IMapper _mapper;
        private ApiPublisherController _apiPublisherController;

        [SetUp]
        public void Setup()
        {
            _publisherServices = A.Fake<IPublisherService>();
            _gameService = A.Fake<IGameService>();
            _logger = A.Fake<ILogger<ApiPublisherController>>();
            _mapper = A.Fake<IMapper>();
            _apiPublisherController = new ApiPublisherController(_publisherServices, _gameService,_logger, _mapper);
        }

        [Test]
        public void GetByIdAsync_ReturnsViewModel_WhenFound()
        {
            var result = _apiPublisherController.GetByIdAsync(Id).Result;

            result.Value.Should().NotBeNull();
        }

        [Test]
        public void CreateAsync_CallsService_Always()
        {
            var viewModel = GetModifyPublisherViewModel();

            _apiPublisherController.CreateAsync(viewModel);

            A.CallTo(() => _publisherServices.CreateAsync(A<ModifyPublisherDto>._)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void CreateAsync_ReturnsCreatedViewModel_WhenCreated()
        {
            var viewModel = GetModifyPublisherViewModel();

            var result = _apiPublisherController.CreateAsync(viewModel).Result as CreatedAtActionResult;
            var model = result.Value as ModifyPublisherViewModel;

            model.Should().NotBeNull();
        }
        
        [Test]
        public void UpdateAsync_CallsService_Always()
        {
            var viewModel = GetModifyPublisherViewModel();

            _apiPublisherController.UpdateAsync(viewModel);

            A.CallTo(() => _publisherServices.UpdateAsync(A<ModifyPublisherDto>._)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void UpdateAsync_ReturnsNoContent_WhenUpdated()
        {
            var viewModel = GetModifyPublisherViewModel();

            var result = _apiPublisherController.UpdateAsync(viewModel).Result;

            result.Should().BeAssignableTo<NoContentResult>();
        }
        
        [Test]
        public void DeleteAsync_CallsService_Always()
        {
            _apiPublisherController.DeleteAsync(Id);

            A.CallTo(() => _publisherServices.DeleteAsync(Id)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void DeleteAsync_ReturnsNoContent_WhenUpdated()
        {
            var result = _apiPublisherController.DeleteAsync(Id).Result;

            result.Should().BeAssignableTo<NoContentResult>();
        }

        [Test]
        public void GetGamesByPublisherAsync_ReturnsGamesCollection_WhenFound()
        {
            var result = _apiPublisherController.GetGamesByPublisherAsync(Id).Result;

            result.Value.Should().NotBeNull();
        }
        
        private static ModifyPublisherViewModel GetModifyPublisherViewModel()
        {
            var viewModel = new ModifyPublisherViewModel();

            return viewModel;
        }
    }
}