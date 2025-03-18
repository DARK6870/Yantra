using Yantra.GraphQl.Mutation;
using Yantra.Infrastructure.Authentication;

namespace Yantra.GraphQl.Types.MutationTypes;

public class SupportMutationType : ObjectTypeExtension<SupportMutation>
{
    protected override void Configure(IObjectTypeDescriptor<SupportMutation> descriptor)
    {
        if (!AuthenticationSetup.EnableSecurity)
            return;

        descriptor
            .Field(x => x.SubmitSupportRequest(null!, null!, CancellationToken.None))
            .AllowAnonymous();
    }
}