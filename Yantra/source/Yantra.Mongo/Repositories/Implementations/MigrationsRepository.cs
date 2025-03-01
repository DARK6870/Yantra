using MongoDB.Driver;
using Yantra.Mongo.Models.Entities;
using Yantra.Mongo.Repositories.Interfaces;

namespace Yantra.Mongo.Repositories.Implementations;

public class MigrationsRepository(
    IMongoDatabase database
) : GenericRepository<MigrationEntity>(database), IMigrationsRepository
{
    
}