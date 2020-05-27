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
using GameStore.Core.Abstractions;
using GameStore.Core.Models;
using NUnit.Framework;

namespace GameStore.BusinessLayer.Tests
{
    [TestFixture]
    public class OrderDetailsServiceTests
    {
        private const string Id = "1";

        private IUnitOfWork _unitOfWork;
        private IAsyncRepository<OrderDetails> _orderDetailsDecorator;
        private IGameService _gameService;
        private IMapper _mapper;
        private IOrderDetailsService _orderDetailsService;

        [SetUp]
        public void Setup()
        {
            _unitOfWork = A.Fake<IUnitOfWork>();
            _orderDetailsDecorator = A.Fake<IAsyncRepository<OrderDetails>>();
            _gameService = A.Fake<IGameService>();
            _mapper = A.Fake<IMapper>();
            A.CallTo(() => _unitOfWork.GetRepository<IAsyncRepository<OrderDetails>>()).Returns(_orderDetailsDecorator);

            _orderDetailsService = new OrderDetailsService(_unitOfWork, _gameService, _mapper);
        }

        [Test]
        public async Task CreateAsync_ThrowsException_WhenNullDto()
        {
            Func<Task> action = async () => await _orderDetailsService.CreateAsync(null);

            await action.Should().ThrowAsync<InvalidServiceOperationException>();
        }

