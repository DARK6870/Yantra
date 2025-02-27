using Yantra.Mongo.Models.Entities;

namespace Yantra.Mongo.Repositories.Interfaces;

public interface IUsersRepository : IGenericRepository<UserEntity>
{
    Task<UserEntity?> GetUserByCredentials(string email, string password);
    
    Task<bool> UpdatePassword(string email, string password);
    
    Task<UserEntity?> GetUserByEmail(string email);
}