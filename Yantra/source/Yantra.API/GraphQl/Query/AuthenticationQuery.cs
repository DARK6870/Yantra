using MediatR;
using Yantra.Application.Features.Authentication.Commands;
using Yantra.Application.Features.Authentication.Queries;

namespace Yantra.GraphQl.Query;

[ExtendObjectType(typeof(Query))]
public class AuthenticationQuery
{
    public async Task<string> RequestAccessToken(
        [Service] IMediator mediator,
        RequestAccessTokenQuery request,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(request, cancellationToken);
    }

    public async Task<bool> SetPassword(
        [Service] IMediator mediator,
        SetPasswordCommand request,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(request, cancellationToken);
    }

    public async Task<bool> UpdatePassword(
        [Service] IMediator mediator,
        UpdatePasswordCommand request,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(request, cancellationToken);
    }
}