using System.Text.Json;
using FluentAssertions;
using GraphQL.Client.Http;
using Microsoft.Extensions.Caching.Memory;
using Yantra.Application.Constants;
using Yantra.Application.Features.MenuItems.Commands;
using Yantra.Mongo.Models.Entities;
using Yantra.Mongo.Models.Enums;
using Yantra.Mongo.Repositories.Interfaces;
using Yantra.ServiceLevelTests.Shared.Collections;
using Yantra.ServiceLevelTests.Shared.Constants.GraphQl;
using Yantra.ServiceLevelTests.Shared.Factory;
using Yantra.ServiceLevelTests.Shared.Helpers;
using Yantra.ServiceLevelTests.Shared.Responses.MenuItems;
using GraphQLRequest = GraphQL.GraphQLRequest;

namespace Yantra.ServiceLevelTests.Tests.GraphQlTests;

[Collection(nameof(MainCollection))]
[Trait("Category", "SmokeTest")]
public class MenuItemsGraphQlTests(YantraWebApplicationFactory factory)
{
    private readonly IMenuItemsRepository _menuItemsRepository = factory.GetRequiredService<IMenuItemsRepository>();
    private readonly GraphQLHttpClient _client = factory.CreateAdminGraphQlHttpClient();
    
    [Fact(DisplayName = "Get Menu Items; Should return menu items")]
    public async Task GetMenuItems_ShouldReturnMenuItems()
    {
        // Arrange
        var cache = factory.GetRequiredService<IMemoryCache>();
        
        var menuItem = new MenuItemEntity
        {
            Name = "Gyros Chicken",
            Description = "Gyros Chicken",
            Image = "gyros-chicken.png",
            Type = ItemType.Dish,
            Price = 10m
        };

        // Act
        await _menuItemsRepository.InsertOneAsync(menuItem);
        cache.Remove(CacheConstants.MenuItemsCacheKey);

        var getMenuItemsResult = await _client.SendQueryAsync<GetMenuItemsResponse>(
            new GraphQLRequest(MenuItemsGraphQlConstants.GetMenuItemsQuery)
        );

        // Assert
        
        getMenuItemsResult.Should().NotBeNull();
        getMenuItemsResult.Data.MenuItems.Should().NotBeNullOrEmpty();
        getMenuItemsResult.Data.MenuItems.Count.Should().BeGreaterThanOrEqualTo(1);
        getMenuItemsResult.Data.MenuItems.First(x => x.Name == menuItem.Name).Should().NotBeNull();
    }
    
