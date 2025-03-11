using System.Net;
using HotChocolate.Subscriptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver.Linq;
using Yantra.Application.Constants;
using Yantra.Infrastructure.Common.Constants;
using Yantra.Infrastructure.Common.Exceptions;
using Yantra.Infrastructure.Common.Extensions;
using Yantra.Mongo.Models;
using Yantra.Mongo.Models.Entities;
using Yantra.Mongo.Models.Enums;
using Yantra.Mongo.Repositories.Interfaces;
using Yantra.Notifications.Builders;
using Yantra.Notifications.Models;
using Yantra.Notifications.Services.Interfaces;

namespace Yantra.Application.Features.Orders.Commands;

public record CreateOrderCommand(
    string CustomerFullName,
    string CustomerAddress,
    string CustomerEmail,
    string CustomerPhone,
    string? OrderDetails,
    List<OrderItem> OrderItems,
    decimal DeliveryPrice
) : IRequest<bool>;

public class CreateOrderCommandHandler(
    IOrdersRepository ordersRepository,
    IMenuItemsRepository menuItemRepository,
    IHttpContextAccessor httpContextAccessor,
    INotificationService notificationService,
    ITopicEventSender eventSender
) : IRequestHandler<CreateOrderCommand, bool>
{
    public async Task<bool> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var menuItems = await menuItemRepository
            .AsQueryable()
            .Where(x => request.OrderItems.Select(oi => oi.ItemName).Contains(x.Name))
            .ToListAsync(cancellationToken);

        var order = new OrderEntity()
        {
            CustomerFullName = request.CustomerFullName,
            CustomerAddress = request.CustomerAddress,
            CustomerEmail = request.CustomerEmail,
            CustomerPhone = request.CustomerPhone,
            OrderDetails = request.OrderDetails,
            OrderItems = request.OrderItems,
            DeliveryPrice = request.DeliveryPrice,
            Status = OrderStatus.Pending,
            TotalPrice = request.OrderItems.Sum(orderItem =>
            {
                var menuItem = menuItems.FirstOrDefault(mi => mi.Name == orderItem.ItemName)
                    ?? throw new ApiErrorException($"Menu item '{orderItem.ItemName}' does not exist.", HttpStatusCode.BadRequest);
        
                return menuItem.Price * orderItem.Quantity;
            }) + request.DeliveryPrice
        };


        await ordersRepository.InsertOneAsync(order, cancellationToken);
        await eventSender.SendAsync(GraphQlConstants.OrderEventsTopicName, order.Id, cancellationToken);
        await SendOrderCreatedEmailAsync(order);

        return true;
    }

    private async Task SendOrderCreatedEmailAsync(OrderEntity order)
    {
        var hostUrl = httpContextAccessor.GetHostUrl();
        var email = new MessageBuilder
        {
            Title = "Your order has been accepted",
            FullName = order.CustomerFullName,
            Message =
                "We have received your order in processing, within 10-15 minutes your manager will call you to confirm your order. All details of the order you can see at the link below.",
            ActionUrl = hostUrl + HttpConstants.OrderDetailsPath(order.Id),
            ActionText = "to view order details"
        };

        await notificationService.SendEmailNotification(
            order.CustomerEmail,
            NotificationSubjects.OrderAccepted,
            email
        );
    }
}