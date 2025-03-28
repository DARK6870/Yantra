using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Yantra.Mongo.Models.Entities.Generic;

public interface IEntity
{
    [BsonRepresentation(BsonType.ObjectId)]
    string Id { get; set; }
}