using System;
using System.IO;
using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using FluentAssertions.AspNetCore.Mvc;
using GameStore.BusinessLayer.DTO;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Web.Controllers;
using GameStore.Web.Models.ViewModels.ImageViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace GameStore.Web.Tests.Controllers
{
    [TestFixture]
    public class GameImageControllerTests
    {
        private const string Id = "1";

        private IGameImageService _gameImageService;
        private IMapper _mapper;
        private ILogger<GameImageController> _logger;
        private GameImageController _gameImageController;

        [SetUp]
        public void Setup()
        {
            _gameImageService = A.Fake<IGameImageService>();
            _mapper = A.Fake<IMapper>();
            _logger = A.Fake<ILogger<GameImageController>>();
            _gameImageController = new GameImageController(_gameImageService, _mapper, _logger);
        }

        [Test]
        public void IndexAsync_ReturnsView_Always()
        {
            var result = _gameImageController.IndexAsync(Id).Result as ViewResult;
            var model = result.Model;

            model.Should().BeAssignableTo<ImageIndexViewModel>();
        }

        [Test]
        public void CreateAsync_ReturnsRedirect_Always()
        {
            var viewModel = GetModifyImageViewModel();
            _gameImageController.ModelState.AddModelError(string.Empty, string.Empty);

            var result = _gameImageController.CreateAsync(viewModel).Result;

            result.Should().BeRedirectToActionResult();
        }

        [Test]
        public void DeleteAsync_ReturnsRedirect_WhenDeleted()
        {
            var result = _gameImageController.DeleteAsync(Id).Result;

            result.Should().BeRedirectToActionResult();
        }

        [Test]
        public void GetById_CallsService_Always()
        {
            var imageDto = GetGameImageDto();
            A.CallTo(() => _gameImageService.GetByIdAsync(Id))
                .Returns(imageDto);

            _gameImageController.GetById(Id);

            A.CallTo(() => _gameImageService.GetByIdAsync(Id))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public void GetById_ReturnsFile_WhenFound()
        {
            var imageDto = GetGameImageDto();
            A.CallTo(() => _gameImageService.GetByIdAsync(Id))
                .Returns(imageDto);

            var result = _gameImageController.GetById(Id);

            result.Should().BeAssignableTo<FileContentResult>();
        }

        [Test]
        public void GetByIdAsync_CallsService_Always()
        {
            var imageDto = GetGameImageDto();
            A.CallTo(() => _gameImageService.GetByIdAsync(Id))
                .Returns(imageDto);

            _gameImageController.GetByIdAsync(Id);

            A.CallTo(() => _gameImageService.GetByIdAsync(Id))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public void GetByIdAsync_ReturnsFile_WhenFound()
        {
            var imageDto = GetGameImageDto();
            A.CallTo(() => _gameImageService.GetByIdAsync(Id))
                .Returns(imageDto);

            var result = _gameImageController.GetByIdAsync(Id).Result;

            result.Should().BeAssignableTo<FileContentResult>();
        }

        private static ModifyImageViewModel GetModifyImageViewModel()
        {
            var viewModel = new ModifyImageViewModel
            {
                Image = new FormFile(new MemoryStream(), 0, long.MaxValue, Id, Id)
            };

            return viewModel;
        }

        private static GameImageDto GetGameImageDto()
        {
            var dto = new GameImageDto
            {
                Content = new byte[] {1, 2, 3},
                ContentType = "image/png"
            };

            return dto;
        }
    }
}