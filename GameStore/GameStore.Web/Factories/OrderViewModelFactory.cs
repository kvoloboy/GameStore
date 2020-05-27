using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GameStore.BusinessLayer.DTO;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Common.Models;
using GameStore.Core.Models;
using GameStore.Web.Factories.Interfaces;
using GameStore.Web.Models.ViewModels;
using GameStore.Web.Models.ViewModels.OrderViewModels;
using GameStore.Web.Models.ViewModels.PaymentViewModels;

namespace GameStore.Web.Factories
{
    public class OrderViewModelFactory : IAsyncViewModelFactory<OrderDto, OrderViewModel>
    {
        private readonly IPaymentTypeService _paymentService;
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrderViewModelFactory(
            IPaymentTypeService paymentService,
            IOrderService orderService,
            IMapper mapper)
        {
            _paymentService = paymentService;
            _orderService = orderService;
            _mapper = mapper;
        }

        public async Task<OrderViewModel> CreateAsync(OrderDto model)
        {
            var payments = await _paymentService.GetAllAsync();
            var paymentViewModels = _mapper.Map<IEnumerable<PaymentTypeViewModel>>(payments);
            var basketViewModel = CreateBasketViewModel(model);

            var orderViewModel = new OrderViewModel
            {
                Basket = basketViewModel,
                Payments = paymentViewModels,
            };

            return orderViewModel;
        }

        private BasketViewModel CreateBasketViewModel(OrderDto orderDto)
        {
            var orderDetailsViewModels = GetOrderDetailsViewModels(orderDto);

            var basketViewModel = new BasketViewModel
            {
                OrderId = orderDto.Id,
                OrderDetails = orderDetailsViewModels,
                TotalCost = _orderService.ComputeTotal(orderDto.Details),
                TotalItems = _orderService.ComputeProductsQuantity(orderDto.Details)
            };

            return basketViewModel;
        }

        private IEnumerable<OrderDetailsViewModel> GetOrderDetailsViewModels(OrderDto orderDto)
        {
            var orderDetailsViewModels = orderDto.Details.Select(od => _mapper.Map<OrderDetailsViewModel>(od));

            return orderDetailsViewModels;
        }
    }
}