using Microsoft.AspNetCore.Http;

namespace Yantra.Infrastructure.Common.Extensions;

public static class HttpContextAccessorExtension
{
    public static string GetHostUrl(this IHttpContextAccessor httpContextAccessor)
    {
        var context = httpContextAccessor.HttpContext
                      ?? throw new InvalidOperationException("HttpContext is null.");
        
        var request = context.Request
                      ?? throw new InvalidOperationException("HttpRequest is null.");

        
        return $"{request.Scheme}://{request.Host.Value}";
    }
}