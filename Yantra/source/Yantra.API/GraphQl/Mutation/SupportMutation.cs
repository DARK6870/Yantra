using HotChocolate.Authorization;
using MediatR;
using Yantra.Application.Features.Support.Commands;

namespace Yantra.GraphQl.Mutation;

[ExtendObjectType(typeof(Mutation))]
public class SupportMutation
{
    public async Task<bool> SubmitSupportRequest(
        [Service] IMediator mediator,
        CreateSupportRequestCommand request,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(request, cancellationToken);
    }
}