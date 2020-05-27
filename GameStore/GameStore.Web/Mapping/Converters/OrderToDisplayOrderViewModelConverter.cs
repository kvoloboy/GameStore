using System;
using System.Collections.Generic;
using AutoMapper;
using GameStore.BusinessLayer.DTO;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Core.Models;
using GameStore.Web.Controllers;
using GameStore.Web.Models;
using GameStore.Web.Models.ViewModels.OrderViewModels;
using Microsoft.Extensions.Localization;

namespace GameStore.Web.Mapping.Converters
{
    public class OrderDtoToDisplayOrderViewModelConverter : ITypeConverter<OrderDto, DisplayOrderViewModel>
    {
        private readonly IStringLocalizer<OrderController> _stringLocalizer;
        private readonly IOrderService _orderService;

        public OrderDtoToDisplayOrderViewModelConverter(
            IOrderService orderService,
            IStringLocalizer<OrderController> stringLocalizer)
        {
            _orderService = orderService;
            _stringLocalizer = stringLocalizer;
        }

        public DisplayOrderViewModel Convert(
            OrderDto source,
            DisplayOrderViewModel destination,
            ResolutionContext context)
        {
            var orderState = Enum.GetName(typeof(OrderState), source.State);
            var localizedState = _stringLocalizer[orderState];

            var displayOrderViewModel = new DisplayOrderViewModel
            {
                Id = source.Id,
                OrderDate = source.OrderDate,
                UserId = source.UserId,
                Total = _orderService.ComputeTotal(source.Details),
                ProductsQuantity = _orderService.ComputeProductsQuantity(source.Details),
                State = new KeyValuePair<string, string>(orderState, localizedState),
                ShipperName = source.Shipper?.CompanyName ?? string.Empty
            };

            return displayOrderViewModel;
        }
    }
}