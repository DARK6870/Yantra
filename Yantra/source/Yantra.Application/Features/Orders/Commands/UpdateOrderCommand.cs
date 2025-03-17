using System.Net;
using HotChocolate.Subscriptions;
using MediatR;
using Yantra.Application.Constants;
using Yantra.Infrastructure.Common.Constants;
using Yantra.Infrastructure.Common.Exceptions;
using Yantra.Mongo.Models;
using Yantra.Mongo.Models.Enums;
using Yantra.Mongo.Repositories.Interfaces;

namespace Yantra.Application.Features.Orders.Commands;

public record UpdateOrderCommand(
    string Id,
    string CustomerAddress,
    OrderStatus Status,
    List<OrderItem> OrderItems,
    string? CourierName = null,
    string? OrderDetails = null
) : IRequest<bool>;

public class UpdateOrderCommandHandler(
    IOrdersRepository ordersRepository,
    IUsersRepository usersRepository,
    ITopicEventSender eventSender
) : IRequestHandler<UpdateOrderCommand, bool>
{
    public async Task<bool> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await ordersRepository.FindByIdAsync(request.Id, cancellationToken)
                    ?? throw new ApiErrorException("Order not found", HttpStatusCode.NotFound);
        
        if (!string.IsNullOrEmpty(request.CourierName) && ! await usersRepository.ExistsAsync(x => x.UserName == request.CourierName, cancellationToken))
            throw new ApiErrorException("Courier not found", HttpStatusCode.NotFound);
        
        order.CustomerAddress = request.CustomerAddress;
        order.OrderDetails = request.OrderDetails;
        order.CourierName = request.CourierName;
        order.Status = request.Status;
        order.OrderItems = request.OrderItems;
        order.DateUpdated = DateTime.UtcNow;

        await ordersRepository.ReplaceOneAsync(order, cancellationToken);
        await eventSender.SendAsync(GraphQlConstants.OrderEventsTopicName, order.Id, cancellationToken);

        return true;
    }
}