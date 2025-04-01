using System.Runtime.CompilerServices;
using HotChocolate.Subscriptions;
using MediatR;
using Yantra.Application.Features.Orders.Queries;
using Yantra.Infrastructure.Common.Constants;
using Yantra.Mongo.Models.Entities;

namespace Yantra.GraphQl.Subscription;

[ExtendObjectType(nameof(Subscription))]
public class OrderSubscription
{
    public async IAsyncEnumerable<OrderEntity> OrderUpdatesStream(
        [Service] IMediator mediator,
        [Service] ITopicEventReceiver eventReceiver,
        [EnumeratorCancellation] CancellationToken cancellationToken
    )
    {
        var sourceStream = await eventReceiver.SubscribeAsync<string>(
            GraphQlConstants.OrderEventsTopicName,
            cancellationToken
        );

        await foreach (var id in sourceStream.ReadEventsAsync().WithCancellation(cancellationToken))
        {
            yield return mediator.Send(new GetOrderByIdQuery(id), cancellationToken).Result;
        }
    }

    [Subscribe(With = nameof(OrderUpdatesStream))]
    public OrderEntity SubscribeOnOrderUpdates(
        [EventMessage] OrderEntity message
    )
    {
        return message;
    }
}