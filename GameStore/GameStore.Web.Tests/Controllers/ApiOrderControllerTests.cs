using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using GameStore.BusinessLayer.DTO;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Web.Controllers;
using GameStore.Web.Factories.Interfaces;
using GameStore.Web.Models.ViewModels.OrderViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace GameStore.Web.Tests.Controllers
{
    [TestFixture]
    public class ApiOrderControllerTests
    {
        private const string Id = "1";

        private IBasketService _basketService;
        private IOrderService _orderService;
        private IAsyncViewModelFactory<string, DetailedOrderViewModel> _detailedOrderViewModelFactory;
        private IGameService _gameService;
        private ILogger<ApiOrderController> _logger;
        private IMapper _mapper;
        private ApiOrderController _apiOrderController;

        [SetUp]
        public void Setup()
        {
            _basketService = A.Fake<IBasketService>();
            _orderService = A.Fake<IOrderService>();
            _detailedOrderViewModelFactory = A.Fake<IAsyncViewModelFactory<string, DetailedOrderViewModel>>();
            _gameService = A.Fake<IGameService>();
            _logger = A.Fake<ILogger<ApiOrderController>>();
            _mapper = A.Fake<IMapper>();

            _apiOrderController = new ApiOrderController(
                _basketService,
                _orderService,
                _detailedOrderViewModelFactory,
                _gameService,
                _logger,
                _mapper);
        }

        [Test]
        public void GetByIdAsync_ReturnsViewModel_WhenFound()
        {
            var result = _apiOrderController.GetByIdAsync(Id).Result;

            result.Value.Should().NotBeNull();
        }

        [Test]
        public void CreateAsync_CallsService_Always()
        {
            _apiOrderController.CreateAsync(Id);

            A.CallTo(() => _basketService.AddAsync(A<string>._, A<string>._)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void CreateAsync_ReturnsCreatedOrder_WhenCreated()
        {
            var result = _apiOrderController.CreateAsync(Id).Result as CreatedAtActionResult;
            var model = result.Value as DetailedOrderViewModel;

            model.Should().NotBeNull();
        }

        [Test]
        public void UpdateAsync_CallsService_Always()
        {
            var viewModel = GetModifyOrderViewModel();

            _apiOrderController.UpdateAsync(viewModel);

            A.CallTo(() => _orderService.UpdateAsync(A<OrderDto>._)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void UpdateAsync_ReturnsNoContent_WhenUpdated()
        {
            var viewModel = GetModifyOrderViewModel();

            var result = _apiOrderController.UpdateAsync(viewModel).Result;

            result.Should().BeAssignableTo<NoContentResult>();
        }

        private static ModifyOrderViewModel GetModifyOrderViewModel()
        {
            var viewModel = new ModifyOrderViewModel();

            return viewModel;
        }
    }
}