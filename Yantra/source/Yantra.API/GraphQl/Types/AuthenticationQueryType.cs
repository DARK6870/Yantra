using Yantra.GraphQl.Query;
using Yantra.Infrastructure.Authentication;

namespace Yantra.GraphQl.Types;

public class AuthenticationQueryType : ObjectTypeExtension<AuthenticationQuery>
{
    protected override void Configure(IObjectTypeDescriptor<AuthenticationQuery> descriptor)
    {
        if (!AuthenticationSetup.EnableSecurity)
            return;
    }
}