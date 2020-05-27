using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using GameStore.BusinessLayer.DTO;
using GameStore.BusinessLayer.Exceptions;
using GameStore.BusinessLayer.Services.Interfaces;
using GameStore.Core.Abstractions;
using GameStore.Core.Models;

namespace GameStore.BusinessLayer.Services
{
    public class OrderDetailsService : IOrderDetailsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAsyncRepository<OrderDetails> _orderDetailsDecorator;
        private readonly IGameService _gameService;
        private readonly IMapper _mapper;

        public OrderDetailsService(
            IUnitOfWork unitOfWork,
            IGameService gameService,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _gameService = gameService;
            _orderDetailsDecorator = _unitOfWork.GetRepository<IAsyncRepository<OrderDetails>>();
            _mapper = mapper;
        }

        public async Task CreateAsync(OrderDetailsDto orderDetailsDto)
        {
            if (orderDetailsDto == null)
            {
                throw new InvalidServiceOperationException("Is null dto");
            }

            var orderDetails = _mapper.Map<OrderDetails>(orderDetailsDto);
            await ValidateQuantityAsync(orderDetails);

            var gameDto = await _gameService.GetByIdAsync(orderDetailsDto.GameId);
            orderDetails.Price = gameDto.Price;
            orderDetails.Discount = gameDto.Discount;

            await _orderDetailsDecorator.AddAsync(orderDetails);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateAsync(OrderDetailsDto orderDetailsDto)
        {
            if (orderDetailsDto == null)
            {
                throw new InvalidServiceOperationException("Is null dto");
            }

            var exists = await _orderDetailsDecorator.AnyAsync(od => od.Id == orderDetailsDto.Id);

            if (!exists)
            {
                throw new EntityNotFoundException<OrderDetails>(orderDetailsDto.Id);
            }

            var details = _mapper.Map<OrderDetails>(orderDetailsDto);

            if (details.Quantity == 0)
            {
                await _orderDetailsDecorator.DeleteAsync(details.Id);
                await _unitOfWork.CommitAsync();
             
                return;
            }

            await ValidateQuantityAsync(details);

            var game = await _gameService.GetByIdAsync(orderDetailsDto.GameId);
            details.Price = game.Price;
            details.Discount = game.Discount;

            await _orderDetailsDecorator.UpdateAsync(details);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteAsync(string detailsId)
        {
            var exists = await _orderDetailsDecorator.AnyAsync(od => od.Id == detailsId);

            if (!exists)
            {
                throw new EntityNotFoundException<OrderDetails>(detailsId);
            }

            await _orderDetailsDecorator.DeleteAsync(detailsId);
            await _unitOfWork.CommitAsync();
        }

        public async Task<OrderDetailsDto> GetByIdAsync(string id)
        {
            var details = await _orderDetailsDecorator.FindSingleAsync(od => od.Id == id);

            if (details == null)
            {
                throw new EntityNotFoundException<OrderDetails>(id);
            }

            var detailsDto = _mapper.Map<OrderDetailsDto>(details);

            return detailsDto;
        }

        public async Task<OrderDetails> FindSingleAsync(Expression<Func<OrderDetails, bool>> predicate)
        {
            var orderDetails = await _orderDetailsDecorator.FindSingleAsync(predicate);

            return orderDetails;
        }

        private async Task ValidateQuantityAsync(OrderDetails orderDetails)
        {
            var detailsQuantity = orderDetails.Quantity;
            string errorMessage;

            if (detailsQuantity < 0)
            {
                errorMessage = "Is negative quantity";
                throw new ValidationException<OrderDetails>(orderDetails.Quantity.ToString(), errorMessage);
            }

            var gameDto = await _gameService.GetByIdAsync(orderDetails.GameRootId);
            var gamesInStock = gameDto.UnitsInStock;
            var areValidDetails = gamesInStock == null || detailsQuantity <= gamesInStock;

            if (areValidDetails)
            {
                return;
            }

            errorMessage = $"Available only {gamesInStock} units in stock";
            throw new ValidationException<OrderDetails>(detailsQuantity.ToString(), errorMessage);
        }
    }
}