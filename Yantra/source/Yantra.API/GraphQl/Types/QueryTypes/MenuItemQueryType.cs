using Yantra.GraphQl.Query;
using Yantra.Infrastructure.Authentication;

namespace Yantra.GraphQl.Types.QueryTypes;

public class MenuItemQueryType : ObjectTypeExtension<MenuItemQuery>
{
    protected override void Configure(IObjectTypeDescriptor<MenuItemQuery> descriptor)
    {
        if (!AuthenticationSetup.EnableSecurity)
            return;

        descriptor.Field(x => x.GetMenuItems(null!, CancellationToken.None))
            .AllowAnonymous();
        
        descriptor.Field(x => x.GetMenuItemByIdAsync(null!, null!, CancellationToken.None))
            .AllowAnonymous();
    }
}