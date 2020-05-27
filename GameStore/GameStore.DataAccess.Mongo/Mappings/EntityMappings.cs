using GameStore.Core.Models;
using GameStore.DataAccess.Mongo.Models;
using GameStore.DataAccess.Mongo.Serializers;
using GameStore.Infrastructure.Logging.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;
using DateTimeSerializer = GameStore.DataAccess.Mongo.Serializers.DateTimeSerializer;
using Publisher = GameStore.DataAccess.Mongo.Models.Publisher;

namespace GameStore.DataAccess.Mongo.Mappings
{
    public static class EntityMappings
    {
        public static void Configure()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(Product)))
            {
                BsonClassMap.RegisterClassMap<Product>(cm =>
                {
                    cm.AutoMap();
                    cm.UnmapMember(p => p.Id);
                    cm.MapMember(p => p.Id).SetElementName("ProductID").SetSerializer(new IntToStringSerializer());
                    cm.MapMember(g => g.SupplierId).SetElementName("SupplierID")
                        .SetSerializer(new IntToStringSerializer());
                    cm.MapMember(g => g.Discontinued).SetSerializer(new BooleanSerializer());
                    cm.MapMember(g => g.UnitPrice).SetSerializer(new DecimalSerializer(BsonType.Double));
                    cm.SetIgnoreExtraElements(true);
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(Shipper)))
            {
                BsonClassMap.RegisterClassMap<Shipper>(cm =>
                {
                    cm.AutoMap();
                    cm.UnmapMember(p => p.Id);
                    cm.MapMember(s => s.Id).SetElementName("ShipperID").SetSerializer(new IntToStringSerializer());
                    cm.SetIgnoreExtraElements(true);
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(Publisher)))
            {
                BsonClassMap.RegisterClassMap<Publisher>(cm =>
                {
                    cm.AutoMap();
                    cm.UnmapMember(p => p.Id);
                    cm.MapMember(p => p.Id).SetElementName("SupplierID").SetSerializer(new IntToStringSerializer());
                    cm.MapMember(p => p.PostalCode).SetSerializer(new IntToStringSerializer());
                    cm.MapMember(p => p.Country).SetSerializer(new IntToStringSerializer());
                    cm.MapMember(p => p.Address).SetSerializer(new IntToStringSerializer());
                    cm.MapMember(p => p.City).SetSerializer(new IntToStringSerializer());
                    cm.MapMember(p => p.Fax).SetSerializer(new IntToStringSerializer());
                    cm.MapMember(p => p.Phone).SetSerializer(new IntToStringSerializer());
                    cm.SetIgnoreExtraElements(true);
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(Genre)))
            {
                BsonClassMap.RegisterClassMap<Genre>(cm =>
                {
                    cm.MapMember(g => g.Id).SetElementName("CategoryID");
                    cm.MapMember(g => g.Name).SetElementName("CategoryName");
                    cm.SetIgnoreExtraElements(true);
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(OrderDetails)))
            {
                BsonClassMap.RegisterClassMap<OrderDetails>(cm =>
                {
                    cm.AutoMap();
                    cm.UnmapMember(o => o.Id);
                    cm.MapMember(od => od.Price).SetElementName("UnitPrice");
                    cm.MapMember(od => od.GameRootId).SetElementName("ProductID")
                        .SetSerializer(new IntToStringSerializer());
                    cm.MapMember(od => od.OrderId).SetElementName("OrderID").SetSerializer(new IntToStringSerializer());
                    cm.SetIgnoreExtraElements(true);
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(Order)))
            {
                BsonClassMap.RegisterClassMap<Order>(cm =>
                {
                    cm.AutoMap();
                    cm.UnmapMember(o => o.Id);
                    cm.MapMember(o => o.Id).SetElementName("OrderID").SetSerializer(new IntToStringSerializer());
                    cm.MapMember(o => o.UserId).SetElementName("CustomerID");
                    cm.MapMember(o => o.Name).SetElementName("ShipName");
                    cm.MapMember(o => o.ShipperEntityId).SetElementName("ShipVia").SetSerializer(new IntToStringSerializer());
                    cm.MapMember(o => o.Region).SetElementName("ShipRegion");
                    cm.MapMember(o => o.PostalCode).SetElementName("ShipPostalCode")
                        .SetSerializer(new IntToStringSerializer());
                    cm.MapMember(o => o.Country).SetElementName("ShipCountry")
                        .SetSerializer(new IntToStringSerializer());
                    cm.MapMember(o => o.City).SetElementName("ShipCity").SetSerializer(new IntToStringSerializer());
                    cm.MapMember(o => o.Address).SetElementName("ShipAddress")
                        .SetSerializer(new IntToStringSerializer());
                    cm.MapMember(o => o.OrderDate).SetSerializer(new DateTimeSerializer());
                    cm.MapMember(o => o.RequiredDate).SetSerializer(new DateTimeSerializer());
                    cm.MapMember(o => o.ShippedDate).SetSerializer(new DateTimeSerializer());
                    cm.MapMember(o => o.State).SetDefaultValue(OrderState.Closed);
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