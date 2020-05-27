using System.Collections.Generic;
using GameStore.Core.Models;
using GameStore.Infrastructure.DatabaseSettings.Interfaces;
using GameStore.SeedingServices.Repositories.Interfaces;
using MongoDB.Driver;

namespace GameStore.SeedingServices.Repositories
{
    public class CategoryRepository : IRepository<Genre>
    {
        private readonly IMongoCollection<Genre> _genreCollection;

        public CategoryRepository(IMongoClient mongoClient, IMongoDatabaseSettings<Genre> mongoDatabaseSettings)
        {
            var databaseName = mongoDatabaseSettings.GetDatabaseName();
            var collectionName = mongoDatabaseSettings.GetCollectionName();
            
            var database = mongoClient.GetDatabase(databaseName);
            _genreCollection = database.GetCollection<Genre>(collectionName);
        }

        public IEnumerable<Genre> GetAll()
        {
            var genres = _genreCollection.AsQueryable().ToList();

            return genres;
        }
    }
}