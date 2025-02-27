using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Yantra.Application;

public static class Configuration
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services
    )
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(AssemblyMarker).Assembly)
        );
        
        services.AddValidatorsFromAssembly(typeof(AssemblyMarker).Assembly);
        
        return services;
    }
}