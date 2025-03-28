using Yantra.Mongo.Models.Entities;

namespace Yantra.Mongo.Repositories.Interfaces;

public interface IUsersRepository : IGenericRepository<UserEntity>
{
    Task<UserEntity?> GetUserByCredentialsAsync(string email, string password);
    
    Task<bool> UpdatePasswordAsync(string email, string password);
    
    Task<UserEntity?> GetUserByEmailAsync(string email);
}