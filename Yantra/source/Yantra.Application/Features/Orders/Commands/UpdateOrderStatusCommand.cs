using System.Net;
using HotChocolate.Subscriptions;
using MediatR;
using Yantra.Application.Constants;
using Yantra.Infrastructure.Common.Constants;
using Yantra.Infrastructure.Common.Exceptions;
using Yantra.Infrastructure.Services.Interfaces;
using Yantra.Mongo.Models.Enums;
using Yantra.Mongo.Repositories.Interfaces;

namespace Yantra.Application.Features.Orders.Commands;

public record UpdateOrderStatusCommand(
    string Id,
    OrderStatus Status
) : IRequest<bool>;

public class UpdateOrderStatusCommandHandler(
    IOrdersRepository ordersRepository,
    IUserContext userContext,
    ITopicEventSender eventSender
) : IRequestHandler<UpdateOrderStatusCommand, bool>
{
    public async Task<bool> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await ordersRepository.FindByIdAsync(request.Id, cancellationToken)
                    ?? throw new ApiErrorException("Order not found", HttpStatusCode.NotFound);

        if (userContext.Role == Role.Courier && order.CourierName != userContext.UserName)
            throw new ApiErrorException("You do not have permission to update this order", HttpStatusCode.Forbidden);
        
        var result = await ordersRepository.UpdateOrderStatusAsync(request.Id, request.Status);
        await eventSender.SendAsync(GraphQlConstants.OrderEventsTopicName, order.Id, cancellationToken);

        return result;
    }
}