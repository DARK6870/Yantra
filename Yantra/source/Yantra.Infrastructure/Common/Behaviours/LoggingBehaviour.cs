using MediatR;
using Microsoft.Extensions.Logging;

namespace Yantra.Infrastructure.Common.Behaviours;

public class LoggingBehavior<TRequest, TResponse>(
    ILogger<LoggingBehavior<TRequest, TResponse>> logger
) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        logger.LogDebug("Handling request: {requestType}", typeof(TRequest).Name);

        try
        {
            var response = await next();

            logger.LogDebug("Request {requestType} proceed successfully", typeof(TRequest).Name);

            return response;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Request {requestType} failed.\nRequest: {@request}", typeof(TRequest).Name, request);
            throw;
        }
    }
}