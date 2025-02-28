using Yantra.GraphQl.Mutation;
using Yantra.Infrastructure.Authentication;

namespace Yantra.GraphQl.Types;

public class AuthenticationMutationType : ObjectTypeExtension<AuthenticationMutation>
{
    protected override void Configure(IObjectTypeDescriptor<AuthenticationMutation> descriptor)
    {
        if (!AuthenticationSetup.EnableSecurity)
            return;

        descriptor
            .Field(x => x.SetPassword(null!, null!, CancellationToken.None))
            .Authorize();
        
        descriptor
            .Field(x => x.ChangePassword(null!, null!, CancellationToken.None))
            .Authorize();
    }
}