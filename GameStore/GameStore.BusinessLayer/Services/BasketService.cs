using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using GameStore.BusinessLayer.DTO;
using GameStore.BusinessLayer.Exceptions;
using GameStore.BusinessLayer.Models;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Common.Models;
using GameStore.Core.Models;

namespace GameStore.BusinessLayer.Services
{
    public class BasketService : IBasketService
    {
        private readonly IGameService _gameService;
        private readonly IOrderService _orderService;
        private readonly IOrderDetailsService _orderDetailsService;
        private readonly IMapper _mapper;

        public BasketService(
            IGameService gameService,
            IOrderService orderService,
            IOrderDetailsService orderDetailsService,
            IMapper mapper)
        {
            _gameService = gameService;
            _orderService = orderService;
            _orderDetailsService = orderDetailsService;
            _mapper = mapper;
        }

        public async Task<Basket> GetBasketForUserAsync(string userId, string culture = Culture.En)
        {
            var order = await _orderService.GetNewOrderByUserIdAsync(userId, culture);
            var total = _orderService.ComputeTotal(order.Details);
            var productCount = _orderService.ComputeProductsQuantity(order.Details);
            var basket = new Basket
            {
                OrderId = order.Id,
                OrderDetails = order.Details,
                TotalCost = total,
                TotalItems = productCount
            };

            return basket;
        }

        public async Task<string> AddAsync(string gameKey, string customerId)
        {
            if (string.IsNullOrEmpty(gameKey))
            {
                throw new InvalidServiceOperationException("Is empty game key");
            }

            var existingDetails = await GetDetailsByGameKeyAndCustomerIdAsync(gameKey, customerId);
            var orderId = existingDetails?.OrderId;

            if (existingDetails == null)
            {
                orderId = (await _orderService.GetNewOrderByUserIdAsync(customerId)).Id;
                await CreateDetailsAsync(gameKey, orderId);
            }
            else
            {
                await _orderService.SetNewStateWhenOrderedAsync(orderId);
                await IncrementDetailsQuantityAsync(existingDetails);
            }

            return orderId;
        }

        public async Task UpdateQuantityAsync(string detailsId, short quantity)
        {
            var detailsDto = await _orderDetailsService.GetByIdAsync(detailsId);
            detailsDto.Quantity = quantity;
            await _orderDetailsService.UpdateAsync(detailsDto);
        }

        public async Task DeleteAsync(string detailsId)
        {
            await _orderDetailsService.DeleteAsync(detailsId);
        }

        private async Task CreateDetailsAsync(string gameKey, string orderId)
        {
            var gameDto = await _gameService.GetByKeyAsync(gameKey);
            var detailsDto = new OrderDetailsDto
            {
                GameId = gameDto.Id,
                Price = gameDto.Price,
                Discount = gameDto.Discount,
                OrderId = orderId
            };
            await _orderDetailsService.CreateAsync(detailsDto);
        }

        private async Task IncrementDetailsQuantityAsync(OrderDetails details)
        {
            details.Quantity++;
            var orderDetailsDto = _mapper.Map<OrderDetailsDto>(details);
            await _orderDetailsService.UpdateAsync(orderDetailsDto);
        }

        private async Task<OrderDetails> GetDetailsByGameKeyAndCustomerIdAsync(string gameKey, string customerId)
        {
            Expression<Func<OrderDetails, bool>> existingNewOrderFilter = od =>
                od.GameRoot.Key == gameKey && od.Order.UserId == customerId && od.Order.State <= OrderState.Ordered;
            var existingDetails = await _orderDetailsService.FindSingleAsync(existingNewOrderFilter);

            return existingDetails;
        }
    }
}