﻿using MongoDB.Driver;
using Yantra.Mongo.Common.Helpers;
using Yantra.Mongo.Models.Entities;
using Yantra.Mongo.Repositories.Interfaces;

namespace Yantra.Mongo.Repositories.Implementations;

public class UsersRepository(IMongoDatabase database)
    : GenericRepository<UserEntity>(database), IUsersRepository
{
    public async Task<UserEntity?> GetUserByCredentials(string email, string password)
    {
        var passwordHash = HashHelper.ComputeHash(password);

        var filter = Builders<UserEntity>
                         .Filter
                         .Eq(x => x.Email, email)
                     & Builders<UserEntity>
                         .Filter
                         .Eq(x => x.PasswordHash, passwordHash);

        return await Collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<bool> UpdatePassword(string email, string password)
    {
        var passwordHash = HashHelper.ComputeHash(password);
        
        var result = await Collection.UpdateOneAsync(
            x => x.Email == email,
            Builders<UserEntity>.Update.Set(x => x.PasswordHash, passwordHash)
        );

        return result.ModifiedCount > 0;
    }

    public async Task<UserEntity?> GetUserByEmail(string email)
    {
        return await Collection.Find(x => x.Email == email).FirstOrDefaultAsync();
    }
}