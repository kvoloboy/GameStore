using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using FluentAssertions.AspNetCore.Mvc;
using GameStore.BusinessLayer.DTO;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Core.Models;
using GameStore.Web.Controllers;
using GameStore.Web.Factories.Interfaces;
using GameStore.Web.Models.ViewModels.OrderViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Localization;
using NUnit.Framework;

namespace GameStore.Web.Tests.Controllers
{
    [TestFixture]
    public class OrderControllerTests
    {
        private const string Id = "1";

        private IOrderService _orderService;
        private IAsyncViewModelFactory<OrderDto, OrderViewModel> _orderViewModelFactory;
        private IAsyncViewModelFactory<OrdersListViewModel, OrdersListViewModel> _historyViewModelFactory;
        private IAsyncViewModelFactory<ShipmentViewModel, ShipmentViewModel> _shipmentViewModelFactory;
        private IAsyncViewModelFactory<string, DetailedOrderViewModel> _detailedOrderViewModelFactory;
        private IStringLocalizer<OrderController> _stringLocalizer;
        private IMapper _mapper;

        private OrderController _orderController;

        [SetUp]
        public void Setup()
        {
            _orderService = A.Fake<IOrderService>();
            _orderViewModelFactory = A.Fake<IAsyncViewModelFactory<OrderDto, OrderViewModel>>();
            _historyViewModelFactory = A.Fake<IAsyncViewModelFactory<OrdersListViewModel, OrdersListViewModel>>();
            _shipmentViewModelFactory = A.Fake<IAsyncViewModelFactory<ShipmentViewModel, ShipmentViewModel>>();
            _detailedOrderViewModelFactory = A.Fake<IAsyncViewModelFactory<string, DetailedOrderViewModel>>();
            _stringLocalizer = A.Fake<IStringLocalizer<OrderController>>();
            _mapper = A.Fake<IMapper>();

            _orderController = new OrderController(
                _orderService,
                _orderViewModelFactory,
                _shipmentViewModelFactory,
                _detailedOrderViewModelFactory,
                _historyViewModelFactory,
                _stringLocalizer,
                _mapper);
        }

        [Test]
        public void IndexAsync_ReturnsView_Always()
        {
            var result = _orderController.IndexAsync().Result;

            result.Should().BeViewResult();
        }

        [Test]
        public void ListAsync_ReturnsViewWithOrderListViewModel_Always()
        {
            var viewModel = new OrdersListViewModel();

            var result = _orderController.ListAsync(viewModel).Result as ViewResult;
            var model = result.Model;

            model.Should().BeAssignableTo<OrdersListViewModel>();
        }

        [Test]
        public void SetShipmentOptionsAsync_ReturnsView_Always()
        {
            var result = _orderController.SetShipmentOptionsAsync(Id).Result as ViewResult;
            var model = result.Model;

            model.Should().NotBeNull();
        }

