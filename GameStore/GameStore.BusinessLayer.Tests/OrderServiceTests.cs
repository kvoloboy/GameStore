using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using GameStore.BusinessLayer.DTO;
using GameStore.BusinessLayer.Exceptions;
using GameStore.BusinessLayer.Services;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.BusinessLayer.Services.Notification.Interfaces;
using GameStore.Common.Decorators.Interfaces;
using GameStore.Core.Abstractions;
using GameStore.Core.Models;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace GameStore.BusinessLayer.Tests
{
    [TestFixture]
    public class OrderServiceTests
    {
        private const string Id = "1";
        private const string UserId = "2";

        private IUnitOfWork _unitOfWork;
        private IGameService _gameService;
        private INotificationService<Order> _notificationService;
        private ILogger<OrderService> _logger;
        private IAsyncRepository<Order> _orderRepository;
        private IGameDecorator _gameDecorator;
        private IMapper _mapper;
        private IAsyncRepository<OrderDetails> _orderDetailsDecorator;
        private OrderService _orderService;

        [SetUp]
        public void Setup()
        {
            _unitOfWork = A.Fake<IUnitOfWork>();
            _gameService = A.Fake<IGameService>();
            _notificationService = A.Fake<INotificationService<Order>>();
            _orderRepository = A.Fake<IAsyncRepository<Order>>();
            _mapper = A.Fake<IMapper>();
            _gameDecorator = A.Fake<IGameDecorator>();
            _logger = A.Fake<ILogger<OrderService>>();
            _orderDetailsDecorator = A.Fake<IAsyncRepository<OrderDetails>>();
            A.CallTo(() => _unitOfWork.GetRepository<IAsyncRepository<Order>>()).Returns(_orderRepository);
            A.CallTo(() => _unitOfWork.GetRepository<IAsyncRepository<OrderDetails>>()).Returns(_orderDetailsDecorator);

            _orderService = new OrderService(
                _unitOfWork,
                _gameService,
                _notificationService,
                _logger,
                _gameDecorator,
                _mapper);
        }

        [Test]
        public async Task SetShipmentDetailsAsync_ThrowsException_WhenNullDto()
        {
            Func<Task> action = async () => await _orderService.SetShipmentDetailsAsync(null);

            await action.Should().ThrowAsync<InvalidServiceOperationException>();
        }

        [Test]
        public async Task SetShipmentDetailsAsync_ThrowsException_WhenOrderDetailsQuantityIsGreaterThanUnitsInStock()
        {
            var testGame = CreateGameRoot();
            var testOrder = CreateTestOrder(testGame);
            A.CallTo(() => _orderRepository.FindSingleAsync(A<Expression<Func<Order, bool>>>._)).Returns(testOrder);

            Func<Task> action = async () => await _orderService.SetShipmentDetailsAsync(new ShipmentDto());

            await action.Should().ThrowAsync<InvalidServiceOperationException>();
        }

        [Test]
        public async Task SetShipmentDetailsAsync_ThrowsException_WhenOrderIdIsNull()
        {
            Func<Task> action = async () => await _orderService.SetShipmentDetailsAsync(new ShipmentDto());

            await action.Should().ThrowAsync<InvalidServiceOperationException>().WithMessage("Is null order id");
        }

        [Test]
        public void SetShipmentDetailsAsync_UpdatesOrder_WhenValidShipmentDetailsAndOrderDetails()
        {
            var gameRoot = CreateGameRoot(3);
            var order = CreateTestOrder(gameRoot);
            Expression<Func<Order, bool>> matchPredicate = o => o.State == OrderState.Ordered;
            A.CallTo(() => _orderRepository.FindSingleAsync(A<Expression<Func<Order, bool>>>._)).Returns(order);

            _orderService.SetShipmentDetailsAsync(new ShipmentDto {OrderId = Id});

            A.CallTo(() => _orderRepository.UpdateAsync(A<Order>.That.Matches(matchPredicate)))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task GetByIdAsync_ThrowsException_WhenNotFound()
        {
            A.CallTo(() => _orderRepository.FindSingleAsync(A<Expression<Func<Order, bool>>>._)).Returns((Order) null);

            Func<Task> action = async () => await _orderService.GetByIdAsync(Id);

            await action.Should().ThrowAsync<EntityNotFoundException<Order>>();
        }

        [Test]
        public void GetByIdAsync_ReturnsOrder_WhenFound()
        {
            var order = CreateTestOrder(null);
            A.CallTo(() => _orderRepository.FindSingleAsync(A<Expression<Func<Order, bool>>>._)).Returns(order);

            var orderDto = _orderService.GetByIdAsync(Id);

            orderDto.Should().NotBeNull();
        }

        [Test]
        public void GetByUserIdAsync_CallsRepository_Always()
        {
            _orderService.GetByUserIdAsync(Id);

            A.CallTo(() => _orderRepository.FindAllAsync(A<Expression<Func<Order, bool>>>._))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public void GetByDateRangeAsync_CallsRepositoryWithExpression_WhenDatesAreAssigned()
        {
            var minDate = new DateTime(2020, 11, 9);
            var maxDate = new DateTime(2020, 11, 10);

            _orderService.GetByDateRangeAsync(minDate, maxDate);

            A.CallTo(() => _orderRepository.FindAllAsync(A<Expression<Func<Order, bool>>>._))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public void GetByDateRangeAsync_CallsRepositoryWithoutExpression_WhenDatesAreNotAssigned()
        {
            _orderService.GetByDateRangeAsync(default, default);

            A.CallTo(() => _orderRepository.FindAllAsync(null)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task FindSingleAsync_ThrowsException_WhenNotFound()
        {
            Expression<Func<Order, bool>> testExpression = o => true;
            A.CallTo(() => _orderRepository.FindSingleAsync(A<Expression<Func<Order, bool>>>._)).Returns((Order) null);

            Func<Task> action = async () => await _orderService.FindSingleAsync(testExpression);

            await action.Should().ThrowAsync<EntityNotFoundException<Order>>();
        }

        [Test]
        public void FindSingleAsync_ReturnsOrder_WhenFound()
        {
            Expression<Func<Order, bool>> testExpression = o => true;
            var order = CreateTestOrder(null);
            A.CallTo(() => _orderRepository.FindSingleAsync(A<Expression<Func<Order, bool>>>._)).Returns(order);

            var orderDto = _orderService.FindSingleAsync(testExpression);

            orderDto.Should().NotBeNull();
        }

        [Test]
        public void GetNewOrderByUserIdAsync_CreatesNewOrder_WhenNotExists()
        {
            A.CallTo(() => _orderRepository.FindSingleAsync(A<Expression<Func<Order, bool>>>._))
                .Returns((Order) null);

            _orderService.GetNewOrderByUserIdAsync(Id);

            A.CallTo(() => _orderRepository.AddAsync(A<Order>._)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void GetNewOrderByUserIdAsync_SetsNewState_WhenOrdered()
        {
            Expression<Func<Order, bool>> matchPredicate = order => order.State == OrderState.New;
            var testOrder = CreateTestOrder(new GameRoot(), OrderState.Ordered);
            A.CallTo(() => _orderRepository.FindSingleAsync(A<Expression<Func<Order, bool>>>._)).Returns(testOrder);

            _orderService.GetNewOrderByUserIdAsync(Id);

            A.CallTo(() => _orderRepository.UpdateAsync(A<Order>.That.Matches(matchPredicate)))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public void SetNewStateWhenOrderedAsync_CallsRepositoryNever_WhenOrderStateIsNotThenOrdered()
        {
            _orderService.SetNewStateWhenOrderedAsync(Id);

            A.CallTo(() => _orderRepository.UpdateAsync(A<Order>._)).MustNotHaveHappened();
        }

        [Test]
        public void SetNewStateWhenOrderedAsync_CallsRepository_WhenOrderedState()
        {
            var order = CreateTestOrder(null, OrderState.Ordered);
            A.CallTo(() => _orderRepository.FindSingleAsync(A<Expression<Func<Order, bool>>>._)).Returns(order);

            _orderService.SetNewStateWhenOrderedAsync(Id);

            A.CallTo(() => _orderRepository.UpdateAsync(order)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task ConfirmAsync_ThrowsException_WhenNotExists()
        {
            var order = CreateTestOrder(null, OrderState.Closed);
            A.CallTo(() => _orderRepository.FindSingleAsync(A<Expression<Func<Order, bool>>>._))
                .Returns(order);

            Func<Task> action = async () => await _orderService.ConfirmAsync(Id);

            await action.Should().ThrowAsync<InvalidServiceOperationException>()
                .WithMessage($"Not found unconfirmed order with id: {Id}");
        }

        [Test]
        public void ConfirmAsync_SubtractGameUnitsInStock_WhenFound()
        {
            const short expectedUnitsInStock = 1;
            var testGame = CreateGameRoot(2);
            var testOrder = CreateTestOrder(testGame, OrderState.Ordered);
            A.CallTo(() => _orderRepository.FindSingleAsync(A<Expression<Func<Order, bool>>>._))
                .Returns(testOrder);

            _orderService.ConfirmAsync(Id);

            A.CallTo(() => _gameDecorator.UpdateUnitsInStockAsync(testGame.Key, expectedUnitsInStock))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task ConfirmAsync_ThrowsException_WhenDetailsQuantityIsGreaterThanGameQuantity()
        {
            var testGame = CreateGameRoot();
            var testOrder = CreateTestOrder(testGame);
            A.CallTo(() => _orderRepository.FindSingleAsync(A<Expression<Func<Order, bool>>>._))
                .Returns(testOrder);

            Func<Task> action = async () => await _orderService.ConfirmAsync(Id);

            await action.Should().ThrowAsync<InvalidServiceOperationException>();
        }

        [Test]
        public void MergeOrdersAsync_DoNothing_WhenOldOrderNotFound()
        {
            var newOrder = CreateTestOrder(null);
            A.CallTo(() => _orderRepository.FindSingleAsync(A<Expression<Func<Order, bool>>>._))
                .ReturnsNextFromSequence(null, newOrder);

            _orderService.MergeOrdersAsync(Id, UserId);

            A.CallTo(() => _orderRepository.DeleteAsync(A<string>._)).MustNotHaveHappened();
        }

        [Test]
        public void MergeOrdersAsync_UpdatesOrderIdInOldOrderDetails_WhenFoundOldOrder()
        {
            var oldOrder = CreateTestOrder(null);
            var orderDto = new OrderDto {Id = Id};
            A.CallTo(() => _orderRepository.FindSingleAsync(A<Expression<Func<Order, bool>>>._)).Returns(oldOrder);
            A.CallTo(() => _mapper.Map<OrderDto>(A<Order>._)).Returns(orderDto);

            _orderService.MergeOrdersAsync(Id, UserId);

            A.CallTo(() => _orderDetailsDecorator.UpdateAsync(
                A<OrderDetails>.That.Matches(od => od.OrderId == Id))).MustHaveHappened();
        }

        [Test]
        public void MergeOrdersAsync_DeletesOldOrder_WhenFound()
        {
            const string newUserId = "2";
            var orderDetailsDecorator = A.Fake<IAsyncRepository<OrderDetails>>();
            A.CallTo(() => _unitOfWork.GetRepository<IAsyncRepository<OrderDetails>>()).Returns(orderDetailsDecorator);
            var oldOrder = CreateTestOrder(null, id: Id);
            var newOrder = CreateTestOrder(null);
            A.CallTo(() => _orderRepository.FindSingleAsync(A<Expression<Func<Order, bool>>>._))
                .ReturnsNextFromSequence(oldOrder, newOrder);

            _orderService.MergeOrdersAsync(Id, newUserId);

            A.CallTo(() => _orderRepository.DeleteAsync(Id)).MustHaveHappened();
        }

        [Test]
        public void SetStateAsync_UpdatesOrderWithNewState_Always()
        {
            const OrderState state = OrderState.Canceled;

            _orderService.SetStateAsync(Id, state);

            A.CallTo(() => _orderRepository.UpdateAsync(
                    A<Order>.That.Matches(o => o.State == state)))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public void CanBeCanceled_ReturnsTrue_WhenOrderStateIsPending()
        {
            var result = _orderService.CanBeCanceled(OrderState.Pending);

            result.Should().BeTrue();
        }

        [Test]
        public void CanBeCanceled_ReturnsTrue_WhenOrderStateIsHigherThanOrdered()
        {
            var result = _orderService.CanBeCanceled(OrderState.Payed);

            result.Should().BeTrue();
        }

        [Test]
        public void CanBeCanceled_ReturnsFalse_WhenOrderStateIsNotPendingOrLowerThanOrdered()
        {
            var result = _orderService.CanBeCanceled(OrderState.Ordered);

            result.Should().BeFalse();
        }

        [Test]
        public void ComputeTotal_ThrowsException_WhenNullDetails()
        {
            Action action = () => _orderService.ComputeTotal(null);

            action.Should().Throw<InvalidServiceOperationException>();
        }

        [Test]
        public void ComputeTotal_ReturnsTotal_WhenNotNullDetails()
        {
            const decimal expectedTotal = 1500;
            var details = CreateOrderDetailsDto();

            var total = _orderService.ComputeTotal(details);

            total.Should().Be(expectedTotal);
        }

        [Test]
        public void ComputeProductsQuantity_ThrowsException_WhenNullDetails()
        {
            Action action = () => _orderService.ComputeProductsQuantity(null);

            action.Should().Throw<InvalidServiceOperationException>();
        }

        [Test]
        public void ComputeProductsQuantity_ReturnsSum_WhenNotNullDetails()
        {
            const int expectedSum = 5;
            var details = CreateOrderDetailsDto();

            var total = _orderService.ComputeProductsQuantity(details);

            total.Should().Be(expectedSum);
        }

        private static Order CreateTestOrder(GameRoot game, OrderState state = OrderState.New, string id = "2")
        {
            var order = new Order
            {
                Id = id,
                Details = new[]
                {
                    new OrderDetails
                    {
                        GameRoot = game,
                        Quantity = 1
                    }
                },
                State = state
            };

            return order;
        }

        private static GameRoot CreateGameRoot(short unitsInStock = 0)
        {
            var game = new GameRoot
            {
                Details = new GameDetails
                {
                    UnitsInStock = unitsInStock
                }
            };

            return game;
        }

        private static IEnumerable<OrderDetailsDto> CreateOrderDetailsDto()
        {
            var details = new[]
            {
                new OrderDetailsDto {Price = 1000, Quantity = 3, Discount = 50},
                new OrderDetailsDto {Quantity = 2}
            };

            return details;
        }
    }
}