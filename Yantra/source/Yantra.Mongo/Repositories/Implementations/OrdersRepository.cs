using MongoDB.Driver;
using Yantra.Mongo.Models.Entities;
using Yantra.Mongo.Models.Enums;
using Yantra.Mongo.Repositories.Interfaces;

namespace Yantra.Mongo.Repositories.Implementations;

public class OrdersRepository(
    IMongoDatabase database
) : GenericRepository<OrderEntity>(database), IOrdersRepository
{
    public async Task<bool> UpdateOrderStatusAsync(string id, OrderStatus status)
    {
        var result = await Collection.UpdateOneAsync(
            x => x.Id == id,
            Builders<OrderEntity>.Update
                .Set(x => x.Status, status)
                .Set(x => x.DateUpdated, DateTime.UtcNow)
        );

        return result.ModifiedCount > 0; 
    }
}