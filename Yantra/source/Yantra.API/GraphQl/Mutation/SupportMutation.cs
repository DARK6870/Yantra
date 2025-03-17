using HotChocolate.Authorization;
using MediatR;
using Yantra.Application.Features.Support.Commands;
using Yantra.Notifications.Builders;
using Yantra.Notifications.Services.Interfaces;

namespace Yantra.GraphQl.Mutation;

[ExtendObjectType(typeof(Mutation))]
public class SupportMutation
{
    [AllowAnonymous]
    public async Task<bool> SubmitSupportRequest(
        [Service] IMediator mediator,
        CreateSupportRequestCommand request,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(request, cancellationToken);
    }
}