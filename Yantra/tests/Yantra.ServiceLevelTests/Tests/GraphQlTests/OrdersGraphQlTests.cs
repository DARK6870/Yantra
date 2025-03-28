using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text.Json;
using FluentAssertions;
using GraphQL;
using GraphQL.Client.Abstractions.Websocket;
using GraphQL.Client.Http;
using MongoDB.Driver.Linq;
using Xunit.Abstractions;
using Yantra.Application.Features.Orders.Commands;
using Yantra.Mongo.Models;
using Yantra.Mongo.Models.Entities;
using Yantra.Mongo.Models.Enums;
using Yantra.Mongo.Repositories.Interfaces;
using Yantra.ServiceLevelTests.Shared.Collections;
using Yantra.ServiceLevelTests.Shared.Constants.GraphQl;
using Yantra.ServiceLevelTests.Shared.Factory;
using Yantra.ServiceLevelTests.Shared.Helpers;
using Yantra.ServiceLevelTests.Shared.Responses.Orders;

namespace Yantra.ServiceLevelTests.Tests.GraphQlTests;

[Collection(nameof(MainCollection))]
[Trait("Category", "SmokeTest")]
public class OrdersGraphQlTests(YantraWebApplicationFactory factory)
{
    private readonly IOrdersRepository _ordersRepository = factory.GetRequiredService<IOrdersRepository>();
    private readonly GraphQLHttpClient _client = factory.CreateAdminGraphQlHttpClient();

    private const string ItemName = "Pizza Prosciutto";
    private const decimal ItemPrice = 10m;
    
    [Fact(DisplayName = "Get Orders; Should return orders")]
    public async Task GetOrders_ShouldReturnOrders()
    {
        // Arrange
        var order = new OrderEntity
        {
            CustomerFullName = "Vlad Timbal",
            CustomerAddress = "str. Vasile Alecsandri 32",
            CustomerEmail = "vlad.timbal@yantra.com",
            CustomerPhone = "069077777",
            OrderItems =
            [
                new OrderItem
                {
                    ItemName = ItemName,
                    Price = ItemPrice,
                    Quantity = 2
                }
            ],
            Status = OrderStatus.Pending,
            DeliveryPrice = 0m,
            TotalPrice = 20m
        };

        // Act
        await _ordersRepository.InsertOneAsync(order);
        var getOrdersResponse = await _client.SendQueryAsync<GetOrdersResponse>(new GraphQLRequest(OrdersGraphQlConstants.GetOrdersQuery));

        // Assert
        getOrdersResponse.Data.Should().NotBeNull();
        getOrdersResponse.Data.Orders.Should().NotBeNull();
        getOrdersResponse.Data.Orders.TotalCount.Should().BeGreaterThan(0);
        getOrdersResponse.Data.Orders.Items.Should().NotBeNull();
        getOrdersResponse.Data.Orders.Items.Count.Should().BeGreaterThan(0);
        getOrdersResponse.Data.Orders.Items.First(x => x.CustomerFullName == order.CustomerFullName).OrderItems.Should().NotBeNull();
    }
    
