using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GameStore.Core.Abstractions;
using GameStore.DataAccess.Mongo.Models;
using GameStore.Infrastructure.DatabaseSettings.Interfaces;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace GameStore.DataAccess.Mongo.Repositories
{
    public class PublisherRepository : IAsyncReadonlyRepository<Publisher>
    {
        private readonly IMongoCollection<Publisher> _publishersCollection;

        public PublisherRepository(IMongoDatabaseSettings<Publisher> mongoDatabaseSettings, IMongoClient mongoClient)
        {
            var databaseName = mongoDatabaseSettings.GetDatabaseName();
            var collectionName = mongoDatabaseSettings.GetCollectionName();

            var database = mongoClient.GetDatabase(databaseName);
            _publishersCollection = database.GetCollection<Publisher>(collectionName);
        }

        public async Task<Publisher> FindSingleAsync(Expression<Func<Publisher, bool>> predicate)
        {
            var filter = MongoHelpers.GetDocumentFilter(predicate);
            var publisher = (await _publishersCollection.FindAsync(filter)).FirstOrDefault();

            return publisher;
        }

        public async Task<List<Publisher>> FindAllAsync(Expression<Func<Publisher, bool>> predicate = null)
        {
            var filter = MongoHelpers.GetDocumentFilter(predicate);
            var publishers = (await _publishersCollection.FindAsync(filter)).ToList();

            return publishers;
        }

        public Task<bool> AnyAsync(Expression<Func<Publisher, bool>> predicate)
        {
            var any = _publishersCollection.AsQueryable().AnyAsync(predicate);

            return any;
        }
    }
}