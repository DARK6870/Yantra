using Yantra.GraphQl.Query;
using Yantra.Infrastructure.Authentication;

namespace Yantra.GraphQl.Types.QueryTypes;

public class OrderQueryType : ObjectTypeExtension<OrderQuery>
{
    protected override void Configure(IObjectTypeDescriptor<OrderQuery> descriptor)
    {
        if (!AuthenticationSetup.EnableSecurity)
            return;
        
        descriptor
            .Field(x => x.GetCustomerOrderById(null!, null!, CancellationToken.None))
            .AllowAnonymous();

        descriptor
            .Field(x => x.GetOrders(null!))
            .Authorize();
        
        descriptor
            .Field(x => x.GetOrderById(null!, null!, CancellationToken.None))
            .Authorize();
    }
}