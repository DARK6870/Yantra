using System.Net;
using MediatR;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver.Linq;
using Yantra.Application.Constants;
using Yantra.Application.Responses;
using Yantra.Infrastructure.Common.Exceptions;
using Yantra.Infrastructure.Services.Interfaces;
using Yantra.Mongo.Models.Entities;
using Yantra.Mongo.Repositories.Interfaces;

namespace Yantra.Application.Features.Authentication.Commands;

public record LoginCommand(
    string Email,
    string Password
) : IRequest<LoginResponse>;

public class LoginCommandHandler(
    IUsersRepository usersRepository,
    IRefreshTokensRepository refreshTokensRepository,
    IAuthenticationService authenticationService,
    IHttpContextAccessor httpContextAccessor
) : IRequestHandler<LoginCommand, LoginResponse>
{
    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await usersRepository.GetUserByCredentialsAsync(request.Email, request.Password)
                   ?? throw new ApiErrorException("Invalid credentials, please try again", HttpStatusCode.BadRequest);

        var accessToken = authenticationService.GenerateJwtToken(
            user.UserName,
            user.Email,
            user.Role.ToString()
        );

        var refreshToken = await refreshTokensRepository
            .AsQueryable()
            .FirstOrDefaultAsync(
                x => x.UserId == user.Id,
                cancellationToken: cancellationToken
            );

        if (refreshToken == null)
        {
            refreshToken = new RefreshTokenEntity
            {
                UserId = user.Id,
                Token = authenticationService.GenerateRefreshToken()
            };

            await refreshTokensRepository.InsertOneAsync(refreshToken, cancellationToken);
        }

        AddRefreshTokenToCookie(refreshToken.Token);
        return new LoginResponse(accessToken, refreshToken.Token);
    }

    private void AddRefreshTokenToCookie(string refreshToken)
    {
        httpContextAccessor.HttpContext?.Response.Cookies.Append(
            CookieConstants.RefreshTokenCookieKey,
            refreshToken,
            CookieConstants.CookieOptions
        );
    }
}