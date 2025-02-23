using MongoDB.Driver;
using Yantra.Infrastructure.Repositories;
using Yantra.Mongo.Models.Entities;

namespace Yantra.Mongo.Repositories;

public class MenuItemsRepository(IMongoDatabase database)
    : GenericRepository<MenuItem>(database), IMenuItemsRepository
{
    
}