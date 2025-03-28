using System.Runtime.CompilerServices;
using HotChocolate.Subscriptions;
using MediatR;
using Yantra.Application.Constants;
using Yantra.Application.Features.Orders.Queries;
using Yantra.Infrastructure.Common.Constants;
using Yantra.Mongo.Models.Entities;

namespace Yantra.GraphQl.Subscription;

[ExtendObjectType(typeof(Subscription))]
public class OrderSubscription
{
    public async IAsyncEnumerable<OrderEntity> OnPublishedStream(
        [Service] IMediator mediator,
        [Service] ITopicEventReceiver eventReceiver,
        [EnumeratorCancellation] CancellationToken cancellationToken
    )
    {
        var sourceStream = await eventReceiver.SubscribeAsync<string>(GraphQlConstants.OrderEventsTopicName, cancellationToken);

        await foreach (var id in sourceStream.ReadEventsAsync().WithCancellation(cancellationToken))
        {
            yield return mediator.Send(new GetOrderByIdQuery(id), cancellationToken).Result;
        }
    }

    [Subscribe(With = nameof(OnPublishedStream))]
    public OrderEntity OnOrderUpdates(
        [EventMessage] OrderEntity message
    )
    {
        return message;
    }
}