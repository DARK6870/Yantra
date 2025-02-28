using Yantra.GraphQl.Query;
using Yantra.Infrastructure.Authentication;

namespace Yantra.GraphQl.Types;

public class UserQueryType : ObjectTypeExtension<UserQuery>
{
    protected override void Configure(IObjectTypeDescriptor<UserQuery> descriptor)
    {
        if (!AuthenticationSetup.EnableSecurity)
            return;
        
        descriptor.Authorize(AuthenticationSetup.AdminAccessPolicy);
    }
}