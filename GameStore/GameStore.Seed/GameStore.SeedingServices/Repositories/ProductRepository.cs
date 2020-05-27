using System.Collections.Generic;
using GameStore.Infrastructure.DatabaseSettings.Interfaces;
using GameStore.SeedingServices.Models;
using GameStore.SeedingServices.Repositories.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace GameStore.SeedingServices.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMongoCollection<Product> _productCollection;

        public ProductRepository(IMongoClient mongoClient, IMongoDatabaseSettings<Product> mongoDatabaseSettings)
        {
            var databaseName = mongoDatabaseSettings.GetDatabaseName();
            var collectionName = mongoDatabaseSettings.GetCollectionName();
            
            var database = mongoClient.GetDatabase(databaseName);
            _productCollection = database.GetCollection<Product>(collectionName);
        }

        public IEnumerable<Product> GetAll()
        {
            var products = _productCollection.AsQueryable().ToList();

            return products;
        }

        public void SetKey(ObjectId productId, string key)
        {
            var updateDefinition = Builders<Product>.Update.Set(p => p.Key, key);
            _productCollection.UpdateOne(p => p.Id == productId, updateDefinition);
        }
    }
}