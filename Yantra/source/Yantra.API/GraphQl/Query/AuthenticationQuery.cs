using MediatR;
using Yantra.Application.Features.Authentication.Queries;

namespace Yantra.GraphQl.Query;

[ExtendObjectType(nameof(Query))]
public class AuthenticationQuery
{
    public async Task<string> RefreshAccessToken(
        [Service] IMediator mediator,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(new RefreshAccessTokenQuery(), cancellationToken);
    }
}