using Microsoft.AspNetCore.Http;

namespace Yantra.Application.Constants;

public static class CookieConstants
{
    public const string RefreshTokenCookieKey = "refreshToken";

    public static CookieOptions CookieOptions => new CookieOptions
    {
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.Strict,
        Expires = DateTime.UtcNow.AddDays(14)
    };
}