    [Fact(DisplayName = "Get Order By Id; Should return corresponding order")]
    public async Task GetOrderById_ShouldReturnOrder()
    {
        // Arrange
        var order = new OrderEntity
        {
            CustomerFullName = "Alexandr Cernomaz",
            CustomerAddress = "str. Vasile Alecsandri 32",
            CustomerEmail = "alexandr.cernomaz@yantra.com",
            CustomerPhone = "069077777",
            OrderItems =
            [
                new OrderItem
                {
                    ItemName = ItemName,
                    Price = ItemPrice,
                    Quantity = 2
                }
            ],
            Status = OrderStatus.Pending,
            DeliveryPrice = 0m,
            TotalPrice = 20m
        };

        var getOrderByIdGraphQlRequest = new GraphQLRequest
        {
            Query = OrdersGraphQlConstants.GetOrderByIdQuery,
            Variables = new
            {
                Id = order.Id
            }
        };

        // Act
        await _ordersRepository.InsertOneAsync(order);
        var getOrderByIdResponse = await _client.SendQueryAsync<GetOrderByIdResponse>(getOrderByIdGraphQlRequest);

        // Assert
        // Assert
        getOrderByIdResponse.Data.Should().NotBeNull();
        getOrderByIdResponse.Data.Order.Should().NotBeNull();
        getOrderByIdResponse.Data.Order.Should().BeEquivalentTo(order, options =>
            options
                .Excluding(x => x.DateCreated)
                .Excluding(x => x.DateUpdated)
                .Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, TimeSpan.FromMilliseconds(1)))
                .WhenTypeIs<DateTime>()
        );
    }

    [Fact(DisplayName = "Get Customer Order By Id; Should return corresponding order")]
    public async Task GetCustomerOrderById_ShouldReturnCustomerOrder()
    {
        // Arrange
        var order = new OrderEntity
        {
            CustomerFullName = "Mihail Fauras",
            CustomerAddress = "str. Vasile Alecsandri 32",
            CustomerEmail = "mihail.fauras@yantra.com",
            CustomerPhone = "069077777",
            OrderItems =
            [
                new OrderItem
                {
                    ItemName = ItemName,
                    Price = ItemPrice,
                    Quantity = 2
                }
            ],
            Status = OrderStatus.Pending,
            DeliveryPrice = 0m,
            TotalPrice = 20m
        };

        var getCustomerOrderByIdGraphQlRequest = new GraphQLRequest
        {
            Query = OrdersGraphQlConstants.GetCustomerOrderByIdQuery,
            Variables = new
            {
                Id = order.Id
            }
        };
        
        // Act
        await _ordersRepository.InsertOneAsync(order);
        var getCustomerOrderByIdResponse = await _client.SendQueryAsync<GetCustomerOrderByIdResponse>(getCustomerOrderByIdGraphQlRequest);
        
        // Assert
        getCustomerOrderByIdResponse.Errors.Should().BeNull();
        getCustomerOrderByIdResponse.Data.Should().NotBeNull();
        getCustomerOrderByIdResponse.Data.Order.Should().NotBeNull();
    }

    [Fact(DisplayName = "Create Order; Should create new order")]
    public async Task CreateOrder_ShouldCreateNewOrder()
    {
        // Arrange
        var createOrderRequest = new CreateOrderCommand(
            "Iana Zabolotnii",
            "str. Zabolotnii 32/2",
            "iana.zabolotnii@yantra.com",
            "069077777",
            [
                new OrderItem
                {
                    ItemName = ItemName,
                    Quantity = 3
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
        var createOrderResponse = await _client.SendMutationAsync<JsonDocument>(createOrderGraphQlRequest);
        var order = await _ordersRepository.AsQueryable()
            .FirstOrDefaultAsync(x => x.CustomerFullName == createOrderRequest.CustomerFullName);
        
        // Assert
        createOrderResponse.Errors.Should().BeNull();
        
        order.Should().NotBeNull();
        order.TotalPrice.Should().Be(ItemPrice * createOrderRequest.OrderItems.First().Quantity);
    }

    [Fact(DisplayName = "Update Order; Should update order")]
    public async Task UpdateOrder_ShouldUpdateOrder()
    {
        // Arrange
        var order = new OrderEntity
        {
            CustomerFullName = "Denis Martin",
            CustomerAddress = "str. Vasile Alecsandri 32",
            CustomerEmail = "denis.martin@yantra.com",
            CustomerPhone = "069077777",
            OrderItems =
            [
                new OrderItem
                {
                    ItemName = ItemName,
                    Price = ItemPrice,
                    Quantity = 1
                }
            ],
            Status = OrderStatus.Pending,
            DeliveryPrice = 0m,
            TotalPrice = 10m
        };

        var updateOrderRequest = new UpdateOrderCommand(
            order.Id,
            "str. Vasile Alecsandri 33",
            order.Status,
            order.OrderItems,
            "test-courier",
            "some details"
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
        var updateOrderResponse = await _client.SendMutationAsync<JsonDocument>(updateOrderGraphQlRequest);
        
        var updatedOrder = await _ordersRepository.FindByIdAsync(order.Id);
        
        // Assert
        updateOrderResponse.Errors.Should().BeNull();
        
        updatedOrder.Should().NotBeNull();
        updatedOrder.CourierName.Should().Be(updateOrderRequest.CourierName);
        updatedOrder.CustomerAddress.Should().Be(updateOrderRequest.CustomerAddress);
        updatedOrder.OrderDetails.Should().Be(updateOrderRequest.OrderDetails);
    }

    [Fact(DisplayName = "Update Order Status; Should update order status")]
    public async Task UpdateOrderStatus_ShouldUpdateOrderStatus()
    {
        // Arrange
        var order = new OrderEntity()
        {
            CustomerFullName = "Ion Bahrin",
            CustomerAddress = "str. Vasile Alecsandri 32",
            CustomerEmail = "ion.bahrin@yantra.com",
            CustomerPhone = "069077777",
            OrderItems =
            [
                new OrderItem
                {
                    ItemName = ItemName,
                    Price = ItemPrice,
                    Quantity = 1
                }
            ],
            Status = OrderStatus.Pending,
            DeliveryPrice = 0m,
            TotalPrice = 10m
        };

        var updateOrderRequest = new UpdateOrderStatusCommand(
            order.Id,
            OrderStatus.OnDelivery
        );

        var updateOrderStatusGraphQlRequest = new GraphQLRequest
        {
            Query = OrdersGraphQlConstants.UpdateOrderStatusMutation,
            Variables = new
            {
                request = updateOrderRequest
            }
        };

        // Act
        await _ordersRepository.InsertOneAsync(order);
        var updateOrderStatusResult = await _client.SendMutationAsync<JsonDocument>(updateOrderStatusGraphQlRequest);

        var updatedOrder = await _ordersRepository.FindByIdAsync(order.Id);

        // Assert
        updateOrderStatusResult.Errors.Should().BeNull();
        
        updatedOrder.Should().NotBeNull();
        updatedOrder.Status.Should().Be(updateOrderRequest.Status);
        updatedOrder.DateUpdated.Should().BeAfter(updatedOrder.DateCreated);
        updatedOrder.Should().BeEquivalentTo(order, options =>
            options
                .Excluding(x => x.Status)
                .Excluding(x => x.DateUpdated)
                .Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, TimeSpan.FromMilliseconds(1)))
                .When(x => x.Path.EndsWith(nameof(order.DateCreated)))
        );
    }

    /*[Fact(DisplayName = "Subscribe on Order Updates; Should return updates after new order creation")]
    public async Task SubscribeOnOrderUpdates_ShouldReturnUpdatesAfterNewOrderCreation()
    {
        // Arrange
        var createOrderRequest = new CreateOrderCommand(
            "Oren Gabay",
            "str. Valise Alecsandri 32/2",
            "oren.gabay@yantra.com",
            "069077777",
            [
                new OrderItem
                {
                    ItemName = ItemName,
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
        var orderUpdatesSubscription = _client.CreateSubscriptionStream<OnOrderUpdatesResponse>(
            new GraphQLRequest(OrdersGraphQlConstants.OnOrderUpdatesSubscription)
        );
        
        var subscription1 = orderUpdatesSubscription.Subscribe(response =>
        {
            Console.WriteLine(response.ToString());
        });

        var update = await orderUpdatesSubscription.FirstOrDefaultAsync().Timeout(TimeSpan.FromSeconds(5));
        
        var createOrderResponse = await _client.SendMutationAsync<JsonDocument>(createOrderGraphQlRequest);
        
        await Task.Delay(-1);
        // Assert
        createOrderResponse.Errors.Should().BeNull();
        
        update.Should().NotBeNull();
        update.Data.Should().NotBeNull();
        update.Data.Order.Should().NotBeNull();
        update.Data.Order.CustomerFullName.Should().Be(createOrderRequest.CustomerFullName);
    }*/
}