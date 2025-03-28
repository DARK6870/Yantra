using MongoDB.Driver;
using Yantra.Mongo.Models.Entities;
using Yantra.Mongo.Repositories.Interfaces;

namespace Yantra.Mongo.Repositories.Implementations;

public class RefreshTokensRepository(
    IMongoDatabase database
) : GenericRepository<RefreshTokenEntity>(database), IRefreshTokensRepository
{
    public async Task DeleteByRefreshTokenAsync(string refreshToken)
    {
        await Collection.DeleteOneAsync(x => x.Token == refreshToken);
    }
}