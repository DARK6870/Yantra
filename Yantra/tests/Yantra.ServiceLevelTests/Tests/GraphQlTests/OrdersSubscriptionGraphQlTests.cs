using System.Text.Json;
using FluentAssertions;
using GraphQL;
using GraphQL.Client.Http;
using HotChocolate.Subscriptions;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Yantra.Application.Features.Orders.Commands;
using Yantra.GraphQl.Subscription;
using Yantra.Mongo.Models;
using Yantra.Mongo.Models.Entities;
using Yantra.Mongo.Models.Enums;
using Yantra.Mongo.Repositories.Interfaces;
using Yantra.ServiceLevelTests.Shared.Collections;
using Yantra.ServiceLevelTests.Shared.Constants.GraphQl;
using Yantra.ServiceLevelTests.Shared.Factory;
using Yantra.ServiceLevelTests.Shared.Helpers;

namespace Yantra.ServiceLevelTests.Tests.GraphQlTests;

[Collection(nameof(MainCollection))]
[Trait("Category", "SmokeTest")]
public class OrdersSubscriptionGraphQlTests(YantraWebApplicationFactory factory)
{
    #region Init

    private readonly GraphQLHttpClient _client = factory.CreateAdminGraphQlHttpClient();
    private readonly IOrdersRepository _ordersRepository = factory.GetRequiredService<IOrdersRepository>();

    #endregion
    
    [Fact(DisplayName = "Subscribe on Order Updates; Should return updates after new order creation")]
    public async Task SubscribeOnOrderUpdates_ShouldReturnUpdatesAfterNewOrderCreation()
    {
        // Arrange
        using var scope = factory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        
        var createOrderRequest = new CreateOrderCommand(
            "Oren Gabay",
            "str. Valise Alecsandri 32/2",
            "oren.gabay@yantra.com",
            "069077777",
            [
                new OrderItem
                {
                    ItemName = TestDataMigrationHelper.ItemName,
                    Quantity = 1
                }
            ]
        );

        var createOrderGraphQlRequest = new GraphQLRequest
        {
            Query = OrdersGraphQlConstants.CreateOrderMutation,
            Variables = new
            {
                request = createOrderRequest
            }
        };

        // Act
        var orderUpdatesSubscriptionStream = CreateOrderUpdatesSubscriptionStream(factory, mediator);
        
        var orderUpdatesEnumerator = orderUpdatesSubscriptionStream.GetAsyncEnumerator();
        var moveNextTask = orderUpdatesEnumerator.MoveNextAsync();
        
        var createOrderResponse = await _client.SendMutationAsync<JsonDocument>(createOrderGraphQlRequest);

        await moveNextTask.AsTask().WaitAsync(new CancellationTokenSource(TimeSpan.FromSeconds(10)).Token);
        var newOrder = orderUpdatesEnumerator.Current;
        
        // Assert
        createOrderResponse.Errors.Should().BeNull();
        
        newOrder.Should().NotBeNull();
        newOrder.CustomerFullName.Should().Be(createOrderRequest.CustomerFullName);
        newOrder.CustomerAddress.Should().Be(createOrderRequest.CustomerAddress);
        newOrder.CustomerEmail.Should().Be(createOrderRequest.CustomerEmail);
        newOrder.CustomerPhone.Should().Be(createOrderRequest.CustomerPhone);
        newOrder.TotalPrice.Should().Be(TestDataMigrationHelper.ItemPrice + 2.5m);
    }
    
    [Fact(DisplayName = "Subscribe on Order Updates; Should return updates after order updates")]
    public async Task SubscribeOnOrderUpdates_ShouldReturnUpdatesAfterOrderUpdates()
    {
        // Arrange
        using var scope = factory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        
        var order = new OrderEntity
        {
            CustomerFullName = "Iana Bux",
            CustomerAddress = "str. Vasile Alecsandri 32",
            CustomerEmail = "yana.bux@yantra.com",
            CustomerPhone = "069077777",
            OrderItems =
            [
                new OrderItem
                {
                    ItemName = TestDataMigrationHelper.ItemName,
                    Price = TestDataMigrationHelper.ItemPrice,
                    Quantity = 2
                }
            ],
            Status = OrderStatus.Pending,
            DeliveryPrice = 2.5m,
            TotalPrice = 22.5m
        };
        
        var updateOrderRequest = new UpdateOrderCommand(
            order.Id,
            "str. Valise Alecsandri 32/2",
            OrderStatus.OnDelivery,
            [
                new OrderItem
                {
                    ItemName = TestDataMigrationHelper.ItemName,
                    Quantity = 3
                }
            ]
        );

        var updateOrderGraphQlRequest = new GraphQLRequest
        {
            Query = OrdersGraphQlConstants.UpdateOrderMutation,
            Variables = new
            {
                request = updateOrderRequest
            }
        };

        // Act
        await _ordersRepository.InsertOneAsync(order);
        
        var orderUpdatesSubscriptionStream = CreateOrderUpdatesSubscriptionStream(factory, mediator);
        
        var orderUpdatesEnumerator = orderUpdatesSubscriptionStream.GetAsyncEnumerator();
        var moveNextTask = orderUpdatesEnumerator.MoveNextAsync();
        
        var updateOrderResponse = await _client.SendMutationAsync<JsonDocument>(updateOrderGraphQlRequest);

        await moveNextTask.AsTask().WaitAsync(new CancellationTokenSource(TimeSpan.FromSeconds(10)).Token);
        var updatedOrder = orderUpdatesEnumerator.Current;
        
        // Assert
        updateOrderResponse.Errors.Should().BeNull();
        
        updatedOrder.Should().NotBeNull();
        updatedOrder.CustomerAddress.Should().Be(updateOrderRequest.CustomerAddress);
        updatedOrder.Status.Should().Be(updateOrderRequest.Status);
        updatedOrder.TotalPrice.Should().Be(updateOrderRequest.OrderItems!.Sum(x => x.Quantity * order.OrderItems.First().Price));
    }
    
    private static IAsyncEnumerable<OrderEntity> CreateOrderUpdatesSubscriptionStream(
        WebApplicationFactory<Program> factory,
        IMediator mediator
    )
    {
        var orderSubscription = new OrderSubscription();
        var topicEventReceiver = factory.GetRequiredService<ITopicEventReceiver>();

        return orderSubscription.OrderUpdatesStream(mediator, topicEventReceiver, CancellationToken.None);
    }
}