        [Test]
        public void SetShipmentOptionsAsync_SavesShipmentDetails_Always()
        {
            _orderController.SetShipmentOptionsAsync(new ShipmentViewModel());

            A.CallTo(() => _orderService.SetShipmentDetailsAsync(A<ShipmentDto>._)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void SetShipmentOptionsAsync_ReturnsRedirect_WhenSavedOptions()
        {
            var result = _orderController.SetShipmentOptionsAsync(new ShipmentViewModel()).Result;

            result.Should().BeRedirectToActionResult();
        }

        [Test]
        public void UpdateShipmentsAsync_ReturnsView_Always()
        {
            var result = _orderController.UpdateShipmentsAsync(Id).Result;

            result.Should().BeViewResult();
        }

        [Test]
        public void UpdateShipmentsAsync_CallsService_Always()
        {
            var viewModel = new ShipmentViewModel();

            _orderController.UpdateShipmentsAsync(viewModel);

            A.CallTo(() => _orderService.UpdateShipmentDetailsAsync(A<ShipmentDto>._)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void ConfirmOrderAsync_SetsOrderedState_WhenFound()
        {
            _orderController.ConfirmOrderAsync(Id);

            A.CallTo(() => _orderService.ConfirmAsync(Id)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void ConfirmOrderAsync_ReturnsRedirect_WhenConfirm()
        {
            var result = _orderController.ConfirmOrderAsync(Id).Result;

            result.Should().BeRedirectToActionResult();
        }

        [Test]
        public void HistoryAsync_ReturnsView_Always()
        {
            var viewModel = new OrdersListViewModel();
            var result = _orderController.HistoryAsync(viewModel).Result as ViewResult;
            var model = result.Model as OrdersListViewModel;

            model.Should().NotBeNull();
        }

        [Test]
        public void HistoryAsync_ReturnsViewWithFilteredOrders_Always()
        {
            var minDate = new DateTime(2020, 1, 9);
            var maxDate = new DateTime(2020, 1, 10);
            var testViewModel = new OrdersListViewModel
            {
                MinDate = minDate,
                MaxDate = maxDate
            };
            A.CallTo(() => _historyViewModelFactory.CreateAsync(testViewModel)).Returns(testViewModel);

            var result = _orderController.HistoryAsync(testViewModel).Result as ViewResult;
            var model = result.Model as OrdersListViewModel;

            model.MinDate.Should().Be(minDate);
            model.MaxDate.Should().Be(maxDate);
        }

        [Test]
        public void DetailsAsync_ReturnsView_Always()
        {
            var result = _orderController.DetailsAsync(Id).Result;

            result.Should().BeViewResult();
        }

        [Test]
        public void UpdateStateAsync_ReturnsViewWithAssignedStates_Always()
        {
            var result = _orderController.UpdateStateAsync(Id).Result as ViewResult;
            var model = result.Model as UpdateStateViewModel;

            model.States.Should().NotBeEmpty();
        }

        [Test]
        public void UpdateStateAsync_ReturnsBadRequest_WhenOrderStateCannotBeParsed()
        {
            var viewModel = CreateUpdateStateViewModel();

            var result = _orderController.UpdateStateAsync(viewModel).Result;

            result.Should().BeAssignableTo<BadRequestResult>();
        }

        [Test]
        public void UpdateStateAsync_ReturnsRedirect_WhenOrderStateIsUpdated()
        {
            var viewModel = CreateUpdateStateViewModel(OrderState.Closed.ToString());

            var result = _orderController.UpdateStateAsync(viewModel).Result;

            result.Should().BeRedirectToActionResult();
        }

        [Test]
        public void GetForUserAsync_ReturnsOrderCollection_WhenExists()
        {
            var viewModels = CreateDisplayOrderViewModels();
            A.CallTo(() => _mapper.Map<IEnumerable<DisplayOrderViewModel>>(A<IEnumerable<OrderDto>>._))
                .Returns(viewModels);
            A.CallTo(() => _orderService.CanBeCanceled(A<OrderState>._)).Returns(true);

            var result = _orderController.GetForUserAsync().Result as ViewResult;
            var model = (result.Model as IEnumerable<DisplayOrderViewModel>).ToArray();
            
            model[0].CanBeChanged.Should().BeTrue();
        }

        [Test]
        public void CancelAsync_SetsNewOrderState_WhenFound()
        {
            const OrderState state = OrderState.Canceled;

            _orderController.CancelAsync(Id);

            A.CallTo(() => _orderService.SetStateAsync(Id, state)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void CancelAsync_ReturnsRedirect_WhenChangedState()
        {
            var result = _orderController.CancelAsync(Id).Result;

            result.Should().BeRedirectToActionResult();
        }


        private static UpdateStateViewModel CreateUpdateStateViewModel(string state = "")
        {
            var viewModel = new UpdateStateViewModel
            {
                State = state
            };

            return viewModel;
        }

        private static IEnumerable<DisplayOrderViewModel> CreateDisplayOrderViewModels()
        {
            var orderState = OrderState.Closed.ToString();
            var viewModels = new[]
            {
                new DisplayOrderViewModel
                {
                    State = new KeyValuePair<string, string>(orderState, orderState)
                }
            };

            return viewModels;
        }
    }
}