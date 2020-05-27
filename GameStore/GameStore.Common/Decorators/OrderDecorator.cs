using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GameStore.Common.Decorators.Interfaces;
using GameStore.Common.Models;
using GameStore.Core.Abstractions;
using GameStore.Core.Models;

namespace GameStore.Common.Decorators
{
    public class OrderDecorator : IAsyncRepository<Order>
    {
        private readonly IAsyncRepository<Order> _sqlOrderRepository;
        private readonly IAsyncReadonlyRepository<Order> _mongoOrderRepository;
        private readonly IGameDecorator _gameDecorator;
        private readonly IAsyncReadonlyRepository<Shipper> _shippersRepository;

        public OrderDecorator(
            IAsyncRepository<Order> orderRepository,
            IAsyncReadonlyRepository<Order> mongoOrderRepository,
            IAsyncReadonlyRepository<Shipper> shippersRepository,
            IGameDecorator gameDecorator)
        {
            _sqlOrderRepository = orderRepository;
            _mongoOrderRepository = mongoOrderRepository;
            _shippersRepository = shippersRepository;
            _gameDecorator = gameDecorator;
        }

        public async Task AddAsync(Order entity)
        {
            await _sqlOrderRepository.AddAsync(entity);
        }

        public async Task UpdateAsync(Order entity)
        {
            await _sqlOrderRepository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(string id)
        {
            await _sqlOrderRepository.DeleteAsync(id);
        }

        public async Task<Order> FindSingleAsync(Expression<Func<Order, bool>> filter)
        {
            var order = await _sqlOrderRepository.FindSingleAsync(filter) ??
                        await _mongoOrderRepository.FindSingleAsync(filter);

            if (order == null)
            {
                return null;
            }

            await SetupShippersAsync(order);
            var areAssignedDetails = order.Details.All(od => od.GameRoot.Details != null);

            if (areAssignedDetails)
            {
                return order;
            }

            await SetupGameDetailsAsync(order);

            return order;
        }

        public async Task<List<Order>> FindAllAsync(Expression<Func<Order, bool>> predicate = null)
        {
            var sqlOrders = await _sqlOrderRepository.FindAllAsync(predicate);
            var mongoOrders = await _mongoOrderRepository.FindAllAsync(predicate);
            var commonOrders = sqlOrders.Concat(mongoOrders).ToArray();
            await SetupShippersAsync(commonOrders);

            return commonOrders.ToList();
        }

        public async Task<bool> AnyAsync(Expression<Func<Order, bool>> predicate)
        {
            var any = await _sqlOrderRepository.AnyAsync(predicate);

            return any;
        }

        private async Task SetupShippersAsync(params Order[] orders)
        {
            var shippersId = orders.Select(o => o.ShipperEntityId).Distinct();
            var shippers = await _shippersRepository.FindAllAsync(s => shippersId.Contains(s.Id));

            foreach (var order in orders)
            {
                order.Shipper = shippers.FirstOrDefault(s => s.Id == order.ShipperEntityId);
            }
        }

        private async Task SetupGameDetailsAsync(params Order[] orders)
        {
            var orderDetails = orders.SelectMany(o => o.Details);
            var keys = orderDetails.Select(od => od.GameRoot.Key).ToList();
            var roots = await _gameDecorator.FindAllAsync(new GameFilterData {Keys = keys});

            foreach (var details in orderDetails)
            {
                details.GameRoot = roots.FirstOrDefault(r => r.Id == details.GameRootId);
            }
        }
    }
}