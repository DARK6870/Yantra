using Yantra.GraphQl.Mutation;
using Yantra.Infrastructure.Authentication;

namespace Yantra.GraphQl.Types.MutationTypes;

public class UserMutationType : ObjectTypeExtension<UserMutation>
{
    protected override void Configure(IObjectTypeDescriptor<UserMutation> descriptor)
    {
        if (!AuthenticationSetup.EnableSecurity)
            return;

        descriptor.Authorize(AuthenticationSetup.AdminAccessPolicy);
    }
}