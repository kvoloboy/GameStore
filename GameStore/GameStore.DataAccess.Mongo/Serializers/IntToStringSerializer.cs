using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace GameStore.DataAccess.Mongo.Serializers
{
    public class IntToStringSerializer : SerializerBase<string>
    {
        public override string Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var currentType = context.Reader.GetCurrentBsonType();

            var stringRepresentation = currentType switch
            {
                BsonType.String => context.Reader.ReadString(),
                BsonType.Int32 => context.Reader.ReadInt32().ToString(),
                _ => string.Empty
            };

            return stringRepresentation;
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, string value)
        {
            if (context.Writer.State == BsonWriterState.Value)
            {
                context.Writer.WriteInt32(!int.TryParse(value, out var intRepresentation)
                    ? default
                    : intRepresentation);
            }
        }
    }
}