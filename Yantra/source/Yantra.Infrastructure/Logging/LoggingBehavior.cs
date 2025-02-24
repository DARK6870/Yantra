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
        logger.LogDebug("Starting request: {request}", typeof(TRequest).Name);

        try
        {
            var response = await next();
            
            logger.LogDebug("Completed request: {request}", typeof(TRequest).Name);
            
            return response;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Request {request} failed.", typeof(TRequest).Name);
            throw;
        }
    }
}