﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Yantra.Mongo.Models.Entities.Generic;

public class BaseEntity : IEntity
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();
}