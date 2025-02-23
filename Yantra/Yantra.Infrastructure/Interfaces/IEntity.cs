using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Yantra.Infrastructure.Interfaces;

public interface IEntity
{
    [BsonRepresentation(BsonType.ObjectId)]
    string Id { get; set; }
}