using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Yantra.Mongo.Repositories.Implementations;
using Yantra.Mongo.Repositories.Interfaces;

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
            .AddSingleton<IUsersRepository, UsersRepository>()
            .AddSingleton<IRefreshTokensRepository, RefreshTokensRepository>()
            .AddSingleton<IMigrationsRepository, MigrationsRepository>()
            .AddSingleton<IOrdersRepository, OrdersRepository>()
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