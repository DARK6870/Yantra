using System.Text.Json.Serialization;
using Yantra.Mongo.Models.Entities;

namespace Yantra.ServiceLevelTests.Shared.Responses.Orders;

public record OnOrderUpdatesResponse(
    [property: JsonPropertyName("onOrderUpdates")]
    OrderEntity Order
);