using HotChocolate.Authorization;
using MediatR;
using Yantra.Application.Features.Orders.Commands;

namespace Yantra.GraphQl.Mutation;

[ExtendObjectType(typeof(Mutation))]
public class OrderMutation
{
    [AllowAnonymous]
    public async Task<bool> CreateOrder(
        [Service] IMediator mediator,
        CreateOrderCommand request,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(request, cancellationToken);
    }

    public async Task<bool> UpdateOrder(
        [Service] IMediator mediator,
        UpdateOrderCommand request,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(request, cancellationToken);
    }

    public async Task<bool> UpdateOrderStatus(
        [Service] IMediator mediator,
        UpdateOrderStatusCommand request,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(request, cancellationToken);
    }
}