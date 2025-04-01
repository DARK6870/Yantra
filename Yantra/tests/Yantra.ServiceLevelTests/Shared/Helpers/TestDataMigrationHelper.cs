using Microsoft.Extensions.DependencyInjection;
using Yantra.Mongo.Models.Entities;
using Yantra.Mongo.Models.Enums;
using Yantra.Mongo.Repositories.Interfaces;

namespace Yantra.ServiceLevelTests.Shared.Helpers;

public static class TestDataMigrationHelper
{
    public const string ItemName = "Pizza Prosciutto";
    public const decimal ItemPrice = 10m;

    public const string AdminUserName = "admin";
    public const string AdminEmail = "admin@yantra.com";
    
    public static async Task MigrateTestData(this IServiceProvider serviceProvider)
    {
        await Task.WhenAll(
            serviceProvider.MigrateUsers(),
            serviceProvider.MigrateMenuItems()
        );
    }

    private static async Task MigrateUsers(this IServiceProvider serviceProvider)
    {
        var usersRepository = serviceProvider.GetRequiredService<IUsersRepository>();
        var user = new UserEntity
        {
            Email = "courier@yantra.com",
            FirstName = "John",
            LastName = "Doe",
            Role = Role.Courier,
            UserName = "test-courier"
        };
        
        await usersRepository.InsertOneAsync(user);
    }
    
    private static async Task MigrateMenuItems(this IServiceProvider serviceProvider)
    {
        var menuItemsRepository = serviceProvider.GetRequiredService<IMenuItemsRepository>();
        var menuItem = new MenuItemEntity
        {
            Name = ItemName,
            Description = "Pizza Prosciutto",
            Image = "pizza-prosciutto.png",
            Type = ItemType.Dish,
            Price = ItemPrice
        };
        
        await menuItemsRepository.InsertOneAsync(menuItem);
    }
}