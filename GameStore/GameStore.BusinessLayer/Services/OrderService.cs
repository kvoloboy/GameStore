using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using GameStore.BusinessLayer.DTO;
using GameStore.BusinessLayer.Exceptions;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.BusinessLayer.Services.Notification.Interfaces;
using GameStore.Common.Decorators.Interfaces;
using GameStore.Common.Models;
using GameStore.Core.Abstractions;
using GameStore.Core.Models;
using GameStore.Infrastructure.Extensions;
using Microsoft.Extensions.Logging;

namespace GameStore.BusinessLayer.Services
{
    public class OrderService : IOrderService
    {
        private const int MaxDiscountValue = 100;

        private readonly IUnitOfWork _unitOfWork;
        private readonly IGameDecorator _gameDecorator;
        private readonly IGameService _gameService;
        private readonly INotificationService<Order> _notificationService;
        private readonly IAsyncRepository<Order> _orderDecorator;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderService> _logger;

        public OrderService(
            IUnitOfWork unitOfWork,
            IGameService gameService,
            INotificationService<Order> notificationService,
            ILogger<OrderService> logger,
            IGameDecorator gameDecorator,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _gameService = gameService;
            _notificationService = notificationService;
            _logger = logger;
            _gameDecorator = gameDecorator;
            _mapper = mapper;
            _orderDecorator = _unitOfWork.GetRepository<IAsyncRepository<Order>>();
        }

        public async Task SetShipmentDetailsAsync(ShipmentDto shipmentDto)
        {
            const bool isUpdate = false;
            await SetShipmentDetailsAsync(shipmentDto, isUpdate);
        }

        public async Task UpdateShipmentDetailsAsync(ShipmentDto shipmentDto)
        {
            const bool isUpdate = true;
            await SetShipmentDetailsAsync(shipmentDto, isUpdate);
        }

        public async Task<OrderDto> GetByIdAsync(string id, string culture = "en-US")
        {
            var order = await _orderDecorator.FindSingleAsync(o => o.Id == id);

            if (order == null)
            {
                throw new EntityNotFoundException<Order>(id);
            }

            var orderDto = await GetOrderDtoAsync(order, culture, true);

            return orderDto;
        }

        public async Task<ShipmentDto> GetShipmentDetailsAsync(string orderId)
        {
            var order = await GetEntityByIdAsync(orderId);
            var shipmentDto = _mapper.Map<ShipmentDto>(order);

            return shipmentDto;
        }

        public async Task<IEnumerable<OrderDto>> GetByUserIdAsync(string userId, string culture = "en-US")
        {
            Expression<Func<Order, bool>> predicate = o => o.UserId == userId && o.State != OrderState.New;
            var orders = await FindAllAsync(predicate);
            var ordersDto = new List<OrderDto>();

            foreach (var order in orders)
            {
                var orderDto = await GetOrderDtoAsync(order, culture);
                ordersDto.Add(orderDto);
            }

            return ordersDto;
        }

        public async Task<IEnumerable<OrderDto>> GetByDateRangeAsync(
            DateTime minDate, 
            DateTime maxDate,
            string culture = Culture.En)
        {
            Expression<Func<Order, bool>> predicate = o => o.OrderDate >= minDate && o.OrderDate <= maxDate;
            var isNotAssigned = minDate == default && maxDate == default;
            var orders = isNotAssigned ? await FindAllAsync() : await FindAllAsync(predicate);
            var ordersDto = new List<OrderDto>();

            foreach (var order in orders)
            {
                var orderDto = await GetOrderDtoAsync(order, culture);
                ordersDto.Add(orderDto);
            }

            return ordersDto;
        }

        public async Task<OrderDto> FindSingleAsync(Expression<Func<Order, bool>> predicate, string culture = "en-US")
        {
            var order = await _orderDecorator.FindSingleAsync(predicate);

            if (order == null)
            {
                throw new EntityNotFoundException<Order>();
            }

            var orderDto = await GetOrderDtoAsync(order, culture, true);

            return orderDto;
        }

