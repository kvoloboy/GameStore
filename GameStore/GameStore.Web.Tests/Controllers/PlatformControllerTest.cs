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
using GameStore.Web.Models.ViewModels.PlatformViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace GameStore.Web.Tests.Controllers
{
    [TestFixture]
    public class PlatformControllerTest
    {
        private const string Id = "1";

        private IPlatformService _platformService;
        private ILogger<PlatformController> _logger;
        private IMapper _mapper;
        private PlatformController _platformController;

        [SetUp]
        public void Setup()
        {
            _platformService = A.Fake<IPlatformService>();
            _logger = A.Fake<ILogger<PlatformController>>();
            _mapper = A.Fake<IMapper>();
            _platformController = new PlatformController(_platformService, _logger, _mapper);
        }

        [Test]
        public void GetAllAsync_ReturnsViewModelsCollection_Always()
        {
            var result = _platformController.GetAllAsync().Result as ViewResult;
            var model = result.Model;

            model.Should().BeAssignableTo<IEnumerable<PlatformViewModel>>();
        }

        [Test]
        public void Create_ReturnsViewModelWithAssignedParentGenres_Always()
        {
            var result = _platformController.Create() as ViewResult;
            var model = result.Model as PlatformViewModel;

            model.Should().NotBeNull();
        }

        [Test]
        public void CreateAsync_ReturnsView_WhenInvalidViewModel()
        {
            var testViewModel = CreatePlatformViewModel();
            var expectedName = testViewModel.Name;
            _platformController.ModelState.AddModelError(string.Empty, string.Empty);

            var result = _platformController.CreateAsync(testViewModel).Result as ViewResult;
            var model = result.Model as PlatformViewModel;
            var actualName = model.Name;

            expectedName.Should().BeEquivalentTo(actualName);
        }

        [Test]
        public void CreateAsync_ReturnsView_WhenCatchException()
        {
            var viewModel = CreatePlatformViewModel();
            const string key = nameof(viewModel.Name);
            var value = viewModel.Name;
            A.CallTo(() => _platformService.CreateAsync(A<PlatformDto>._))
                .Throws(new EntityExistsWithKeyValueException<Platform>(key, value));

            var result = _platformController.CreateAsync(viewModel).Result;

            result.Should().BeViewResult();
        }

        [Test]
        public void CreateAsync_ReturnsRedirect_WhenCreated()
        {
            var viewModel = CreatePlatformViewModel();

            var result = _platformController.CreateAsync(viewModel).Result;

            result.Should().BeRedirectToActionResult();
        }

        [Test]
        public void UpdateAsync_ReturnsViewModelWithAssignedParentGenres_Always()
        {
            var testDto = CreatePlatformDto();
            var testViewModel = CreatePlatformViewModel();
            A.CallTo(() => _platformService.GetByIdAsync(Id)).Returns(testDto);
            A.CallTo(() => _mapper.Map<PlatformViewModel>(testDto)).Returns(testViewModel);

            var result = _platformController.UpdateAsync(Id).Result as ViewResult;
            var model = result.Model as PlatformViewModel;

            model.Id.Should().Be(Id);
        }

        [Test]
        public void UpdateAsync_ReturnsView_WhenInvalidViewModel()
        {
            var testViewModel = CreatePlatformViewModel();
            var expectedName = testViewModel.Name;
            _platformController.ModelState.AddModelError(string.Empty, string.Empty);

            var result = _platformController.UpdateAsync(testViewModel).Result as ViewResult;
            var model = result.Model as PlatformViewModel;
            var actualName = model.Name;

            expectedName.Should().BeEquivalentTo(actualName);
        }

        [Test]
        public void UpdateAsync_ReturnsView_WhenCatchException()
        {
            var viewModel = CreatePlatformViewModel();
            A.CallTo(() => _platformService.UpdateAsync(A<PlatformDto>._))
                .Throws(new EntityExistsWithKeyValueException<Platform>(string.Empty, string.Empty));

            var result = _platformController.UpdateAsync(viewModel).Result;

            result.Should().BeViewResult();
        }

        [Test]
        public void UpdateAsync_ReturnsRedirect_WhenCreated()
        {
            var viewModel = CreatePlatformViewModel();

            var result = _platformController.UpdateAsync(viewModel).Result;

            result.Should().BeRedirectToActionResult();
        }

        [Test]
        public void DeleteAsync_ReturnsRedirect_WhenDeleted()
        {
            var result = _platformController.DeleteAsync(Id).Result;

            result.Should().BeRedirectToActionResult();
        }

        private static PlatformViewModel CreatePlatformViewModel()
        {
            var platformViewModel = new PlatformViewModel
            {
                Id = "1",
                Name = "Name"
            };

            return platformViewModel;
        }

        private static PlatformDto CreatePlatformDto()
        {
            var dto = new PlatformDto
            {
                Id = "1",
                Name = "Name"
            };

            return dto;
        }
    }
}