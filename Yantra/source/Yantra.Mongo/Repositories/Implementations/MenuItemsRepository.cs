using MongoDB.Driver;
using Yantra.Mongo.Models.Entities;
using Yantra.Mongo.Repositories.Interfaces;

namespace Yantra.Mongo.Repositories.Implementations;

public class MenuItemsRepository(
    IMongoDatabase database
) : GenericRepository<MenuItemEntity>(database), IMenuItemsRepository
{
}