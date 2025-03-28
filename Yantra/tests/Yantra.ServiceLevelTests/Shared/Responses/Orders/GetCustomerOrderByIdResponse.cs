using System.Text.Json.Serialization;
using Yantra.Application.Responses;

namespace Yantra.ServiceLevelTests.Shared.Responses.Orders;

public record GetCustomerOrderByIdResponse(
    [property: JsonPropertyName("customerOrderById")]
    CustomerOrderResponse Order
);