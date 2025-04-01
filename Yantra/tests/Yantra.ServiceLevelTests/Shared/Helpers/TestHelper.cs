using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;
using HotChocolate.Subscriptions;
using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Yantra.GraphQl.Subscription;
using Yantra.Infrastructure.Services.Interfaces;
using Yantra.Mongo.Models.Entities;
using Yantra.Mongo.Models.Enums;

namespace Yantra.ServiceLevelTests.Shared.Helpers;

public static class TestHelper
{
    public static GraphQLHttpClient CreateGraphQlHttpClient(
        this WebApplicationFactory<Program> factory,
        string? accessToken = null
    )
    {
        var httpClient = factory.CreateClient();
        
        if (accessToken is not null)
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        
        var graphQlHttpClient = new GraphQLHttpClient(
            new GraphQLHttpClientOptions
            {
                EndPoint = new Uri("http://localhost:5000/graphql"),
            },
            new SystemTextJsonSerializer(
                new JsonSerializerOptions
                {
                    Converters = { new JsonStringEnumConverter() },
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                }
            ),
            httpClient
        );

        return graphQlHttpClient;
    }

    public static T GetRequiredService<T>(
        this WebApplicationFactory<Program> factory
    ) where T : notnull
    {
        return factory.Services.GetRequiredService<T>();
    }

    public static GraphQLHttpClient CreateAdminGraphQlHttpClient(
        this WebApplicationFactory<Program> factory
    )
    {
        var token = factory.GetAdminAccessToken();

        return factory.CreateGraphQlHttpClient(token);
    }
    
    private static string GetAdminAccessToken(
        this WebApplicationFactory<Program> factory
    )
    {
        var authenticationService = factory.GetRequiredService<IAuthenticationService>();

        return authenticationService.GenerateJwtToken(
            TestDataMigrationHelper.AdminUserName,
            TestDataMigrationHelper.AdminEmail,
            Role.Admin.ToString()
        );
    }
}