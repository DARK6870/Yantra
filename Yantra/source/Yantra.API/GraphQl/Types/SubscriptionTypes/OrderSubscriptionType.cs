using Yantra.GraphQl.Subscription;
using Yantra.Infrastructure.Authentication;

namespace Yantra.GraphQl.Types.SubscriptionTypes;

public class OrderSubscriptionType : ObjectTypeExtension<OrderSubscription>
{
    protected override void Configure(IObjectTypeDescriptor<OrderSubscription> descriptor)
    {
        if (!AuthenticationSetup.EnableSecurity)
            return;

        descriptor
            .Field(x => x.OnOrderUpdates(null!))
            .Authorize();
    }
}