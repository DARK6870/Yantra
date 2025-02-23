using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
}