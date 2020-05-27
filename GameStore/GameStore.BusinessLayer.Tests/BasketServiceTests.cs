using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using GameStore.BusinessLayer.DTO;
using GameStore.BusinessLayer.Exceptions;
using GameStore.BusinessLayer.Services;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Core.Models;
using NUnit.Framework;

namespace GameStore.BusinessLayer.Tests
{
    [TestFixture]
    public class BasketServiceTests
    {
        private const string CustomerId = "1";
        private const string DetailsId = "1";
        private const string GameKey = "key";
        private const string Culture = Common.Models.Culture.En;

        private IOrderService _orderService;
        private IGameService _gameService;
        private IOrderDetailsService _orderDetailsService;
        private IMapper _mapper;
        private BasketService _basketService;

        [SetUp]
        public void Setup()
        {
            _orderService = A.Fake<IOrderService>();
            _orderDetailsService = A.Fake<IOrderDetailsService>();
            _gameService = A.Fake<IGameService>();
            _mapper = A.Fake<IMapper>();

            _basketService = new BasketService(_gameService, _orderService, _orderDetailsService, _mapper);
        }

        [Test]
        public void GetBasketForCustomerAsync_ReturnsBasketWithNewOrder_Always()
        {
            _basketService.GetBasketForUserAsync(CustomerId);

            A.CallTo(() => _orderService.GetNewOrderByUserIdAsync(CustomerId, Common.Models.Culture.En))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task AddAsync_ThrowsException_WhenEmptyGameKey()
        {
            var gameKey = string.Empty;

            Func<Task> action = async () => await _basketService.AddAsync(gameKey, CustomerId);

            await action.Should().ThrowAsync<InvalidServiceOperationException>().WithMessage("Is empty game key");
        }

        [Test]
        public void AddAsync_CreatesNewDetails_WhenNotExists()
        {
            var testGameDto = new GameDto();
            A.CallTo(() => _gameService.GetByKeyAsync(A<string>._, Culture)).Returns(testGameDto);
            A.CallTo(() => _orderDetailsService.FindSingleAsync(A<Expression<Func<OrderDetails, bool>>>._))
                .Returns((OrderDetails) null);

            _basketService.AddAsync(GameKey, CustomerId);

            A.CallTo(() => _orderDetailsService.CreateAsync(A<OrderDetailsDto>._)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void AddAsync_UpdatesExistingDetailsQuantity_WhenFound()
        {
            const int expectedQuantity = 2;
            var testOrderDetails = CreateOrderDetails();
            A.CallTo(() => _orderDetailsService.FindSingleAsync(A<Expression<Func<OrderDetails, bool>>>._))
                .Returns(testOrderDetails);

            _basketService.AddAsync(GameKey, CustomerId);

            testOrderDetails.Quantity.Should().Be(expectedQuantity);
        }

        [Test]
        public void UpdateQuantityAsync_SetsNewQuantityToExistingDetails_Always()
        {
            const int quantity = 1;
            A.CallTo(() => _orderDetailsService.GetByIdAsync(A<string>._)).Returns(new OrderDetailsDto());

            _basketService.UpdateQuantityAsync(DetailsId, quantity);

            A.CallTo(() => _orderDetailsService.UpdateAsync(
                A<OrderDetailsDto>.That.Matches(dto => dto.Quantity == quantity))).MustHaveHappenedOnceExactly();
        }

        [Test]
        public void DeleteAsync_CallsOrderDetailsService_Always()
        {
            _basketService.DeleteAsync(DetailsId);

            A.CallTo(() => _orderDetailsService.DeleteAsync(DetailsId)).MustHaveHappenedOnceExactly();
        }

        private static OrderDetails CreateOrderDetails()
        {
            var details = new OrderDetails
            {
                Id = "1",
                Quantity = 1,
                Discount = 1,
                Price = 1,
                GameRoot = new GameRoot
                {
                    Details = new GameDetails
                    {
                        UnitsInStock = 10
                    }
                },
                Order = new Order()
            };

            return details;
        }
    }
}