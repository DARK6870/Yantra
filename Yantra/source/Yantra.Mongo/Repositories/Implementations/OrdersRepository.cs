using MongoDB.Driver;
using Yantra.Mongo.Models.Entities;
using Yantra.Mongo.Repositories.Interfaces;

namespace Yantra.Mongo.Repositories.Implementations;

public class OrdersRepository(
    IMongoDatabase database
) : GenericRepository<OrderEntity>(database), IOrdersRepository
{
    
}