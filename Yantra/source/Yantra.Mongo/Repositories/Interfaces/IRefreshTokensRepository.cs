using Yantra.Mongo.Models.Entities;

namespace Yantra.Mongo.Repositories.Interfaces;

public interface IRefreshTokensRepository : IGenericRepository<RefreshTokenEntity>
{
    Task DeleteByRefreshTokenAsync(string refreshToken);
}