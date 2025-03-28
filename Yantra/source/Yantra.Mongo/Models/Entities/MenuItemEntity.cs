using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Yantra.Mongo.Common.Attributes;
using Yantra.Mongo.Models.Entities.Generic;
using Yantra.Mongo.Models.Enums;

namespace Yantra.Mongo.Models.Entities;

[MongoCollection("menuItems")]
public class MenuItemEntity : BaseEntity
{
    public required string Name { get; set; }
    
    public required string Description { get; set; }
    
    public required string Image { get; set; }
    
    [BsonRepresentation(BsonType.String)]
    public required ItemType Type { get; set; }
    
    public decimal Price { get; set; }
    
    public DateTime DateUpdated { get; set; } = DateTime.UtcNow;
}