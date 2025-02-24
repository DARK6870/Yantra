using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Yantra.Infrastructure.Interfaces;
using Yantra.Infrastructure.Repositories;
using Yantra.Mongo.Repositories;

namespace Yantra.Mongo;

public static class Configuration
{
    public static IServiceCollection AddRepositories(
        this IServiceCollection services
    )
    {
        services
            .AddSingleton(typeof(IGenericRepository<>), typeof(GenericRepository<>))
            .AddSingleton<IMenuItemsRepository, MenuItemsRepository>()
            ;
        
        return services;
    }

    public static IServiceCollection AddMongoDb(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddSingleton<IMongoClient>(
            new MongoClient(configuration.GetValue<string>("MongoDb:ConnectionString"))
        );

        services.AddSingleton<IMongoDatabase>(sp =>
        {
            var client = sp.GetRequiredService<IMongoClient>();
            return client.GetDatabase(configuration.GetValue<string>("MongoDb:DatabaseName"));
        });

        return services;
    }
}