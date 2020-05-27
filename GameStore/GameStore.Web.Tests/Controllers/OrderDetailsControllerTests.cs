using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using FluentAssertions.AspNetCore.Mvc;
using GameStore.BusinessLayer.DTO;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Web.Controllers;
using GameStore.Web.Factories.Interfaces;
using GameStore.Web.Models.ViewModels.OrderViewModels;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace GameStore.Web.Tests.Controllers
{
    [TestFixture]
    public class OrderDetailsControllerTests
    {
        private const string Id = "Id";

        private IOrderDetailsService _orderDetailsService;
        private IAsyncViewModelFactory<ModifyOrderDetailsViewModel, ModifyOrderDetailsViewModel>
            _modifyOrderDetailsViewModelFactory;
        private IMapper _mapper;
        private OrderDetailsController _orderDetailsController;

        [SetUp]
        public void Setup()
        {
            _orderDetailsService = A.Fake<IOrderDetailsService>();
            _modifyOrderDetailsViewModelFactory =
                A.Fake<IAsyncViewModelFactory<ModifyOrderDetailsViewModel, ModifyOrderDetailsViewModel>>();
            _mapper = A.Fake<IMapper>();

            _orderDetailsController = new OrderDetailsController(
                _orderDetailsService,
                _modifyOrderDetailsViewModelFactory,
                _mapper);
        }

        [Test]
        public void CreateAsync_ReturnsView_Always()
        {
            var result = _orderDetailsController.CreateAsync(Id).Result as ViewResult;
            var model = result.Model;

            model.Should().BeAssignableTo<ModifyOrderDetailsViewModel>();
        }

        [Test]
        public void CreateAsync_ReturnsView_WhenModelStateIsInvalid()
        {
            var viewModel = CreateModifyOrderDetailsViewModel();
            _orderDetailsController.ModelState.AddModelError(string.Empty, string.Empty);

            var result = _orderDetailsController.CreateAsync(viewModel).Result;

            result.Should().BeViewResult();
        }

        [Test]
        public void CreateAsync_CallsService_WhenValidState()
        {
            var viewModel = CreateModifyOrderDetailsViewModel();

            _orderDetailsController.CreateAsync(viewModel);

            A.CallTo(() => _orderDetailsService.CreateAsync(A<OrderDetailsDto>._)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void UpdateAsync_ReturnsView_Always()
        {
            var result = _orderDetailsController.UpdateAsync(Id).Result as ViewResult;
            var model = result.Model;

            model.Should().BeAssignableTo<ModifyOrderDetailsViewModel>();
        }

        [Test]
        public void UpdateAsync_ReturnsView_WhenModelStateIsInvalid()
        {
            var viewModel = CreateModifyOrderDetailsViewModel();
            _orderDetailsController.ModelState.AddModelError(string.Empty, string.Empty);

            var result = _orderDetailsController.UpdateAsync(viewModel).Result;

            result.Should().BeViewResult();
        }

        [Test]
        public void UpdateAsync_ReturnsRedirect_WhenModelIsUpdated()
        {
            var viewModel = CreateModifyOrderDetailsViewModel();

            var result = _orderDetailsController.UpdateAsync(viewModel).Result;

            result.Should().BeRedirectToActionResult();
        }

        [Test]
        public void Delete_CallsService_Always()
        {
            _orderDetailsController.DeleteAsync(Id);

            A.CallTo(() => _orderDetailsService.DeleteAsync(Id)).MustHaveHappenedOnceExactly();
        }

        private static ModifyOrderDetailsViewModel CreateModifyOrderDetailsViewModel()
        {
            var viewModel = new ModifyOrderDetailsViewModel();

            return viewModel;
        }
    }
}