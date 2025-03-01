using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Yantra.Mongo.Migration.Core;

namespace Yantra.Mongo.Migration;

public static class Configuration
{
    public static IServiceCollection AddMongoMigrations(
        this IServiceCollection services
    )
    {
        services.AddSingleton<MigrationRunner>();
        
        return services;
    }

    public static Task ExecuteMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var runner = scope.ServiceProvider.GetRequiredService<MigrationRunner>();

        var assembly = typeof(Configuration).Assembly;
        
        return runner.ExecuteMigrationsAsync(assembly);
    }
}