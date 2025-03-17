using Yantra.Mongo.Models.Entities;

namespace Yantra.ServiceLevelTests.Shared.Responses.Orders;

public record GetOrdersResponse(OffsetPaginatedResponse<OrderEntity> Orders);