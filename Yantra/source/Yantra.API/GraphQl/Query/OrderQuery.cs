using HotChocolate.Authorization;
using MediatR;
using Yantra.Application.Features.Orders.Queries;
using Yantra.Application.Responses;
using Yantra.Infrastructure.Common.Constants;
using Yantra.Mongo.Models.Entities;

namespace Yantra.GraphQl.Query;

[ExtendObjectType(typeof(Query))]
public class OrderQuery
{
    [UseOffsetPaging(
        ProviderName = GraphQlConstants.QueryablePaginationProvider,
        MaxPageSize = GraphQlConstants.MaxPageSize,
        DefaultPageSize = GraphQlConstants.DefaultPageSize,
        IncludeTotalCount = true
    )]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<OrderEntity> GetOrders(
        [Service] IMediator mediator
    )
    {
        return mediator.Send(new GetOrdersQuery()).Result;
    }

    public async Task<OrderEntity> GetOrderById(
        [Service] IMediator mediator,
        string id,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(new GetOrderByIdQuery(id), cancellationToken);
    }

    [UseProjection]
    [AllowAnonymous]
    public async Task<CustomerOrderResponse> GetCustomerOrderById(
        [Service] IMediator mediator,
        string id,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(new GetCustomerOrderByIdQuery(id), cancellationToken);
    }
}