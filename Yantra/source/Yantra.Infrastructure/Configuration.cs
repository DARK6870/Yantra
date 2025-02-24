using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Yantra.Infrastructure.Logging;

namespace Yantra.Infrastructure;

public static class Configuration
{
    public static IServiceCollection AddConfiguration(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        return services;
    }
    
    public static IServiceCollection AddLoggingBehavior(
        this IServiceCollection services
    )
    {
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        
        return services;
    }
}