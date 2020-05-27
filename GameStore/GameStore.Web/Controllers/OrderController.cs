using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GameStore.BusinessLayer.DTO;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Common.Models;
using GameStore.Core.Models;
using GameStore.Core.Models.Identity;
using GameStore.Identity.Attributes;
using GameStore.Identity.Extensions;
using GameStore.Web.Factories.Interfaces;
using GameStore.Web.Models.ViewModels.OrderViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;

namespace GameStore.Web.Controllers
{
    [Route("orders")]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IAsyncViewModelFactory<OrderDto, OrderViewModel> _orderViewModelFactory;
        private readonly IAsyncViewModelFactory<ShipmentViewModel, ShipmentViewModel> _shipmentViewModelFactory;
        private readonly IAsyncViewModelFactory<string, DetailedOrderViewModel> _detailedOrderViewModelFactory;
        private readonly IAsyncViewModelFactory<OrdersListViewModel, OrdersListViewModel> _ordersListViewModelFactory;
        private readonly IStringLocalizer<OrderController> _stringLocalizer;
        private readonly IMapper _mapper;

        public OrderController(
            IOrderService orderService,
            IAsyncViewModelFactory<OrderDto, OrderViewModel> orderViewModelFactory,
            IAsyncViewModelFactory<ShipmentViewModel, ShipmentViewModel> shipmentViewModelFactory,
            IAsyncViewModelFactory<string, DetailedOrderViewModel> detailedOrderViewModelFactory,
            IAsyncViewModelFactory<OrdersListViewModel, OrdersListViewModel> ordersListViewModelFactory,
            IStringLocalizer<OrderController> stringLocalizer,
            IMapper mapper)
        {
            _orderService = orderService;
            _orderViewModelFactory = orderViewModelFactory;
            _ordersListViewModelFactory = ordersListViewModelFactory;
            _stringLocalizer = stringLocalizer;
            _shipmentViewModelFactory = shipmentViewModelFactory;
            _detailedOrderViewModelFactory = detailedOrderViewModelFactory;
            _mapper = mapper;
        }

        [HttpGet("index")]
        [HasPermission(Permissions.MakeOrder)]
        public async Task<IActionResult> IndexAsync()
        {
            var orderViewModel = await CreateOrderViewModelAsync();

            return View("Index", orderViewModel);
        }

        [HttpGet]
        [HasPermission(Permissions.ReadOrders)]
        public async Task<IActionResult> ListAsync(OrdersListViewModel ordersListViewModel)
        {
            if (ordersListViewModel.MinDate == default)
            {
                var minDate = DateTime.UtcNow.AddDays(-Period.Month);
                ordersListViewModel.MinDate = minDate;
            }

            if (ordersListViewModel.MaxDate == default)
            {
                ordersListViewModel.MaxDate = DateTime.UtcNow;
            }

            var pulledViewModel = await _ordersListViewModelFactory.CreateAsync(ordersListViewModel);

            return View("List", pulledViewModel);
        }

        [HttpPost("complete")]
        [HasPermission(Permissions.MakeOrder)]
        public async Task<IActionResult> ConfirmOrderAsync(string orderId)
        {
            await _orderService.ConfirmAsync(orderId);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet("shipment")]
        [HasPermission(Permissions.MakeOrder)]
        public async Task<IActionResult> SetShipmentOptionsAsync(string orderId)
        {
            var filledViewModel = await CreateShipmentViewModelAsync(orderId);

            return View("ShipmentOptions", filledViewModel);
        }

        [HttpPost("shipment")]
        [HasPermission(Permissions.MakeOrder)]
        public async Task<IActionResult> SetShipmentOptionsAsync(ShipmentViewModel shipmentViewModel)
        {
            var shipmentDto = _mapper.Map<ShipmentDto>(shipmentViewModel);
            await _orderService.SetShipmentDetailsAsync(shipmentDto);

            return RedirectToAction(nameof(IndexAsync));
        }

        [HttpGet("update-shipments")]
        [HasPermission(Permissions.UpdateOrder)]
        public async Task<IActionResult> UpdateShipmentsAsync(string orderId)
        {
            var filledViewModel = await CreateShipmentViewModelAsync(orderId);

            return View("UpdateShipments", filledViewModel);
        }

