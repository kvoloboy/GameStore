using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using GameStore.BusinessLayer.DTO;
using GameStore.BusinessLayer.Exceptions;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Core.Models;
using GameStore.Web.Controllers;
using GameStore.Web.Factories.Interfaces;
using GameStore.Web.Models.ViewModels.GameViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace GameStore.Web.Tests.Controllers
{
    [TestFixture]
    public class ApiGameControllerTests
    {
        private const string Id = "1";

        private IGameService _gameService;
        private ILogger<ApiGameController> _logger;
        private IMapper _mapper;
        private IAsyncViewModelFactory<GameDto, DisplayGameViewModel> _displayGameViewModelFactory;
        private ApiGameController _apiGameController;

        [SetUp]
        public void Setup()
        {
            _gameService = A.Fake<IGameService>();
            _logger = A.Fake<ILogger<ApiGameController>>();
            _mapper = A.Fake<IMapper>();
            _displayGameViewModelFactory = A.Fake<IAsyncViewModelFactory<GameDto, DisplayGameViewModel>>();

            _apiGameController = new ApiGameController(_gameService, _logger, _mapper, _displayGameViewModelFactory);
        }

        [Test]
        public void CreateAsync_CallsService_Always()
        {
            var viewModel = GetModifyGameViewModel();

            _apiGameController.CreateAsync(viewModel);
            
            A.CallTo(() => _gameService.CreateAsync(A<ModifyGameDto>._)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void CreateAsync_ReturnsCreatedResult_WhenValidViewModel()
        {
            var viewModel = GetModifyGameViewModel();

            var result = _apiGameController.CreateAsync(viewModel).Result as CreatedAtActionResult;
            var model = result.Value as ModifyGameViewModel;

            model.Id.Should().NotBeEmpty();
        }

        [Test]
        public void CreateAsync_ReturnsBadRequest_WhenCatchValidationException()
        {
            var viewModel = GetModifyGameViewModel();
            A.CallTo(() => _gameService.CreateAsync(A<ModifyGameDto>._))
                .Throws(new ValidationException<GameRoot>(string.Empty, string.Empty));

            var result = _apiGameController.CreateAsync(viewModel).Result;

            result.Should().BeAssignableTo<BadRequestObjectResult>();
        }

        [Test]
        public void UpdateAsync_CallsService_Always()
        {
            var viewModel = GetModifyGameViewModel();

            _apiGameController.UpdateAsync(viewModel);
            
            A.CallTo(() => _gameService.UpdateAsync(A<ModifyGameDto>._)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void UpdateAsync_ReturnsNoContent_WhenValidViewModel()
        {
            var viewModel = GetModifyGameViewModel();

            var result = _apiGameController.UpdateAsync(viewModel).Result;

            result.Should().BeAssignableTo<NoContentResult>();
        }

        [Test]
        public void UpdateAsync_ReturnsBadRequest_WhenCatchValidationException()
        {
            var viewModel = GetModifyGameViewModel();
            A.CallTo(() => _gameService.UpdateAsync(A<ModifyGameDto>._))
                .Throws(new ValidationException<GameRoot>(string.Empty, string.Empty));

            var result = _apiGameController.UpdateAsync(viewModel).Result;

            result.Should().BeAssignableTo<BadRequestObjectResult>();
        }

        [Test]
        public void DeleteAsync_CallsService_Always()
        {
            _apiGameController.DeleteAsync(Id);

            A.CallTo(() => _gameService.DeleteAsync(Id)).MustHaveHappenedOnceExactly();
        }
        
        [Test]
        public void DeleteAsync_ReturnsNoContent_WhenDeleted()
        {
            var result = _apiGameController.DeleteAsync(Id).Result;

            result.Should().BeAssignableTo<NoContentResult>();
        }

        [Test]
        public void GetById_ReturnsViewMode_WhenFound()
        {
            var result = _apiGameController.GetByIdAsync(Id).Result;

            result.Value.Should().NotBeNull();
        }
        
        private static ModifyGameViewModel GetModifyGameViewModel()
        {
            var viewModel = new ModifyGameViewModel();

            return viewModel;
        }
    }
}