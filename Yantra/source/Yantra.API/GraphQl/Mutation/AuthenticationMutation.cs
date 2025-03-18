using MediatR;
using Yantra.Application.Features.Authentication.Commands;
using Yantra.Application.Responses;

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
    
    public async Task<LoginResponse> Login(
        [Service] IMediator mediator,
        LoginCommand request,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(request, cancellationToken);
    }

    public async Task<bool> Logout(
        [Service] IMediator mediator,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(new LogoutCommand(), cancellationToken);
    }
}