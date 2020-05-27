using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using GameStore.BusinessLayer.DTO;
using GameStore.BusinessLayer.Exceptions;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Core.Models;
using GameStore.Web.Controllers;
using GameStore.Web.Models.ViewModels.GenreViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace GameStore.Web.Tests.Controllers
{
    [TestFixture]
    public class ApiGenreControllerTests
    {
        private const string Id = "1";
        
        private IGenreService _genreService;
        private IGameService _gameService;
        private IMapper _mapper;
        private ILogger<ApiGenreController> _logger;
        private ApiGenreController _apiGenreController;

        [SetUp]
        public void Setup()
        {
            _genreService = A.Fake<IGenreService>();
            _gameService = A.Fake<IGameService>();
            _mapper = A.Fake<IMapper>();
            _logger = A.Fake<ILogger<ApiGenreController>>();
            _apiGenreController = new ApiGenreController(_genreService, _gameService, _mapper, _logger);
        }
        
        [Test]
        public void CreateAsync_CallsService_Always()
        {
            var viewModel = GetModifyGenreViewModel();

            _apiGenreController.CreateAsync(viewModel);
            
            A.CallTo(() => _genreService.CreateAsync(A<GenreDto>._)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void CreateAsync_ReturnsCreatedResult_WhenValidViewModel()
        {
            var viewModel = GetModifyGenreViewModel();

            var result = _apiGenreController.CreateAsync(viewModel).Result as CreatedAtActionResult;
            var model = result.Value as ModifyGenreViewModel;

            model.Id.Should().NotBeEmpty();
        }

        [Test]
        public void CreateAsync_ReturnsBadRequest_WhenCatchValidationException()
        {
            var viewModel = GetModifyGenreViewModel();
            A.CallTo(() => _genreService.CreateAsync(A<GenreDto>._))
                .Throws(new ValidationException<Genre>(string.Empty, string.Empty));

            var result = _apiGenreController.CreateAsync(viewModel).Result;

            result.Should().BeAssignableTo<BadRequestObjectResult>();
        }

        [Test]
        public void UpdateAsync_CallsService_Always()
        {
            var viewModel = GetModifyGenreViewModel();

            _apiGenreController.UpdateAsync(viewModel);
            
            A.CallTo(() => _genreService.UpdateAsync(A<GenreDto>._)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void UpdateAsync_ReturnsNoContent_WhenValidViewModel()
        {
            var viewModel = GetModifyGenreViewModel();

            var result = _apiGenreController.UpdateAsync(viewModel).Result;

            result.Should().BeAssignableTo<NoContentResult>();
        }

        [Test]
        public void UpdateAsync_ReturnsBadRequest_WhenCatchValidationException()
        {
            var viewModel = GetModifyGenreViewModel();
            A.CallTo(() => _genreService.UpdateAsync(A<GenreDto>._))
                .Throws(new ValidationException<Genre>(string.Empty, string.Empty));

            var result = _apiGenreController.UpdateAsync(viewModel).Result;

            result.Should().BeAssignableTo<BadRequestObjectResult>();
        }

        [Test]
        public void DeleteAsync_CallsService_Always()
        {
            _genreService.DeleteAsync(Id);

            A.CallTo(() => _genreService.DeleteAsync(Id)).MustHaveHappenedOnceExactly();
        }
        
        [Test]
        public void DeleteAsync_ReturnsNoContent_WhenDeleted()
        {
            var result = _apiGenreController.DeleteAsync(Id).Result;

            result.Should().BeAssignableTo<NoContentResult>();
        }

        [Test]
        public void GetByIdAsync_ReturnsViewMode_WhenFound()
        {
            var result = _apiGenreController.GetByIdAsync(Id).Result;

            result.Value.Should().NotBeNull();
        }

        [Test]
        public void GetGamesByGenreAsync_ReturnsCollection_WhenFound()
        {
            var result =_apiGenreController.GetGamesByGenreAsync(Id).Result;

            result.Value.Should().NotBeNull();
        }

        private static ModifyGenreViewModel GetModifyGenreViewModel()
        {
            var viewModel = new ModifyGenreViewModel();

            return viewModel;
        }
    }
}