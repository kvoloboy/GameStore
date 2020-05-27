using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GameStore.DataAccess.Mongo.Models;
using GameStore.DataAccess.Mongo.Repositories.Interfaces;
using GameStore.Infrastructure.DatabaseSettings.Interfaces;
using GameStore.Infrastructure.Extensions;
using GameStore.Infrastructure.Logging.Interfaces;
using GameStore.Infrastructure.Logging.Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace GameStore.DataAccess.Mongo.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMongoCollection<Product> _productsCollection;
        private readonly ILogger _logger;

        public ProductRepository(
            IMongoClient mongoClient,
            IMongoDatabaseSettings<Product> mongoDatabaseSettings,
            ILogger logger)
        {
            var databaseName = mongoDatabaseSettings.GetDatabaseName();
            var collectionName = mongoDatabaseSettings.GetCollectionName();

            var database = mongoClient.GetDatabase(databaseName);
            _productsCollection = database.GetCollection<Product>(collectionName);
            _logger = logger;
        }

        public async Task<List<Product>> FindAllAsync(Expression<Func<Product, bool>> predicate = null)
        {
            var filter = MongoHelpers.GetDocumentFilter(predicate);
            var games = (await _productsCollection.FindAsync(filter)).ToList();

            return games;
        }

        public async Task<Product> FindSingleAsync(Expression<Func<Product, bool>> predicate)
        {
            var filter = MongoHelpers.GetDocumentFilter(predicate);
            var product = (await _productsCollection.FindAsync(filter)).FirstOrDefault();

            return product;
        }

        public Task<bool> AnyAsync(Expression<Func<Product, bool>> predicate)
        {
            var any = _productsCollection.AsQueryable().AnyAsync(predicate);

            return any;
        }

        public async Task UpdateUnitsInStockAsync(string key, short unitsInStock)
        {
            var existingProduct = await FindSingleAsync(p => p.Key == key);
            var oldValueInstance = existingProduct.Clone();
            existingProduct.UnitsInStock = unitsInStock;
            var filter = MongoHelpers.GetDocumentFilter<Product>(p => p.Key == key);
            await _productsCollection.ReplaceOneAsync(filter, existingProduct);

            var entry = new LogEntry<Product>(Operation.Update, oldValueInstance, existingProduct);
            _logger.Log(entry);
        }

        public async Task UpdateKeyAsync(string productId, string key)
        {
            var existingProduct = await GetById(productId);
            var oldValueInstance = existingProduct.Clone();
            existingProduct.Key = key;
            var filter = MongoHelpers.GetDocumentFilter<Product>(p => p.Id == productId);
            await _productsCollection.ReplaceOneAsync(filter, existingProduct);

            var entry = new LogEntry<Product>(Operation.Update, oldValueInstance, existingProduct);
            _logger.Log(entry);
        }

        private Task<Product> GetById(string id)
        {
            var product = FindSingleAsync(p => p.Id == id);

            return product;
        }
    }
}