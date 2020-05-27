using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GameStore.Core.Abstractions;
using GameStore.Core.Models;
using GameStore.Infrastructure.DatabaseSettings.Interfaces;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace GameStore.DataAccess.Mongo.Repositories
{
    public class OrderDetailsRepository : IAsyncReadonlyRepository<OrderDetails>
    {
        private readonly IMongoCollection<OrderDetails> _orderDetailsCollection;

        public OrderDetailsRepository(
            IMongoClient mongoClient,
            IMongoDatabaseSettings<OrderDetails> mongoDatabaseSettings)
        {
            var databaseName = mongoDatabaseSettings.GetDatabaseName();
            var collectionName = mongoDatabaseSettings.GetCollectionName();

            var database = mongoClient.GetDatabase(databaseName);
            _orderDetailsCollection = database.GetCollection<OrderDetails>(collectionName);
        }

        public async Task<OrderDetails> FindSingleAsync(Expression<Func<OrderDetails, bool>> predicate)
        {
            var filter = MongoHelpers.GetDocumentFilter(predicate);
            var details = (await _orderDetailsCollection.FindAsync(filter)).FirstOrDefault();

            return details;
        }

        public async Task<List<OrderDetails>> FindAllAsync(Expression<Func<OrderDetails, bool>> predicate = null)
        {
            var filter = MongoHelpers.GetDocumentFilter(predicate);
            var details = (await _orderDetailsCollection.FindAsync(filter)).ToList();

            return details;
        }

        public Task<bool> AnyAsync(Expression<Func<OrderDetails, bool>> predicate)
        {
            var any = _orderDetailsCollection.AsQueryable().AnyAsync(predicate);

            return any;
        }
    }
}