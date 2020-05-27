using System.Collections.Generic;
using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using FluentAssertions.AspNetCore.Mvc;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Web.Controllers;
using GameStore.Web.Models.ViewModels.PublisherViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace GameStore.Web.Tests.Controllers
{
    [TestFixture]
    public class PublisherControllerTests
    {
        private const string Id = "1";
        private const string CompanyName = "Company";


        private IPublisherService _publisherService;
        private ILogger<PublisherController> _logger;
        private IMapper _mapper;
        private PublisherController _publisherController;

        [SetUp]
        public void SetUp()
        {
            _publisherService = A.Fake<IPublisherService>();
            _logger = A.Fake<ILogger<PublisherController>>();
            _mapper = A.Fake<IMapper>();
            _publisherController = new PublisherController(_publisherService, _logger, _mapper);
        }

        [Test]
        public void GetAllAsync_ReturnsViewModelsCollection_Always()
        {
            var result = _publisherController.GetAllAsync().Result as ViewResult;
            var model = result.Model;

            model.Should().BeAssignableTo<IEnumerable<PublisherViewModel>>();
        }

        [Test]
        public void DetailsAsync_ReturnsNotFound_WhenEmptyCompanyName()
        {
            var companyName = string.Empty;

            var result = _publisherController.DetailsAsync(companyName).Result as NotFoundResult;

            result.Should().NotBeNull();
        }

        [Test]
        public void DetailsAsync_ReturnsViewWithModel_WhenValidCompanyName()
        {
            var result = _publisherController.DetailsAsync(CompanyName).Result as ViewResult;
            var model = result.Model as PublisherViewModel;

            model.Should().NotBeNull();
        }

        [Test]
        public void Create_ReturnsView_Always()
        {
            var result = _publisherController.Create();

            result.Should().BeViewResult();
        }

        [Test]
        public void CreateAsync_ReturnsView_WhenNotValidViewModel()
        {
            var testViewModel = CreateModifyPublisherViewModel();
            _publisherController.ModelState.AddModelError(string.Empty, string.Empty);

            var result = _publisherController.CreateAsync(testViewModel).Result as ViewResult;
            var model = result.Model;

            model.Should().BeAssignableTo<ModifyPublisherViewModel>();
        }

        [Test]
        public void CreateAsync_ReturnsRedirect_WhenCreated()
        {
            var testViewModel = CreateModifyPublisherViewModel();

            var result = _publisherController.CreateAsync(testViewModel).Result;

            result.Should().BeRedirectToActionResult();
        }

        [Test]
        public void UpdateAsync_ReturnsView_WhenFoundById()
        {
            var result = _publisherController.UpdateAsync(Id).Result;

            result.Should().BeViewResult();
        }

        [Test]
        public void UpdateAsync_ReturnsView_WhenViewModelNotValid()
        {
            var viewModel = CreateModifyPublisherViewModel();
            _publisherController.ModelState.AddModelError(string.Empty, string.Empty);

            var result = _publisherController.UpdateAsync(viewModel).Result as ViewResult;
            var model = result.Model as ModifyPublisherViewModel;

            model.Should().BeEquivalentTo(viewModel);
        }

        [Test]
        public void UpdateAsync_ReturnsRedirect_WhenCreated()
        {
            var testViewModel = CreateModifyPublisherViewModel();

            var result = _publisherController.UpdateAsync(testViewModel).Result;

            result.Should().BeRedirectToActionResult();
        }

        [Test]
        public void Delete_ReturnsRedirect_WhenDeleted()
        {
            var result = _publisherController.DeleteAsync(Id).Result;

            result.Should().BeRedirectToActionResult();
        }


        private static ModifyPublisherViewModel CreateModifyPublisherViewModel()
        {
            var publisher = new ModifyPublisherViewModel
            {
                Id = "1",
                CompanyName = CompanyName,
                HomePage = "Home"
            };

            return publisher;
        }
    }
}