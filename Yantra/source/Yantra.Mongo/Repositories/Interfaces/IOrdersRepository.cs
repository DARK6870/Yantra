using Yantra.Mongo.Models.Entities;
using Yantra.Mongo.Models.Enums;

namespace Yantra.Mongo.Repositories.Interfaces;

public interface IOrdersRepository : IGenericRepository<OrderEntity>
{
    Task<bool> UpdateOrderStatusAsync(string id, OrderStatus status);
}