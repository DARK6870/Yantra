using System.Text;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Yantra.Infrastructure.Common.Behaviours;
using Yantra.Infrastructure.Options;
using Yantra.Infrastructure.Services.Implementations;
using Yantra.Infrastructure.Services.Interfaces;
using Yantra.Mongo.Migration.Core;

namespace Yantra.Infrastructure;

public static class Configuration
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services
    )
    {
        services.AddHttpContextAccessor();
        
        services
            .AddSingleton<IAuthenticationService, AuthenticationService>()
            .AddScoped<IUserContext, UserContext>()
            .AddMemoryCache()
            ;

        return services;
    }

    public static IServiceCollection AddConfiguration(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.Configure<JwtOptions>(configuration.GetSection("AuthenticationOptions:JwtOptions"));

        return services;
    }

    public static IServiceCollection AddPipelineBehaviours(
        this IServiceCollection services
    )
    {
        services
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>))
            .AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>))
            ;

        return services;
    }
    
    public static Task ExecuteMigrations(
        this WebApplication app
    )
    {
        using var scope = app.Services.CreateScope();
        var runner = scope.ServiceProvider.GetRequiredService<MigrationRunner>();

        var assembly = typeof(Configuration).Assembly;

        return runner.ExecuteMigrationsAsync(assembly);
    }
}