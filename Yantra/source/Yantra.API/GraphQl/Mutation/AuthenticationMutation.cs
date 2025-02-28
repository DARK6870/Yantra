using MediatR;
using Yantra.Application.Features.Authentication.Commands;

namespace Yantra.GraphQl.Mutation;

[ExtendObjectType(typeof(Mutation))]
public class AuthenticationMutation
{
    public async Task<bool> SetPassword(
        [Service] IMediator mediator,
        SetPasswordCommand request,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(request, cancellationToken);
    }

    public async Task<bool> ChangePassword(
        [Service] IMediator mediator,
        ChangePasswordCommand request,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(request, cancellationToken);
    }
}