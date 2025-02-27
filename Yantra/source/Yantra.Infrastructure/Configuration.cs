using System.Text;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Yantra.Infrastructure.Common.Behaviours;
using Yantra.Infrastructure.Options;
using Yantra.Infrastructure.Services;
using Yantra.Infrastructure.Services.Implementations;
using Yantra.Infrastructure.Services.Interfaces;

namespace Yantra.Infrastructure;

public static class Configuration
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services
    )
    {
        services
            .AddSingleton<IAuthenticationService, AuthenticationService>()
            .AddMemoryCache()
            ;

        return services;
    }

    public static IServiceCollection AddConfiguration(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services
            .Configure<JwtOptions>(configuration.GetSection("AuthenticationOptions:JwtOptions"));
            ;

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

    public static IServiceCollection AddBearerAuthentication(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                            configuration.GetValue<string>("AuthenticationOptions:JwtOptions:SecretKey")
                            ?? throw new NullReferenceException("Security key is missing.")
                        )
                    )
                };
            });

        return services;
    }
}