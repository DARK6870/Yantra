using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Yantra.Infrastructure.Configurations;
using Yantra.Infrastructure.Models;

namespace Yantra.Infrastructure.GraphQl;

public class GraphQlStatusCodeMiddleware(
    RequestDelegate next,
    ILogger<GraphQlStatusCodeMiddleware> logger
)
{
    private static readonly Dictionary<string, int> ErrorStatusCodes = new()
    {
        { "AUTH_NOT_AUTHORIZED", StatusCodes.Status403Forbidden },
        { "AUTH_NOT_AUTHENTICATED", StatusCodes.Status401Unauthorized },
        { "AUTH_UNAUTHENTICATED", StatusCodes.Status401Unauthorized },
        { "RESOURCE_NOT_FOUND", StatusCodes.Status404NotFound },
        { "INVALID_INPUT", StatusCodes.Status400BadRequest },
        { "INTERNAL_ERROR", StatusCodes.Status500InternalServerError }
    };

    public async Task InvokeAsync(HttpContext context)
    {
        var originalBodyStream = context.Response.Body;

        using var memoryStream = new MemoryStream();
        context.Response.Body = memoryStream;

        await next(context);

        memoryStream.Seek(0, SeekOrigin.Begin);
        var buffer = new byte[memoryStream.Length];
        await memoryStream.ReadExactlyAsync(buffer, 0, buffer.Length);

        byte[] errorsPattern = Encoding.UTF8.GetBytes("\"errors\"");
        if (buffer.AsSpan().IndexOf(errorsPattern) >= 0)
        {
            try
            {
                var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(buffer, JsonSerializerConfiguration.JsonSerializerOptions);
                var code = errorResponse?.Errors.FirstOrDefault()?.Extensions?.Code;

                if (code != null)
                {
                    if (int.TryParse(code, out var parsedCode))
                    {
                        context.Response.StatusCode = parsedCode;
                    }
                    else if (ErrorStatusCodes.TryGetValue(code, out var dictionaryCode))
                    {
                        context.Response.StatusCode = dictionaryCode;
                    }
                }
            }
            catch (JsonException ex)
            {
                logger.LogError("Failed to deserialize result. Message: {Message}.", ex.Message);
            }
        }

        memoryStream.Seek(0, SeekOrigin.Begin);
        await memoryStream.CopyToAsync(originalBodyStream);
    }
}