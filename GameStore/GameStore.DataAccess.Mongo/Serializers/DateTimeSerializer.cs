using System;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace GameStore.DataAccess.Mongo.Serializers
{
    public class DateTimeSerializer : SerializerBase<DateTime>
    {
        public override DateTime Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var stringDateRepresentation = context.Reader.ReadString();
            var result = DateTime.TryParse(stringDateRepresentation, out var dateTime) ? dateTime : default;
            
            return result;
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, DateTime value)
        {
            var stringDateRepresentation = value.ToString("yyyy-MM-dd hh:mm:ss.fff");
            context.Writer.WriteString(stringDateRepresentation);
        }
    }
}