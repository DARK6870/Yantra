using Yantra.Mongo.Common.Attributes;
using Yantra.Mongo.Models.Entities.Generic;

namespace Yantra.Mongo.Models.Entities;

[MongoCollection("refreshTokens")]
public class RefreshTokenEntity : BaseEntity
{
    public required string UserId { get; set; }
    public required string Token { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    public DateTime DateExpired { get; set; } = DateTime.UtcNow.AddDays(14);
}