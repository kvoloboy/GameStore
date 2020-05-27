using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using FluentAssertions.AspNetCore.Mvc;
using FluentAssertions.Common;
using GameStore.BusinessLayer.DTO;
using GameStore.BusinessLayer.Exceptions;
using GameStore.BusinessLayer.Models;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Common.Models;
using GameStore.Core.Models;
using GameStore.Web.Controllers;
using GameStore.Web.Factories.Interfaces;
using GameStore.Web.Models.ViewModels.FilterViewModels;
using GameStore.Web.Models.ViewModels.GameViewModels;
using GameStore.Web.Models.ViewModels.ImageViewModels;
using GameStore.Web.Models.ViewModels.ImageViewModels;
using GameStore.Web.Models.ViewModels.PageViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace GameStore.Web.Tests.Controllers
{
    [TestFixture]
    public class GameControllerTests
    {
        private const string Id = "1";
        private const string GameKey = "key";

        private IAsyncViewModelFactory<ModifyGameViewModel, GameViewModel> _gameViewModelFactory;
        private IAsyncViewModelFactory<GameDto, DisplayGameViewModel> _displayGameViewModelFactory;
        private IAsyncViewModelFactory<FilterSelectedOptionsViewModel, FilterViewModel> _filterViewModelFactory;
        private IViewModelFactory<PageOptions, PageOptionsViewModel> _pageOptionsViewModelFactory;
        private IViewModelFactory<string, GameImageViewModel> _gameImageViewModelFactory;
        private IGameService _gameService;
        private IRatingService _ratingService;
        private ILogger<GameController> _logger;
        private IMapper _mapper;
        private GameController _gameController;

        [SetUp]
        public void Setup()
        {
            _gameViewModelFactory = A.Fake<IAsyncViewModelFactory<ModifyGameViewModel, GameViewModel>>();
            _displayGameViewModelFactory = A.Fake<IAsyncViewModelFactory<GameDto, DisplayGameViewModel>>();
            _filterViewModelFactory = A.Fake<IAsyncViewModelFactory<FilterSelectedOptionsViewModel, FilterViewModel>>();
            _pageOptionsViewModelFactory = A.Fake<IViewModelFactory<PageOptions, PageOptionsViewModel>>();
            _gameImageViewModelFactory = A.Fake<IViewModelFactory<string, GameImageViewModel>>();
            _gameService = A.Fake<IGameService>();
            _ratingService = A.Fake<IRatingService>();
            _logger = A.Fake<Logger<GameController>>();
            _mapper = A.Fake<IMapper>();

            _gameController = new GameController(
                _gameViewModelFactory,
                _displayGameViewModelFactory,
                _filterViewModelFactory,
                _pageOptionsViewModelFactory,
                _gameImageViewModelFactory,
                _gameService,
                _ratingService,
                _logger,
                _mapper);
        }

        [Test]
        public void GetGameAsync_ReturnsNotFound_WhenEmptyKey()
        {
            var result = _gameController.GetGameAsync(string.Empty).Result;

            result.Should().BeAssignableTo<NotFoundResult>();
        }

        [Test]
        public void GetGameAsync_ReturnsDisplayGameViewModel_WhenValidKey()
        {
            var viewModel = GetDisplayGameViewModel();
            A.CallTo(() => _displayGameViewModelFactory.CreateAsync(A<GameDto>._)).Returns(viewModel);

            var result = _gameController.GetGameAsync(GameKey).Result as ViewResult;
            var model = result.Model;

            model.Should().BeAssignableTo<DisplayGameViewModel>();
        }

        [Test]
        public void GetAllAsync_ReturnsViewModelsCollection_Always()
        {
            var selectedOptions = new FilterSelectedOptionsViewModel();
            var filterViewModel = GetFilterViewModel();
            A.CallTo(() => _filterViewModelFactory.CreateAsync(selectedOptions)).Returns(filterViewModel);

            var result = _gameController.GetAllAsync(selectedOptions).Result as ViewResult;
            var model = result.Model;

            model.Should().BeAssignableTo<PageListViewModel>();
        }

        [Test]
        public void CreateAsync_ReturnsGameViewModel_Always()
        {
            var result = _gameController.CreateAsync().Result as ViewResult;
            var model = result.Model;

            model.Should().BeAssignableTo<GameViewModel>();
        }

        [Test]
        public void CreateAsync_ReturnsView_WhenInvalidViewModel()
        {
            var testModifyViewModel = CreateGameViewModel();
            var viewModel = new GameViewModel {ModifyGameViewModel = testModifyViewModel};
            A.CallTo(() => _gameViewModelFactory.CreateAsync(A<ModifyGameViewModel>._)).Returns(viewModel);
            _gameController.ModelState.AddModelError(string.Empty, string.Empty);

            var result = _gameController.CreateAsync(testModifyViewModel).Result as ViewResult;
            var model = result.Model as GameViewModel;

            model.ModifyGameViewModel.Should().BeEquivalentTo(testModifyViewModel);
        }

        [Test]
        public void CreateAsync_ReturnsView_WhenExistsGameWithSameKey()
        {
            var testViewModel = CreateGameViewModel();
            A.CallTo(() => _gameService.CreateAsync(A<ModifyGameDto>._))
                .Throws(new EntityExistsWithKeyValueException<GameRoot>(nameof(testViewModel.Key), testViewModel.Key));

            var result = _gameController.CreateAsync(testViewModel).Result as ViewResult;
            var model = result.Model as GameViewModel;

            model.ModifyGameViewModel.Should().IsSameOrEqualTo(testViewModel);
        }

        [Test]
        public void CreateAsync_ReturnsRedirect_WhenValidViewModel()
        {
            var testViewModel = CreateGameViewModel();

            var result = _gameController.CreateAsync(testViewModel).Result;

            result.Should().BeAssignableTo<RedirectToActionResult>();
        }

        [Test]
        public async Task UpdateAsync_ThrowsException_WhenEntityNotFound()
        {
            A.CallTo(() => _gameService.GetByIdAsync(Id))
                .Throws(() => new EntityNotFoundException<GameRoot>(Id));

            Func<Task> action = async () => await _gameController.UpdateAsync(Id);

            await action.Should().ThrowAsync<EntityNotFoundException<GameRoot>>();
        }

        [Test]
        public void UpdateAsync_ReturnsViewWithViewModel_WhenFoundEntityById()
        {
            var testModifyViewModel = CreateGameViewModel();
            var viewModel = new GameViewModel {ModifyGameViewModel = testModifyViewModel};
            A.CallTo(() => _gameViewModelFactory.CreateAsync(A<ModifyGameViewModel>._)).Returns(viewModel);

            var result = _gameController.UpdateAsync(Id).Result as ViewResult;
            var model = result.Model as GameViewModel;

            model.ModifyGameViewModel.Should().BeEquivalentTo(testModifyViewModel);
        }

        [Test]
        public void UpdateAsync_ReturnsView_WhenInvalidState()
        {
            var testModifyViewModel = CreateGameViewModel();
            var viewModel = new GameViewModel {ModifyGameViewModel = testModifyViewModel};
            A.CallTo(() => _gameViewModelFactory.CreateAsync(A<ModifyGameViewModel>._)).Returns(viewModel);
            var expectedName = testModifyViewModel.Name;
            _gameController.ModelState.AddModelError(string.Empty, string.Empty);

            var result = _gameController.UpdateAsync(testModifyViewModel).Result as ViewResult;
            var model = result.Model as GameViewModel;
            var actualName = model.ModifyGameViewModel.Name;

            expectedName.Should().BeEquivalentTo(actualName);
        }

        [Test]
        public void UpdateAsync_ReturnsView_WhenExistsGameWithSameKey()
        {
            var testViewModel = CreateGameViewModel();
            A.CallTo(() => _gameService.UpdateAsync(A<ModifyGameDto>._))
                .Throws(new EntityExistsWithKeyValueException<GameRoot>(nameof(testViewModel.Key), testViewModel.Key));

            var result = _gameController.UpdateAsync(testViewModel).Result as ViewResult;
            var model = result.Model as GameViewModel;

            model.ModifyGameViewModel.Should().IsSameOrEqualTo(testViewModel);
        }


        [Test]
        public void UpdateAsync_ReturnsRedirect_WhenValidViewModel()
        {
            var testViewModel = CreateGameViewModel();

            var result = _gameController.UpdateAsync(testViewModel).Result;

            result.Should().BeAssignableTo<RedirectToActionResult>();
        }

        [Test]
        public void DeleteAsync_ReturnsRedirect_WhenDeleted()
        {
            var result = _gameController.DeleteAsync(Id).Result;

            result.Should().BeRedirectToActionResult();
        }

        [Test]
        public void DownloadFile_ReturnsNotFound_WhenInvalidKey()
        {
            var gameKey = string.Empty;

            var result = _gameController.DownloadFile(gameKey).Result as NotFoundResult;

            result.Should().NotBeNull();
        }

        [Test]
        public void DownloadFile_ReturnsFile_WhenNotEmptyKey()
        {
            var file = CreateFile();
            A.CallTo(() => _gameService.GetFile(GameKey)).Returns(file);
            var result = _gameController.DownloadFile(GameKey);

            result.Result.Should().BeAssignableTo<FileContentResult>();
        }

        private static ModifyGameViewModel CreateGameViewModel()
        {
            var testViewModel = new ModifyGameViewModel
            {
                Name = "Name",
                Description = "Description"
            };

            return testViewModel;
        }

        private static AppFile CreateFile()
        {
            var file = new AppFile
            {
                Data = new byte[0],
                Mime = "application/json"
            };

            return file;
        }

        private static FilterViewModel GetFilterViewModel()
        {
            var viewModel = new FilterViewModel
            {
                FilterSelectedOptionsViewModel = new FilterSelectedOptionsViewModel(),
                FilterSelectionDataViewModel = new FilterSelectionDataViewModel()
            };

            return viewModel;
        }

        private static DisplayGameViewModel GetDisplayGameViewModel()
        {
            var viewModel = new DisplayGameViewModel
            {
                Images = new List<GameImageViewModel>()
            };

            return viewModel;
        }
    }
}