        public async Task<OrderDto> GetNewOrderByUserIdAsync(string userId, string culture = Culture.En)
        {
            var order = await _orderDecorator.FindSingleAsync(o => o.UserId == userId && o.State <= OrderState.Ordered);

            if (order == null)
            {
                var createdOrder = await CreateEmptyOrderAndSaveAsync(userId);
                var createdOrderDto = await GetOrderDtoAsync(createdOrder, culture, true);

                return createdOrderDto;
            }

            if (order.State != OrderState.New)
            {
                order.State = OrderState.New;
                var orderToUpdate = order.Clone();
                await _orderDecorator.UpdateAsync(orderToUpdate);
                await _unitOfWork.CommitAsync();
            }

            var orderDto = await GetOrderDtoAsync(order, culture, true);

            return orderDto;
        }

        public async Task SetNewStateWhenOrderedAsync(string orderId)
        {
            var order = await _orderDecorator.FindSingleAsync(o => o.Id == orderId);
            if (order.State != OrderState.Ordered)
            {
                return;
            }

            order.State = OrderState.New;
            await _orderDecorator.UpdateAsync(order);
        }

        public async Task ConfirmAsync(string orderId)
        {
            var order = await GetEntityByIdAsync(orderId);

            if (order.State > OrderState.Ordered)
            {
                throw new InvalidServiceOperationException($"Not found unconfirmed order with id: {orderId}");
            }

            await SubtractGamesQuantityAsync(order.Details);
            order.State = OrderState.Closed;

            await _orderDecorator.UpdateAsync(order);
            await _unitOfWork.CommitAsync();

            await _notificationService.NotifyAsync(order);
            _logger.LogDebug($"Confirm order with id: {orderId}.");
        }

        public async Task MergeOrdersAsync(string oldUserId, string newUserId)
        {
            var oldOrder = await _orderDecorator.FindSingleAsync(o => o.UserId == oldUserId);
            var currentOrder = await GetNewOrderByUserIdAsync(newUserId);
            var orderDetailsRepository = _unitOfWork.GetRepository<IAsyncRepository<OrderDetails>>();

            if (oldOrder == null)
            {
                return;
            }

            foreach (var orderDetails in oldOrder.Details)
            {
                orderDetails.OrderId = currentOrder.Id;
                await orderDetailsRepository.UpdateAsync(orderDetails);
            }

            await _orderDecorator.DeleteAsync(oldOrder.Id);
        }

        public async Task SetStateAsync(string orderId, OrderState state)
        {
            var order = await GetEntityByIdAsync(orderId);
            var stateName = ParseOrderState(state);
            order.State = state;
            await _orderDecorator.UpdateAsync(order);
            await _unitOfWork.CommitAsync();
            _logger.LogDebug($"Change order state. Order id: {orderId}. State: {stateName}");
        }

        public bool CanBeCanceled(OrderState orderState)
        {
            var canBeCanceled = orderState > OrderState.Ordered || orderState == OrderState.Pending;

            return canBeCanceled;
        }

        public bool CanBeUpdated(OrderState orderState, DateTime orderDate)
        {
            var isValidState = orderState > OrderState.New && orderState < OrderState.Closed;
            var isValidDate = DateTime.UtcNow.AddDays(-Period.Month) < orderDate;

            return isValidState && isValidDate;
        }

        public async Task UpdateAsync(OrderDto orderDto)
        {
            var exist = await _orderDecorator.AnyAsync(o => o.Id == orderDto.Id);

            if (!exist)
            {
                throw new EntityNotFoundException<Order>(orderDto.Id);
            }

            var order = _mapper.Map<Order>(orderDto);
            _mapper.Map(orderDto.Shipment, order);
            await _orderDecorator.UpdateAsync(order);
            await _unitOfWork.CommitAsync();
        }

        public decimal ComputeTotal(IEnumerable<OrderDetailsDto> details)
        {
            if (details == null)
            {
                throw new InvalidServiceOperationException("Are null details");
            }

            var sum = details.Sum(od => (od.Price - od.Price * od.Discount / MaxDiscountValue) * od.Quantity);

            return sum;
        }

