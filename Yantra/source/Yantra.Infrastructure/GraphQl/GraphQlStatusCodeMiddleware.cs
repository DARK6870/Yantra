using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Yantra.Infrastructure.Models;
using Yantra.Infrastructure.SerializerConfiguration;

namespace Yantra.Infrastructure.GraphQl;

public class GraphQlStatusCodeMiddleware(
    RequestDelegate next,
    ILogger<GraphQlStatusCodeMiddleware> logger
)
{
    private static readonly Dictionary<string, int> _errorStatusCodes = new()
    {
        { "AUTH_NOT_AUTHORIZED", StatusCodes.Status403Forbidden },
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
        var responseBody = await new StreamReader(memoryStream).ReadToEndAsync();

        if (responseBody.Contains("\"errors\""))
        {
            try
            {
                var errorResponse = JsonSerializer
                    .Deserialize<ErrorResponse>(responseBody, JsonSerializerConfiguration.JsonSerializerOptions);

                var code = errorResponse?.Errors?
                    .FirstOrDefault()?.Extensions?.Code;

                if (code != null)
                {
                    if (int.TryParse(code, out var parsedCode))
                    {
                        context.Response.StatusCode = parsedCode;
                    }
                    else if (_errorStatusCodes.TryGetValue(code, out var dictionaryCode))
                    {
                        context.Response.StatusCode = dictionaryCode;
                    }
                }
            }
            catch (JsonException ex)
            {
                logger.LogError("Failed to deserialize result. Message: {message}.", ex.Message);
            }
        }

        memoryStream.Seek(0, SeekOrigin.Begin);
        await memoryStream.CopyToAsync(originalBodyStream);
    }
}