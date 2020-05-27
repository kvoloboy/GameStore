using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FakeItEasy;
using FluentAssertions;
using GameStore.Common.Decorators;
using GameStore.Common.Decorators.Interfaces;
using GameStore.Common.Models;
using GameStore.Core.Abstractions;
using GameStore.Core.Models;
using NUnit.Framework;

namespace GameStore.Common.Tests.DecoratorsTests
{
    [TestFixture]
    public class OrderDecoratorTests
    {
        private const string Id = "1";

        private readonly Expression<Func<Order, bool>> _testExpression = publisher => true;

        private IAsyncRepository<Order> _sqlOrderRepository;
        private IAsyncReadonlyRepository<Order> _mongoOrderRepository;
        private IGameDecorator _gameDecorator;
        private IAsyncReadonlyRepository<Shipper> _shippersRepository;

        private OrderDecorator _orderDecorator;

        [SetUp]
        public void SetUp()
        {
            _sqlOrderRepository = A.Fake<IAsyncRepository<Order>>();
            _mongoOrderRepository = A.Fake<IAsyncReadonlyRepository<Order>>();
            _gameDecorator = A.Fake<IGameDecorator>();
            _shippersRepository = A.Fake<IAsyncReadonlyRepository<Shipper>>();

            _orderDecorator = new OrderDecorator(
                _sqlOrderRepository,
                _mongoOrderRepository,
                _shippersRepository,
                _gameDecorator);
        }

        [Test]
        public void AddAsync_CallsRepository_Always()
        {
            var order = new Order();

            _orderDecorator.AddAsync(order);

            A.CallTo(() => _sqlOrderRepository.AddAsync(order)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void UpdateAsync_CallsRepository_Always()
        {
            var order = new Order();

            _orderDecorator.UpdateAsync(order);

            A.CallTo(() => _sqlOrderRepository.UpdateAsync(order)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void DeleteAsync_CallsRepository_Always()
        {
            _orderDecorator.DeleteAsync(Id);

            A.CallTo(() => _sqlOrderRepository.DeleteAsync(Id)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void FindSingleAsync_CallsSqlRepository_Always()
        {
            var order = CreateOrder();
            A.CallTo(() => _sqlOrderRepository.FindSingleAsync(_testExpression)).Returns(order);

            _orderDecorator.FindSingleAsync(_testExpression);

            A.CallTo(() => _sqlOrderRepository.FindSingleAsync(_testExpression)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void FindSingleAsync_DoesntCallMongoRepository_WhenFoundInSql()
        {
            var order = CreateOrder();
            A.CallTo(() => _sqlOrderRepository.FindSingleAsync(_testExpression)).Returns(order);

            _orderDecorator.FindSingleAsync(_testExpression);

            A.CallTo(() => _mongoOrderRepository.FindSingleAsync(_testExpression)).MustNotHaveHappened();
        }

        [Test]
        public void FindSingleAsync_ReturnsNullWhenNotFound()
        {
            A.CallTo(() => _sqlOrderRepository.FindSingleAsync(_testExpression)).Returns((Order) null);
            A.CallTo(() => _mongoOrderRepository.FindSingleAsync(_testExpression)).Returns((Order) null);

            var result = _orderDecorator.FindSingleAsync(_testExpression).Result;

            result.Should().BeNull();
        }

        [Test]
        public void FindSingleAsync_ReturnsOrderWithShipper_WhenFound()
        {
            var testOrder = CreateOrder();
            var shippers = new List<Shipper>
            {
                CreateShipper()
            };
            A.CallTo(() => _sqlOrderRepository.FindSingleAsync(_testExpression)).Returns(testOrder);
            A.CallTo(() => _shippersRepository.FindAllAsync(A<Expression<Func<Shipper, bool>>>._)).Returns(shippers);

            var order = _orderDecorator.FindSingleAsync(_testExpression).Result;

            order.Shipper.Should().NotBeNull();
        }

        [Test]
        public void FindSingleAsync_ReturnOrderWithInitializedGameDetails_WhenFound()
        {
            var testOrder = CreateOrder();
            var gameRoot = new GameRoot {Id = Id};
            testOrder.Details = new[] {CreateDetails()};
            A.CallTo(() => _sqlOrderRepository.FindSingleAsync(_testExpression)).Returns(testOrder);
            A.CallTo(() => _gameDecorator.FindAllAsync(A<GameFilterData>._)).Returns(new[] {gameRoot});

            var order = _orderDecorator.FindSingleAsync(_testExpression).Result;

            order.Details.First().GameRoot.Should().Be(gameRoot);
        }

        [Test]
        public void FindAllAsync_CallsSqlRepositories_Always()
        {
            _orderDecorator.FindAllAsync(_testExpression);

            A.CallTo(() => _sqlOrderRepository.FindAllAsync(_testExpression)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void FindAllAsync_CallsMongoRepositories_Always()
        {
            _orderDecorator.FindAllAsync(_testExpression);

            A.CallTo(() => _mongoOrderRepository.FindAllAsync(_testExpression)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void FindAllAsync_ReturnsOrdersWithShippers_WhenFound()
        {
            var shippers = new List<Shipper> {CreateShipper()};
            var orders = new List<Order> {CreateOrder()};
            A.CallTo(() => _sqlOrderRepository.FindAllAsync(_testExpression)).Returns(orders);
            A.CallTo(() => _shippersRepository.FindAllAsync(A<Expression<Func<Shipper, bool>>>._)).Returns(shippers);

            var ordersResult = _orderDecorator.FindAllAsync(_testExpression).Result;

            ordersResult.Last().Shipper.Should().NotBeNull();
        }

        [Test]
        public void FindAllAsync_ReturnsDataFromTwoSources_Always()
        {
            const int expectedOrdersCount = 2;
            var orders = new List<Order> {CreateOrder()};
            A.CallTo(() => _sqlOrderRepository.FindAllAsync(_testExpression)).Returns(orders);
            A.CallTo(() => _mongoOrderRepository.FindAllAsync(_testExpression)).Returns(orders);

            var ordersResult = _orderDecorator.FindAllAsync(_testExpression).Result;
            var ordersCount = ordersResult.Count();

            ordersCount.Should().Be(expectedOrdersCount);
        }

        [Test]
        public void AnyAsync_CallsSqlRepository_Always()
        {
            const bool expectedResult = true;
            A.CallTo(() => _sqlOrderRepository.AnyAsync(_testExpression)).Returns(expectedResult);

            var result = _orderDecorator.AnyAsync(_testExpression).Result;

            result.Should().Be(expectedResult);
        }

        private static Order CreateOrder()
        {
            var order = new Order
            {
                ShipperEntityId = Id,
                Details = new List<OrderDetails>()
            };

            return order;
        }

        private static OrderDetails CreateDetails(GameRoot gameRoot = null)
        {
            var details = new OrderDetails
            {
                GameRoot = gameRoot ?? new GameRoot(),
                GameRootId = Id
            };

            return details;
        }

        private static Shipper CreateShipper()
        {
            var shipper = new Shipper
            {
                Id = Id
            };

            return shipper;
        }
    }
}