        [Test]
        public void CreateAsync_SetsPriceAndDiscountAsGamePriceAndDiscount_WhenFoundGame()
        {
            var modifyGameDto = CreateModifyGameDto();
            var orderDetails = CreateOrderDetails(modifyGameDto.Price);
            var orderDetailsDto = CreateOrderDetailsDto();
            A.CallTo(() => _gameService.GetByIdAsync(A<string>._)).Returns(modifyGameDto);
            A.CallTo(() => _mapper.Map<OrderDetails>(A<OrderDetailsDto>._)).Returns(orderDetails);

            _orderDetailsService.CreateAsync(orderDetailsDto);

            A.CallTo(() =>
                    _orderDetailsDecorator.AddAsync(
                        A<OrderDetails>.That.Matches(od => od.Price == modifyGameDto.Price)))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task UpdateAsync_ThrowsException_WhenNullDto()
        {
            Func<Task> action = async () => await _orderDetailsService.UpdateAsync(null);

            await action.Should().ThrowAsync<InvalidServiceOperationException>();
        }

        [Test]
        public async Task UpdateAsync_ThrowsException_WhenDetailsAreNotExist()
        {
            var orderDetailsDto = CreateOrderDetailsDto();

            Func<Task> action = async () => await _orderDetailsService.UpdateAsync(orderDetailsDto);

            await action.Should().ThrowAsync<EntityNotFoundException<OrderDetails>>();
        }

        [Test]
        public async Task UpdateAsync_ThrowsException_WhenNegativeQuantity()
        {
            var orderDetails = CreateOrderDetails(quantity: -1);
            var orderDetailsDto = CreateOrderDetailsDto();
            A.CallTo(() => _mapper.Map<OrderDetails>(A<OrderDetailsDto>._)).Returns(orderDetails);
            A.CallTo(() => _orderDetailsDecorator.AnyAsync(A<Expression<Func<OrderDetails, bool>>>._)).Returns(true);

            Func<Task> action = async () => await _orderDetailsService.UpdateAsync(orderDetailsDto);

            await action.Should().ThrowAsync<ValidationException<OrderDetails>>().WithMessage("Is negative quantity");
        }

        [Test]
        public void UpdateAsync_DeletesDetails_WhenZeroQuantity()
        {
            var orderDetails = CreateOrderDetails();
            var orderDetailsDto = CreateOrderDetailsDto();
            A.CallTo(() => _mapper.Map<OrderDetails>(A<OrderDetailsDto>._)).Returns(orderDetails);
            A.CallTo(() => _orderDetailsDecorator.AnyAsync(A<Expression<Func<OrderDetails, bool>>>._)).Returns(true);

            _orderDetailsService.UpdateAsync(orderDetailsDto);

            A.CallTo(() => _orderDetailsDecorator.DeleteAsync(Id)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task UpdateAsync_ThrowsException_WhenQuantityIsMoreThanAvailableUnitsInStock()
        {
            var modifyGameDto = CreateModifyGameDto();
            var orderDetails = CreateOrderDetails(quantity: 11);
            var orderDetailsDto = CreateOrderDetailsDto();
            A.CallTo(() => _gameService.GetByIdAsync(A<string>._)).Returns(modifyGameDto);
            A.CallTo(() => _mapper.Map<OrderDetails>(A<OrderDetailsDto>._)).Returns(orderDetails);
            A.CallTo(() => _orderDetailsDecorator.AnyAsync(A<Expression<Func<OrderDetails, bool>>>._)).Returns(true);

            Func<Task> action = async () => await _orderDetailsService.UpdateAsync(orderDetailsDto);

            await action.Should().ThrowAsync<ValidationException<OrderDetails>>()
                .WithMessage($"Available only {modifyGameDto.UnitsInStock} units in stock");
        }

        [Test]
        public void UpdateAsync_SetsPriceAndDiscountAsGamePriceAndDiscount_WhenValidDto()
        {
            var modifyGameDto = CreateModifyGameDto();
            var orderDetails = CreateOrderDetails(quantity: 1);
            var orderDetailsDto = CreateOrderDetailsDto();
            A.CallTo(() => _gameService.GetByIdAsync(A<string>._)).Returns(modifyGameDto);
            A.CallTo(() => _mapper.Map<OrderDetails>(A<OrderDetailsDto>._)).Returns(orderDetails);
            A.CallTo(() => _orderDetailsDecorator.AnyAsync(A<Expression<Func<OrderDetails, bool>>>._)).Returns(true);

            _orderDetailsService.UpdateAsync(orderDetailsDto);

            A.CallTo(() => _orderDetailsDecorator.UpdateAsync(
                    A<OrderDetails>.That.Matches(od => od.Price == modifyGameDto.Price)))
                .MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task DeleteAsync_ThrowsException_WhenNotExists()
        {
            Func<Task> action = async () => await _orderDetailsService.DeleteAsync(Id);

            await action.Should().ThrowAsync<EntityNotFoundException<OrderDetails>>();
        }

        [Test]
        public void DeleteAsync_CallsRepository_WhenExistsDetails()
        {
            A.CallTo(() => _orderDetailsDecorator.AnyAsync(A<Expression<Func<OrderDetails, bool>>>._))
                .Returns(true);

            _orderDetailsService.DeleteAsync(Id);

            A.CallTo(() => _orderDetailsDecorator.DeleteAsync(Id)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task GetByIdAsync_ThrowsException_WhenNotFound()
        {
            A.CallTo(() => _orderDetailsDecorator.FindSingleAsync(A<Expression<Func<OrderDetails, bool>>>._))
                .Returns((OrderDetails) null);

            Func<Task> action = async () => await  _orderDetailsService.GetByIdAsync(Id);

            await action.Should().ThrowAsync<EntityNotFoundException<OrderDetails>>();
        }

        [Test]
        public void GetByIdAsync_ReturnsDetailsDto_WhenFound()
        {
            var detailsDto = _orderDetailsService.GetByIdAsync(Id);

            detailsDto.Should().NotBeNull();
        }

        [Test]
        public void FindSingleAsync_CallsRepository_Always()
        {
            Expression<Func<OrderDetails, bool>> expression = od => true;

            _orderDetailsService.FindSingleAsync(expression);

            A.CallTo(() => _orderDetailsDecorator.FindSingleAsync(expression)).MustHaveHappenedOnceExactly();
        }

        private static OrderDetailsDto CreateOrderDetailsDto(short quantity = 0)
        {
            var dto = new OrderDetailsDto
            {
                Id = Id,
                Quantity = quantity
            };

            return dto;
        }

        private static OrderDetails CreateOrderDetails(decimal price = 0, short quantity = 0)
        {
            var details = new OrderDetails
            {
                Id = Id,
                Price = price,
                Quantity = quantity
            };

            return details;
        }

        private static ModifyGameDto CreateModifyGameDto()
        {
            var dto = new ModifyGameDto
            {
                UnitsInStock = 10,
                Price = 10
            };

            return dto;
        }
    }
}