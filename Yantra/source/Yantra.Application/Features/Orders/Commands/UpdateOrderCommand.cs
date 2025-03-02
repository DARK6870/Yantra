using MediatR;
using Yantra.Mongo.Models.Enums;

namespace Yantra.Application.Features.Orders.Commands;

public record UpdateOrderCommand(
    string Id,
    string CustomerAddress,
    string CustomerEmail,
    OrderStatus Status
) : IRequest<bool>;

public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, bool>
{
    public Task<bool> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}