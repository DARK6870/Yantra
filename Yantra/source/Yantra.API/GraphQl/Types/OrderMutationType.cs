using Yantra.GraphQl.Mutation;
using Yantra.Infrastructure.Authentication;

namespace Yantra.GraphQl.Types;

public class OrderMutationType : ObjectTypeExtension<OrderMutation>
{
    protected override void Configure(IObjectTypeDescriptor<OrderMutation> descriptor)
    {
        if (!AuthenticationSetup.EnableSecurity)
            return;
    }
}