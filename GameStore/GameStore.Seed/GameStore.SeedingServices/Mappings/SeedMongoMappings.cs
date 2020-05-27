using GameStore.Core.Models;
using GameStore.DataAccess.Mongo.Serializers;
using GameStore.Infrastructure.Logging.Models;
using GameStore.SeedingServices.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;

namespace GameStore.SeedingServices.Mappings
{
    public static class SeedMongoMappings
    {
        public static void Configure()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(Genre)))
            {
                BsonClassMap.RegisterClassMap<Genre>(cm =>
                {
                    cm.MapMember(g => g.Id).SetElementName("CategoryID").SetSerializer(new IntToStringSerializer());
                    cm.MapMember(g => g.Name).SetElementName("CategoryName");
                    cm.SetIgnoreExtraElements(true);
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(Product)))
            {
                BsonClassMap.RegisterClassMap<Product>(cm =>
                {
                    cm.MapIdMember(g => g.Id);
                    cm.MapMember(g => g.ProductName);
                    cm.MapMember(g => g.CategoryId).SetElementName("CategoryID");
                    cm.MapMember(g => g.Key);
                    cm.SetIgnoreExtraElements(true);
                });
            }
            
            if (!BsonClassMap.IsClassMapRegistered(typeof(BaseLogEntry)))
            {
                BsonClassMap.RegisterClassMap<BaseLogEntry>(cm =>
                {
                    cm.AutoMap();
                    cm.MapIdMember(le => le.Id)
                        .SetIdGenerator(new StringObjectIdGenerator())
                        .SetSerializer(new StringSerializer(BsonType.ObjectId));
                    cm.SetIgnoreExtraElements(true);
                    cm.SetIgnoreExtraElementsIsInherited(true);
                });
            }
        }
    }
}