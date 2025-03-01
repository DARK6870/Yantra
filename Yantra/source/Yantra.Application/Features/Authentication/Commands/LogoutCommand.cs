using System.Net;
using MediatR;
using Microsoft.AspNetCore.Http;
using Yantra.Application.Constants;
using Yantra.Infrastructure.Common.Exceptions;
using Yantra.Mongo.Repositories.Interfaces;

namespace Yantra.Application.Features.Authentication.Commands;

public record LogoutCommand : IRequest<bool>;

public class LogoutCommandHandler(
    IRefreshTokensRepository refreshTokensRepository,
    IHttpContextAccessor httpContextAccessor
) : IRequestHandler<LogoutCommand, bool>
{
    public async Task<bool> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var cookieRefreshToken = GetRefreshTokenFromCookie();
        await refreshTokensRepository.DeleteByRefreshTokenAsync(cookieRefreshToken);
        RemoveRefreshTokenFromCookie();

        return true;
    }

    private void RemoveRefreshTokenFromCookie()
    {
        httpContextAccessor.HttpContext?.Response.Cookies.Delete(CookieConstants.RefreshTokenCookieKey);
    }

    private string GetRefreshTokenFromCookie()
    {
         return httpContextAccessor.HttpContext?.Request.Cookies[CookieConstants.RefreshTokenCookieKey]
             ?? throw new ApiErrorException("Refresh Token cookie not found", HttpStatusCode.BadRequest);
    }
}