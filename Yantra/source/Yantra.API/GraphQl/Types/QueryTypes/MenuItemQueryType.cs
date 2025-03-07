using Yantra.GraphQl.Query;
using Yantra.Infrastructure.Authentication;

namespace Yantra.GraphQl.Types.QueryTypes;

public class MenuItemQueryType : ObjectTypeExtension<MenuItemQuery>
{
    protected override void Configure(IObjectTypeDescriptor<MenuItemQuery> descriptor)
    {
        if (!AuthenticationSetup.EnableSecurity)
            return;
    }
}