        [HttpPost("update-shipments")]
        [HasPermission(Permissions.UpdateOrder)]
        public async Task<IActionResult> UpdateShipmentsAsync(ShipmentViewModel shipmentViewModel)
        {
            var shipmentDto = _mapper.Map<ShipmentDto>(shipmentViewModel);
            await _orderService.UpdateShipmentDetailsAsync(shipmentDto);

            return RedirectToAction(nameof(ListAsync));
        }

        [HttpGet("history")]
        [HasPermission(Permissions.ReadOrders)]
        public async Task<IActionResult> HistoryAsync(OrdersListViewModel ordersListViewModel)
        {
            var maxDate = DateTime.UtcNow.AddDays(-Period.Month);

            if (ordersListViewModel.MaxDate == default)
            {
                ordersListViewModel.MaxDate = maxDate;
            }

            var pulledViewModel = await _ordersListViewModelFactory.CreateAsync(ordersListViewModel);

            return View("History", pulledViewModel);
        }

        [HttpGet("details")]
        [HasPermission(Permissions.ReadOrders)]
        public async Task<IActionResult> DetailsAsync(string id)
        {
            var viewModel = await _detailedOrderViewModelFactory.CreateAsync(id);

            return View("Details", viewModel);
        }

        [HttpGet("update-state")]
        [HasPermission(Permissions.UpdateOrder)]
        public async Task<IActionResult> UpdateStateAsync(string orderId)
        {
            var order = await _orderService.GetByIdAsync(orderId);

            var currentState = Enum.GetName(typeof(OrderState), order.State);
            var localizedState = _stringLocalizer[currentState];

            var statesName = Enum.GetNames(typeof(OrderState));
            var localizedNames = statesName.Select(name => _stringLocalizer[name]);

            var viewModel = new UpdateStateViewModel
            {
                OrderId = orderId,
                State = localizedState,
                States = localizedNames.Select((name, i) => new SelectListItem
                {
                    Text = name,
                    Value = statesName[i],
                    Selected = name == currentState
                })
            };

            return View("UpdateState", viewModel);
        }

        [HttpPost("update-state")]
        [HasPermission(Permissions.UpdateOrder)]
        public async Task<IActionResult> UpdateStateAsync(UpdateStateViewModel viewModel)
        {
            var canParse = Enum.TryParse<OrderState>(viewModel.State, out var state);

            if (!canParse)
            {
                return BadRequest();
            }

            await _orderService.SetStateAsync(viewModel.OrderId, state);

            return RedirectToAction(nameof(DetailsAsync), new {id = viewModel.OrderId});
        }

        [HttpGet("personal")]
        [HasPermission(Permissions.ReadPersonalOrders)]
        public async Task<IActionResult> GetForUserAsync()
        {
            var userId = User?.GetId();
            var orders = await _orderService.GetByUserIdAsync(userId, Culture.Current);
            var orderViewModels = _mapper.Map<IEnumerable<DisplayOrderViewModel>>(orders);

            foreach (var viewModel in orderViewModels)
            {
                viewModel.CanBeChanged = CanBeCanceled(viewModel.State.Key);
            }

            return View("GetForUser", orderViewModels);
        }

        [HttpPost("cancel")]
        [HasPermission(Permissions.ReadPersonalOrders)]
        public async Task<IActionResult> CancelAsync(string orderId)
        {
            await _orderService.SetStateAsync(orderId, OrderState.Canceled);

            return RedirectToAction(nameof(IndexAsync));
        }

        private async Task<OrderViewModel> CreateOrderViewModelAsync()
        {
            var userId = User?.GetId();
            var orderDto =
                await _orderService.FindSingleAsync(o => o.State <= OrderState.Ordered && o.UserId == userId);
            var orderViewModel = await _orderViewModelFactory.CreateAsync(orderDto);

            return orderViewModel;
        }

        private async Task<ShipmentViewModel> CreateShipmentViewModelAsync(string orderId)
        {
            var shipmentDto = await _orderService.GetShipmentDetailsAsync(orderId);
            var shipmentViewModel = _mapper.Map<ShipmentViewModel>(shipmentDto);
            var filledViewModel = await _shipmentViewModelFactory.CreateAsync(shipmentViewModel);

            return filledViewModel;
        }

        private bool CanBeCanceled(string state)
        {
            var orderState = Enum.Parse<OrderState>(state, true);
            var canBeCanceled = _orderService.CanBeCanceled(orderState);

            return canBeCanceled;
        }
    }
}