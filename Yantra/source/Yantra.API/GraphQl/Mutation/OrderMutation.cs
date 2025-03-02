using MediatR;
using Yantra.Application.Features.Orders.Commands;

namespace Yantra.GraphQl.Mutation;

[ExtendObjectType(typeof(Mutation))]
public class OrderMutation
{
    public async Task<bool> CreateOrder(
        [Service] IMediator mediator,
        CreateOrderCommand request,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(request, cancellationToken);
    }
}