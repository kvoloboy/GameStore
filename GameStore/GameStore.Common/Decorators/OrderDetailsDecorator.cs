using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GameStore.Common.Decorators.Interfaces;
using GameStore.Core.Abstractions;
using GameStore.Core.Models;

namespace GameStore.Common.Decorators
{
    public class OrderDetailsDecorator : IAsyncRepository<OrderDetails>
    {
        private readonly IGameDecorator _gameDecorator;
        private readonly IAsyncRepository<OrderDetails> _orderDetailsRepository;

        public OrderDetailsDecorator(
            IAsyncRepository<OrderDetails> orderDetailsRepository,
            IGameDecorator gameDecorator)
        {
            _orderDetailsRepository = orderDetailsRepository;
            _gameDecorator = gameDecorator;
        }

        public async Task<OrderDetails> FindSingleAsync(Expression<Func<OrderDetails, bool>> predicate)
        {
            var details = await _orderDetailsRepository.FindSingleAsync(predicate);

            if (details == null)
            {
                return null;
            }

            Expression<Func<GameRoot, bool>> gamePredicate = root => root.Id == details.GameRootId;
            var gameRoot = await _gameDecorator.FindSingleAsync(gamePredicate);
            details.GameRoot = gameRoot;

            return details;
        }

        public async Task<List<OrderDetails>> FindAllAsync(Expression<Func<OrderDetails, bool>> predicate = null)
        {
            var orderDetails = await _orderDetailsRepository.FindAllAsync(predicate);

            return orderDetails;
        }

        public async Task<bool> AnyAsync(Expression<Func<OrderDetails, bool>> predicate)
        {
            var any = await _orderDetailsRepository.AnyAsync(predicate);

            return any;
        }

        public async Task AddAsync(OrderDetails entity)
        {
            await _orderDetailsRepository.AddAsync(entity);
        }

        public async Task UpdateAsync(OrderDetails entity)
        {
            await _orderDetailsRepository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(string id)
        {
            await _orderDetailsRepository.DeleteAsync(id);
        }
    }
}