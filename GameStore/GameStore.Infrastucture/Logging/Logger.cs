using GameStore.Infrastructure.DatabaseSettings.Interfaces;
using GameStore.Infrastructure.Logging.Interfaces;
using GameStore.Infrastructure.Logging.Models;
using MongoDB.Driver;

namespace GameStore.Infrastructure.Logging
{
    public class Logger : ILogger
    {
        private readonly IMongoCollection<BaseLogEntry> _logsCollection;

        public Logger(IMongoDatabaseSettings<BaseLogEntry> mongoDatabaseSettings, IMongoClient mongoClient)
        {
            var databaseName = mongoDatabaseSettings.GetDatabaseName();
            var collectionName = mongoDatabaseSettings.GetCollectionName();

            var database = mongoClient.GetDatabase(databaseName);
            _logsCollection = database.GetCollection<BaseLogEntry>(collectionName);
        }
        
        public void Log<TEntity>(LogEntry<TEntity> logEntry)
        {
            _logsCollection.InsertOne(logEntry);
        }
    }
}