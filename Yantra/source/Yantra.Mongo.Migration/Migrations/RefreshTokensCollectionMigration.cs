using MongoDB.Driver;
using Yantra.Mongo.Common.Attributes;
using Yantra.Mongo.Models.Entities;

namespace Yantra.Mongo.Migration.Migrations;

public class RefreshTokensCollectionMigration(
    IMongoDatabase database
) : Core.Migration("RefreshTokensCollection: add TTL indexes")
{
    private readonly IMongoCollection<RefreshTokenEntity> _collection = database.GetCollection<RefreshTokenEntity>(
        MongoCollectionAttribute.GetCollectionName(typeof(RefreshTokenEntity))
    );

    public override async Task MigrateAsync()
    {
        var indexOptions = new CreateIndexOptions
        {
            ExpireAfter = TimeSpan.FromDays(14)
        };
        var indexKeys = Builders<RefreshTokenEntity>.IndexKeys.Ascending(x => x.DateCreated);
        
        await _collection.Indexes.CreateOneAsync(new CreateIndexModel<RefreshTokenEntity>(indexKeys, indexOptions));
    }
}