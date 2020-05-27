using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Common.Models;
using GameStore.Web.Factories.Interfaces;
using GameStore.Web.Models.ViewModels.OrderViewModels;

namespace GameStore.Web.Factories
{
    public class OrdersListViewModelFactory : IAsyncViewModelFactory<OrdersListViewModel, OrdersListViewModel>
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrdersListViewModelFactory(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        public async Task<OrdersListViewModel> CreateAsync(OrdersListViewModel ordersListViewModel)
        {
            var orderViewModels = GetDisplayOrderViewModelsAsync(ordersListViewModel.MinDate, ordersListViewModel.MaxDate);
            var viewModel = new OrdersListViewModel
            {
                MinDate = ordersListViewModel.MinDate,
                MaxDate = ordersListViewModel.MaxDate,
                DisplayOrderViewModels = await orderViewModels
            };

            return viewModel;
        }

        private async Task<IEnumerable<DisplayOrderViewModel>> GetDisplayOrderViewModelsAsync(DateTime min, DateTime max)
        {
            var orders = await _orderService.GetByDateRangeAsync(min, max, Culture.Current);
            var orderViewModels = _mapper.Map<IEnumerable<DisplayOrderViewModel>>(orders);

            return orderViewModels;
        }
    }
}