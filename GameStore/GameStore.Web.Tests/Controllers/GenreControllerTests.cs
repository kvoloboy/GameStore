using System.Collections.Generic;
using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using FluentAssertions.AspNetCore.Mvc;
using GameStore.BusinessLayer.DTO;
using GameStore.BusinessLayer.Exceptions;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Core.Models;
using GameStore.Web.Controllers;
using GameStore.Web.Factories.Interfaces;
using GameStore.Web.Models.ViewModels.GenreViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace GameStore.Web.Tests.Controllers
{
    [TestFixture]
    public class GenreControllerTests
    {
        private const string Id = "1";

        private IGenreService _genreService;
        private IAsyncViewModelFactory<ModifyGenreViewModel, GenreViewModel> _genreViewModelFactory;
        private ILogger<GenreController> _logger;
        private IMapper _mapper;
        private GenreController _genreController;

        [SetUp]
        public void Setup()
        {
            _genreService = A.Fake<IGenreService>();
            _genreViewModelFactory = A.Fake<IAsyncViewModelFactory<ModifyGenreViewModel, GenreViewModel>>();
            _logger = A.Fake<ILogger<GenreController>>();
            _mapper = A.Fake<IMapper>();
            _genreController = new GenreController(_genreService, _genreViewModelFactory, _logger, _mapper);
        }

        [Test]
        public void GetAllAsync_ReturnsViewModelsCollection_Always()
        {
            var result = _genreController.GetAllAsync().Result as ViewResult;
            var model = result.Model;

            model.Should().BeAssignableTo<IEnumerable<ModifyGenreViewModel>>();
        }

        [Test]
        public void CreateAsync_ReturnsViewModelWithAssignedParentGenres_Always()
        {
            var testGenreViewModel = CreateTestGenreViewModel();
            A.CallTo(() => _genreViewModelFactory.CreateAsync(A<ModifyGenreViewModel>._))
                .Returns(testGenreViewModel);

            var result = _genreController.CreateAsync().Result as ViewResult;
            var model = result.Model as GenreViewModel;

            model.Parents.Should().NotBeNull();
        }

        [Test]
        public void CreateAsync_ReturnsView_WhenInvalidViewModel()
        {
            var modifyGenreViewModel = CreateModifyGenreViewModel();
            var genreViewModel = CreateTestGenreViewModel();
            A.CallTo(() => _genreViewModelFactory.CreateAsync(modifyGenreViewModel))
                .Returns(genreViewModel);
            var expectedName = modifyGenreViewModel.Name;
            _genreController.ModelState.AddModelError(string.Empty, string.Empty);

            var result = _genreController.CreateAsync(modifyGenreViewModel).Result as ViewResult;
            var model = result.Model as GenreViewModel;
            var actualName = model.ModifyGenreViewModel.Name;

            expectedName.Should().BeEquivalentTo(actualName);
        }

        [Test]
        public void CreateAsync_ReturnsView_WhenCatchException()
        {
            var viewModel = CreateModifyGenreViewModel();
            const string key = nameof(viewModel.Name);
            var value = viewModel.Name;
            A.CallTo(() => _genreService.CreateAsync(A<GenreDto>._))
                .Throws(new EntityExistsWithKeyValueException<Genre>(key, value));

            var result = _genreController.CreateAsync(viewModel).Result;

            result.Should().BeViewResult();
        }

        [Test]
        public void CreateAsync_ReturnsRedirect_WhenCreated()
        {
            var viewModel = CreateModifyGenreViewModel();

            var result = _genreController.CreateAsync(viewModel).Result;

            result.Should().BeRedirectToActionResult();
        }

        [Test]
        public void UpdateAsync_ReturnsViewModelWithAssignedParentGenres_Always()
        {
            var result = _genreController.UpdateAsync(Id).Result as ViewResult;
            var model = result.Model;

            model.Should().BeAssignableTo<GenreViewModel>();
        }

        [Test]
        public void UpdateAsync_ReturnsView_WhenInvalidViewModel()
        {
            var testViewModel = CreateModifyGenreViewModel();
            var genreViewModel = CreateTestGenreViewModel();
            A.CallTo(() => _genreViewModelFactory.CreateAsync(testViewModel))
                .Returns(genreViewModel);
            _genreController.ModelState.AddModelError(string.Empty, string.Empty);

            var result = _genreController.UpdateAsync(testViewModel).Result as ViewResult;
            var model = result.Model;

            model.Should().BeAssignableTo<GenreViewModel>();
        }

        [Test]
        public void UpdateAsync_ReturnsView_WhenCatchException()
        {
            var viewModel = CreateModifyGenreViewModel();
            A.CallTo(() => _genreService.UpdateAsync(A<GenreDto>._))
                .Throws(new EntityExistsWithKeyValueException<Genre>(string.Empty, string.Empty));

            var result = _genreController.UpdateAsync(viewModel).Result;

            result.Should().BeViewResult();
        }

        [Test]
        public void UpdateAsync_ReturnsRedirect_WhenUpdated()
        {
            var viewModel = CreateModifyGenreViewModel();

            var result = _genreController.UpdateAsync(viewModel).Result;

            result.Should().BeRedirectToActionResult();
        }

        [Test]
        public void DeleteAsync_ReturnsRedirect_WhenDeleted()
        {
            var result = _genreController.DeleteAsync(Id).Result;

            result.Should().BeRedirectToActionResult();
        }

        private static ModifyGenreViewModel CreateModifyGenreViewModel()
        {
            var genreViewModel = new ModifyGenreViewModel
            {
                Id = "1",
                Name = "Name"
            };

            return genreViewModel;
        }

        private static GenreViewModel CreateTestGenreViewModel()
        {
            var genreViewModel = new GenreViewModel
            {
                ModifyGenreViewModel = new ModifyGenreViewModel {Name = "Name"},
                Parents = new List<ModifyGenreViewModel>()
            };

            return genreViewModel;
        }
    }
}