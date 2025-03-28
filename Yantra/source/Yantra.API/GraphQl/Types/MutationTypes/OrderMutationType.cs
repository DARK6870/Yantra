using Yantra.GraphQl.Mutation;
using Yantra.Infrastructure.Authentication;

namespace Yantra.GraphQl.Types.MutationTypes;

public class OrderMutationType : ObjectTypeExtension<OrderMutation>
{
    protected override void Configure(IObjectTypeDescriptor<OrderMutation> descriptor)
    {
        if (!AuthenticationSetup.EnableSecurity)
            return;
        
        descriptor
            .Field(x => x.CreateOrder(null!, null!, CancellationToken.None))
            .AllowAnonymous();
        
        descriptor
            .Field(x => x.UpdateOrder(null!, null!, CancellationToken.None))
            .Authorize(AuthenticationSetup.ManagerAccessPolicy);
        
        descriptor
            .Field(x => x.UpdateOrderStatus(null!, null!, CancellationToken.None))
            .Authorize(AuthenticationSetup.CourierAccessPolicy);
    }
}