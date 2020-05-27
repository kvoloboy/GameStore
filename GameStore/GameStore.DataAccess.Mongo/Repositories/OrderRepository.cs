using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using GameStore.Core.Abstractions;
using GameStore.Core.Models;
using MongoDB.Driver;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GameStore.DataAccess.Mongo.Repositories.Interfaces;
using GameStore.Infrastructure.DatabaseSettings.Interfaces;
using MongoDB.Driver.Linq;

namespace GameStore.DataAccess.Mongo.Repositories
{
    public class OrderRepository : IAsyncReadonlyRepository<Order>
    {
        private readonly IAsyncReadonlyRepository<OrderDetails> _orderDetailsRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMongoCollection<Order> _ordersCollection;
        private readonly IMapper _mapper;

        public OrderRepository(
            IMongoDatabaseSettings<Order> mongoDatabaseSettings,
            IMongoClient mongoClient,
            IAsyncReadonlyRepository<OrderDetails> orderDetailsRepository,
            IProductRepository productRepository,
            IMapper mapper)
        {
            var databaseName = mongoDatabaseSettings.GetDatabaseName();
            var collectionName = mongoDatabaseSettings.GetCollectionName();
            var database = mongoClient.GetDatabase(databaseName);
            _ordersCollection = database.GetCollection<Order>(collectionName);

            _orderDetailsRepository = orderDetailsRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<Order> FindSingleAsync(Expression<Func<Order, bool>> predicate)
        {
            var filter = MongoHelpers.GetDocumentFilter(predicate);
            var order = (await _ordersCollection.FindAsync(filter)).FirstOrDefault();

            if (order == null)
            {
                return null;
            }

            await SetupDetails(new[] {order});
            await SetupGameDetails(order.Details);

            return order;
        }

        public async Task<List<Order>> FindAllAsync(Expression<Func<Order, bool>> predicate = null)
        {
            var filter = MongoHelpers.GetDocumentFilter(predicate);
            var orders = (await _ordersCollection.FindAsync(filter)).ToList();
            await SetupDetails(orders);

            return orders;
        }

        public Task<bool> AnyAsync(Expression<Func<Order, bool>> predicate)
        {
            var any = _ordersCollection.AsQueryable().AnyAsync(predicate);

            return any;
        }

        private async Task SetupDetails(IEnumerable<Order> orders)
        {
            var ordersId = orders.Select(o => o.Id);
            var orderDetails = await _orderDetailsRepository.FindAllAsync(od => ordersId.Contains(od.OrderId));

            foreach (var order in orders)
            {
                var details = orderDetails.Where(od => od.OrderId == order.Id).ToList();
                order.Details = details;
            }
        }

        private async Task SetupGameDetails(IEnumerable<OrderDetails> orderDetails)
        {
            var productsId = orderDetails.Select(od => od.GameRootId).Distinct().ToList();
            var products = (await _productRepository.FindAllAsync(p => productsId.Contains(p.Id))).ToList();

            foreach (var details in orderDetails)
            {
                var targetProduct = products.First(p => p.Id == details.GameRootId);
                var gameRoot = _mapper.Map<GameRoot>(targetProduct);
                gameRoot.Id = targetProduct.Id;
                details.GameRoot = gameRoot;
            }
        }
    }
}