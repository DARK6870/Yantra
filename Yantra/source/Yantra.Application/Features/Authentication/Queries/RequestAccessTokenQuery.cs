using System.Net;
using MediatR;
using Yantra.Infrastructure.Common.Exceptions;
using Yantra.Infrastructure.Services.Interfaces;
using Yantra.Mongo.Repositories.Interfaces;

namespace Yantra.Application.Features.Authentication.Queries;

public record RequestAccessTokenQuery(
    string Email,
    string Password
) : IRequest<string>;

public class RequestAccessTokenQueryHandler(
    IUsersRepository repository,
    IAuthenticationService authenticationService
) : IRequestHandler<RequestAccessTokenQuery, string>
{
    public async Task<string> Handle(RequestAccessTokenQuery request, CancellationToken cancellationToken)
    {
        var user = await repository.GetUserByCredentialsAsync(request.Email, request.Password)
                   ?? throw new ApiErrorException("Invalid credentials, please try again", HttpStatusCode.BadRequest);

        return authenticationService.GenerateJwtToken(
            user.UserName,
            user.Email,
            user.Role.ToString()
        );
    }
}