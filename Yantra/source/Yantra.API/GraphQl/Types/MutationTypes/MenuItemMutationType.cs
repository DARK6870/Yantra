using Yantra.GraphQl.Mutation;
using Yantra.Infrastructure.Authentication;

namespace Yantra.GraphQl.Types.MutationTypes;

public class MenuItemMutationType : ObjectTypeExtension<MenuItemMutation>
{
    protected override void Configure(IObjectTypeDescriptor<MenuItemMutation> descriptor)
    {
        if (!AuthenticationSetup.EnableSecurity)
            return;
        
        descriptor.Authorize();
        
        descriptor.Field(x => x.AddMenuItem(null!, null!, CancellationToken.None))
            .Authorize(AuthenticationSetup.AdminAccessPolicy);
        
        descriptor.Field(x => x.UpdateMenuItem(null!, null!, CancellationToken.None))
            .Authorize(AuthenticationSetup.AdminAccessPolicy);
        
        descriptor.Field(x => x.DeleteMenuItem(null!, null!, CancellationToken.None))
            .Authorize(AuthenticationSetup.AdminAccessPolicy);
    }
}