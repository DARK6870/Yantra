using Yantra.GraphQl.Mutation;
using Yantra.Infrastructure.Authentication;

namespace Yantra.GraphQl.Types.MutationTypes;

public class AuthenticationMutationType : ObjectTypeExtension<AuthenticationMutation>
{
    protected override void Configure(IObjectTypeDescriptor<AuthenticationMutation> descriptor)
    {
        if (!AuthenticationSetup.EnableSecurity)
            return;
    }
}