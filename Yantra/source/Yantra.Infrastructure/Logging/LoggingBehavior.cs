using MediatR;
using Microsoft.Extensions.Logging;

namespace Yantra.Infrastructure.Logging;

public class LoggingBehavior<TRequest, TResponse>(
    ILogger<LoggingBehavior<TRequest, TResponse>> logger
) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        logger.LogDebug("Handling request: {RequestType}\nRequest: {@Request}", typeof(TRequest).Name, request);

        try
        {
            var response = await next();
            
            logger.LogDebug("Request {RequestType} proceed successfully", typeof(TRequest).Name);
            
            return response;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Request {RequestType} failed.\nRequest: {@Request}", typeof(TRequest).Name, request);
            throw;
        }
    }
}