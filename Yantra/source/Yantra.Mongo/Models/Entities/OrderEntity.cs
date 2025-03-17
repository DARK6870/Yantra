using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Yantra.Mongo.Common.Attributes;
using Yantra.Mongo.Models.Entities.Generic;
using Yantra.Mongo.Models.Enums;

namespace Yantra.Mongo.Models.Entities;

[MongoCollection("orders")]
public class OrderEntity : BaseEntity
{
    public required string CustomerFullName { get; set; }
    
    public required string CustomerAddress { get; set; }
    
    public required string CustomerEmail { get; set; }
    
    public required string CustomerPhone { get; set; }

    public string? OrderDetails { get; set; } = string.Empty;

    public List<OrderItem> OrderItems { get; set; } = [];
    
    [BsonRepresentation(BsonType.String)]
    public OrderStatus Status { get; set; }
    
    public decimal DeliveryPrice { get; set; }
    
    public decimal TotalPrice { get; set; }
    
    public string? CourierName { get; set; }
    
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    
    public DateTime DateUpdated { get; set; } = DateTime.UtcNow;
}