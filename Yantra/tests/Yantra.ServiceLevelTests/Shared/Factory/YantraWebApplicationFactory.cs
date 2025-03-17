using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Testcontainers.MongoDb;
using Yantra.Mongo.Models.Entities;
using Yantra.Mongo.Models.Enums;
using Yantra.Mongo.Repositories.Interfaces;

namespace Yantra.ServiceLevelTests.Shared.Factory;

public class YantraWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MongoDbContainer _mongoDbContainer =
        new MongoDbBuilder()
            .WithUsername("admin")
            .WithPassword("admin")
            .Build();

    private string MongoDbConnectionString => _mongoDbContainer.GetConnectionString();

    protected override IHost CreateHost(IHostBuilder builder)
    {
        Environment.SetEnvironmentVariable("MongoDb__ConnectionString", MongoDbConnectionString);
        Environment.SetEnvironmentVariable("AuthenticationOptions__EnableSecurity", bool.TrueString);
        
        return base.CreateHost(builder);
    }

    public async Task InitializeAsync()
    {
        await _mongoDbContainer.StartAsync();

        var menuItemsRepository = Services.GetRequiredService<IMenuItemsRepository>();
        var menuItem = new MenuItemEntity
        {
            Name = "Pizza Prosciutto",
            Description = "Pizza Prosciutto",
            Image = "pizza-prosciutto.png",
            Type = ItemType.Dish,
            Price = 10m
        };
        
        var usersRepository = Services.GetRequiredService<IUsersRepository>();
        var user = new UserEntity
        {
            Email = "courier@yantra.com",
            FirstName = "John",
            LastName = "Doe",
            Role = Role.Courier,
            UserName = "test-courier"
        };

        await usersRepository.InsertOneAsync(user);
        await menuItemsRepository.InsertOneAsync(menuItem);
    }

    public new async Task DisposeAsync()
    {
        await base.DisposeAsync();
    }
}