        public int ComputeProductsQuantity(IEnumerable<OrderDetailsDto> details)
        {
            if (details == null)
            {
                throw new InvalidServiceOperationException("Are null details");
            }

            var productsCount = details.Sum(od => od.Quantity);

            return productsCount;
        }

        private void SetOrderedState(Order order)
        {
            foreach (var orderDetails in order.Details)
            {
                ValidateUnitsInStockQuantity(orderDetails);
            }

            order.State = OrderState.Ordered;
            order.OrderDate = DateTime.UtcNow;
            _logger.LogDebug($"Set ordered state to order with id: {order.UserId}.");
        }

        private async Task<IEnumerable<Order>> FindAllAsync(Expression<Func<Order, bool>> predicate = null)
        {
            var orders = await _orderDecorator.FindAllAsync(predicate);

            return orders;
        }

        private async Task<Order> CreateEmptyOrderAndSaveAsync(string userId)
        {
            var order = new Order
            {
                UserId = userId,
                Details = new List<OrderDetails>(),
                State = OrderState.New
            };
            await _orderDecorator.AddAsync(order);
            await _unitOfWork.CommitAsync();

            return order;
        }

        private static string ParseOrderState(OrderState state)
        {
            var stateName = Enum.GetName(typeof(OrderState), state);

            return stateName;
        }

        private async Task SubtractGamesQuantityAsync(IEnumerable<OrderDetails> details)
        {
            var countableDetails = details.Where(od => od.GameRoot.Details.UnitsInStock != null);

            foreach (var orderDetails in countableDetails)
            {
                ValidateUnitsInStockQuantity(orderDetails);
                var unitsInShock = orderDetails.GameRoot.Details.UnitsInStock ?? 0;
                var newUnitsInStock = (short) (unitsInShock - orderDetails.Quantity);
                var key = orderDetails.GameRoot.Key;
                await _gameDecorator.UpdateUnitsInStockAsync(key, newUnitsInStock);
            }

            await _unitOfWork.CommitAsync();
        }

        private static void ValidateUnitsInStockQuantity(OrderDetails details)
        {
            var gamesInStock = details.GameRoot.Details.UnitsInStock;

            if (details.Quantity > gamesInStock)
            {
                throw new InvalidServiceOperationException($"Available only {gamesInStock} units in stock");
            }
        }

        private async Task SetShipmentDetailsAsync(ShipmentDto shipmentDto, bool isUpdate)
        {
            if (shipmentDto == null)
            {
                throw new InvalidServiceOperationException("Is null dto");
            }

            if (string.IsNullOrEmpty(shipmentDto.OrderId))
            {
                throw new InvalidServiceOperationException("Is null order id");
            }

            var order = await GetEntityByIdAsync(shipmentDto.OrderId);
            _mapper.Map(shipmentDto, order);

            if (!isUpdate)
            {
                SetOrderedState(order);
            }

            await _orderDecorator.UpdateAsync(order);
            await _unitOfWork.CommitAsync();
        }

        private async Task<OrderDto> GetOrderDtoAsync(Order order, string culture, bool includeGames = false)
        {
            var orderDto = _mapper.Map<OrderDto>(order);
            var shipmentDto = _mapper.Map<ShipmentDto>(order);
            orderDto.Shipment = shipmentDto;

            foreach (var orderDetails in order.Details)
            {
                var detailsDto = _mapper.Map<OrderDetailsDto>(orderDetails);

                if (includeGames)
                {
                    detailsDto.Game = await _gameService.GetGameDtoAsync(orderDetails.GameRoot, culture);
                }

                orderDto.Details.Add(detailsDto);
            }

            return orderDto;
        }

        private async Task<Order> GetEntityByIdAsync(string id)
        {
            var order = await _orderDecorator.FindSingleAsync(o => o.Id == id);

            if (order == null)
            {
                throw new EntityNotFoundException<Order>(id);
            }

            return order;
        }
    }
}