    [Fact(DisplayName = "Get Menu Item By Id; Should return corresponding menu item")]
    public async Task GetMenuItemById_ShouldReturnMenuItem()
    {
        // Arrange
        var menuItem = new MenuItemEntity
        {
            Name = "Nachos",
            Description = "Nachos",
            Image = "gyros-chicken.png",
            Type = ItemType.Snack,
            Price = 5m
        };

        var getMenuItemByIdGraphQlRequest = new GraphQLRequest
        {
            Query = MenuItemsGraphQlConstants.GetMenuItemByIdQuery,
            Variables = new
            {
                Id = menuItem.Id
            }
        };

        // Act
        await _menuItemsRepository.InsertOneAsync(menuItem);
        var getMenuItemsResult = await _client.SendQueryAsync<GetMenuItemByIdResponse>(getMenuItemByIdGraphQlRequest);

        // Assert
        
        getMenuItemsResult.Should().NotBeNull();
        getMenuItemsResult.Data.Should().NotBeNull();
        getMenuItemsResult.Data.MenuItemEntity.Should().BeEquivalentTo(menuItem, options =>
            options
                .Excluding(x => x.DateUpdated)
                .Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, TimeSpan.FromMilliseconds(1)))
                .WhenTypeIs<DateTime>()
        );
    }

    [Fact(DisplayName = "Create Menu Item; Should create a new menu item")]
    public async Task CreateMenuItem_ShouldCreateMenuItem()
    {
        // Arrange
        var createMenuItemRequest = new CreateMenuItemCommand
        (
            "Pasta Pesto",
            "Pasta Pesto",
            "pasta-pesto.png",
            ItemType.Dish,
            7.5m
        );

        var createMenuItemGraphQlRequest = new GraphQLRequest
        {
            Query = MenuItemsGraphQlConstants.CreateMenuItemMutation,
            Variables = new
            {
                request = createMenuItemRequest
            }
        };

        // Act
        var createMenuItemResponse = await _client.SendMutationAsync<JsonDocument>(createMenuItemGraphQlRequest);
        var exists = await _menuItemsRepository.ExistsAsync(x => x.Name == createMenuItemRequest.Name);

        // Assert
        createMenuItemResponse.Errors.Should().BeNull();

        exists.Should().BeTrue();
    }

    [Fact(DisplayName = "Update Menu Item; Should update menu item")]
    public async Task UpdateMenuItem_ShouldUpdateMenuItem()
    {
        // Arrange
        var menuItem = new MenuItemEntity
        {
            Name = "Helles Beer 0.5",
            Description = "Helles Beer 0.5",
            Image = "helles-beer-0.5.png",
            Type = ItemType.Beverage,
            Price = 2m
        };

        var updateMenuItemRequest = new UpdateMenuItemCommand(
            menuItem.Id,
            "Helles Beer 0.3",
            "Helles Beer 0.3",
            "helles-beer-0.5.png",
            ItemType.Beverage,
            1.5m
        );

        var createMenuItemGraphQlRequest = new GraphQLRequest
        {
            Query = MenuItemsGraphQlConstants.UpdateMenuItemMutation,
            Variables = new
            {
                request = updateMenuItemRequest
            }
        };

        // Act
        await _menuItemsRepository.InsertOneAsync(menuItem);
        var createMenuItemResponse = await _client.SendMutationAsync<JsonDocument>(createMenuItemGraphQlRequest);
        var updateMenuItem = await _menuItemsRepository.FindByIdAsync(menuItem.Id);

        // Assert
        createMenuItemResponse.Errors.Should().BeNull();

        updateMenuItem.Should().NotBeNull();
        updateMenuItem.Name.Should().Be(updateMenuItemRequest.Name);
        updateMenuItem.Description.Should().Be(updateMenuItemRequest.Description);
        updateMenuItem.Image.Should().Be(updateMenuItemRequest.Image);
        updateMenuItem.Type.Should().Be(updateMenuItemRequest.Type);
        updateMenuItem.Price.Should().Be(updateMenuItemRequest.Price);
        updateMenuItem.DateUpdated.Should().BeAfter(menuItem.DateUpdated);
    }

    [Fact(DisplayName = "Delete Menu Item; Should delete menu item")]
    public async Task DeleteMenuItem_ShouldDeleteMenuItem()
    {
        // Arrange
        var menuItem = new MenuItemEntity
        {
            Name = "Cheesecake New York",
            Description = "Cheesecake New York",
            Image = "cheesecake-new-york.png",
            Type = ItemType.Dessert,
            Price = 4m
        };

        var deleteMenuItemGraphQlRequest = new GraphQLRequest
        {
            Query = MenuItemsGraphQlConstants.DeleteMenuItemMutation,
            Variables = new
            {
                id = menuItem.Id
            }
        };

        // Act
        await _menuItemsRepository.InsertOneAsync(menuItem);
        var deleteMenuItemResponse = await _client.SendMutationAsync<JsonDocument>(deleteMenuItemGraphQlRequest);
        
        var exists = await _menuItemsRepository.ExistsAsync(x => x.Name == menuItem.Id);

        // Assert
        deleteMenuItemResponse.Errors.Should().BeNull();

        exists.Should().BeFalse();
    }
}