using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Common.Models;
using GameStore.Core.Models;
using GameStore.Web.Controllers;
using GameStore.Web.Factories.Interfaces;
using GameStore.Web.Models.ViewModels.OrderViewModels;
using Microsoft.Extensions.Localization;

namespace GameStore.Web.Factories
{
    public class DetailedOrderViewModelFactory : IAsyncViewModelFactory<string, DetailedOrderViewModel>
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<OrderController> _stringLocalizer;

        public DetailedOrderViewModelFactory(
            IOrderService orderService,
            IMapper mapper,
            IStringLocalizer<OrderController> stringLocalizer)
        {
            _orderService = orderService;
            _mapper = mapper;
            _stringLocalizer = stringLocalizer;
        }

        public async Task<DetailedOrderViewModel> CreateAsync(string model)
        {
            var orderDto = await _orderService.GetByIdAsync(model, Culture.Current);
            var orderState = Enum.GetName(typeof(OrderState), orderDto.State);
            var localizedState = _stringLocalizer[orderState];

            var viewModel = new DetailedOrderViewModel
            {
                OrderId = orderDto.Id,
                ShipperName = orderDto.Shipper?.CompanyName ?? string.Empty,
                ShipmentViewModel = _mapper.Map<ShipmentViewModel>(orderDto.Shipment),
                TotalItems = _orderService.ComputeProductsQuantity(orderDto.Details).ToString(),
                TotalSum = _orderService.ComputeTotal(orderDto.Details).ToString(FormatTemplates.CurrencyFormat),
                OrderDetailsViewModels = _mapper.Map<IEnumerable<OrderDetailsViewModel>>(orderDto.Details),
                CanBeUpdated = _orderService.CanBeUpdated(orderDto.State, orderDto.OrderDate),
                State = localizedState
            };

            return viewModel;
        }
    }
}