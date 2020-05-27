using System;
using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace GameStore.DataAccess.Mongo
{
    public static class MongoHelpers
    {
        public static BsonDocument GetDocumentFilter<T>(Expression<Func<T, bool>> predicate)
        {
            if (predicate == null)
            {
                return new BsonDocument();
            }
            
            var serializerRegistry = BsonSerializer.SerializerRegistry;
            var serializer =  serializerRegistry.GetSerializer<T>();
            var filter = Builders<T>.Filter.Where(predicate);
            var result = filter.Render(serializer, serializerRegistry).AsBsonDocument;
            
            return result;
        }
    }
}