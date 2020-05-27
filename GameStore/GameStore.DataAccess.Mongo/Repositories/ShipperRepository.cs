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
    public class ShipperRepository : IAsyncReadonlyRepository<Shipper>
    {
        private readonly IMongoCollection<Shipper> _shipperCollection;

        public ShipperRepository(IMongoDatabaseSettings<Shipper> mongoDatabaseSettings, IMongoClient mongoClient)
        {
            var databaseName = mongoDatabaseSettings.GetDatabaseName();
            var collectionName = mongoDatabaseSettings.GetCollectionName();

            var database = mongoClient.GetDatabase(databaseName);
            _shipperCollection = database.GetCollection<Shipper>(collectionName);
        }

        public async Task<Shipper> FindSingleAsync(Expression<Func<Shipper, bool>> predicate)
        {
            var filter = MongoHelpers.GetDocumentFilter(predicate);
            var shipper = (await _shipperCollection.FindAsync(filter)).FirstOrDefault();

            return shipper;
        }

        public async Task<List<Shipper>> FindAllAsync(Expression<Func<Shipper, bool>> predicate = null)
        {
            var filter = MongoHelpers.GetDocumentFilter(predicate);
            var shippers = (await _shipperCollection.FindAsync(filter)).ToList();

            return shippers;
        }

        public Task<bool> AnyAsync(Expression<Func<Shipper, bool>> predicate)
        {
            var any = _shipperCollection.AsQueryable().AnyAsync(predicate);

            return any;
        }
    }
}