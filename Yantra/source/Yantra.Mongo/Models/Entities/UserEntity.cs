using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Yantra.Mongo.Common.Attributes;
using Yantra.Mongo.Models.Entities.Generic;
using Yantra.Mongo.Models.Enums;

namespace Yantra.Mongo.Models.Entities;

[MongoCollection("users")]
public class UserEntity : BaseEntity
{
    public required string UserName { get; set; }
    
    public required string Email { get; set; }
    
    public required string FirstName { get; set; }
    
    public required string LastName { get; set; }
    
    [BsonRepresentation(BsonType.String)]
    public Role Role { get; set; }
    
    public string? PasswordHash { get; set; }

    public string? SetPasswordToken { get; set; } = Guid.NewGuid().ToString();
    
    public DateTime CreateDate { get; set; } = DateTime.UtcNow;
    
    public DateTime UpdateDate { get; set; } = DateTime.UtcNow;
}