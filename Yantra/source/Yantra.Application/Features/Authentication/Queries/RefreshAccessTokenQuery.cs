using System.Net;
using MediatR;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver.Linq;
using Yantra.Application.Constants;
using Yantra.Infrastructure.Common.Exceptions;
using Yantra.Infrastructure.Services.Interfaces;
using Yantra.Mongo.Repositories.Interfaces;

namespace Yantra.Application.Features.Authentication.Queries;

public record RefreshAccessTokenQuery : IRequest<string>;

public class RefreshAccessTokenQueryHandler(
    IRefreshTokensRepository refreshTokensRepository,
    IUsersRepository usersRepository,
    IAuthenticationService authenticationService,
    IHttpContextAccessor httpContextAccessor
) : IRequestHandler<RefreshAccessTokenQuery, string>
{
    public async Task<string> Handle(RefreshAccessTokenQuery refresh, CancellationToken cancellationToken)
    {
        var refreshToken = await refreshTokensRepository.AsQueryable()
            .FirstOrDefaultAsync(
                x => x.Token == GetRefreshTokenFromCookie(),
                cancellationToken: cancellationToken
            ) ?? throw new ApiErrorException("Refresh access token not found", HttpStatusCode.NotFound);

        var user = await usersRepository.FindByIdAsync(refreshToken.UserId, cancellationToken)
                   ?? throw new ApiErrorException("User not found", HttpStatusCode.NotFound);

        return authenticationService.GenerateJwtToken(
            user.UserName,
            user.Email,
            user.Role.ToString()
        );
    }

    private string GetRefreshTokenFromCookie()
    {
        return httpContextAccessor.HttpContext?.Request.Cookies[CookieConstants.RefreshTokenCookieKey]
               ?? throw new ApiErrorException("Refresh Token cookie not found", HttpStatusCode.BadRequest);
    }
}