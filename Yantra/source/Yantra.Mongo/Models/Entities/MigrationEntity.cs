using Yantra.Mongo.Common.Attributes;
using Yantra.Mongo.Models.Entities.Generic;

namespace Yantra.Mongo.Models.Entities;

[MongoCollection("_migrations")]
public class MigrationEntity(string description) : BaseEntity
{
    public string Description { get; set; } = description;

    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
}