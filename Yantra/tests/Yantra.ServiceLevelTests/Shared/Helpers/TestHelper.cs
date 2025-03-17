using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Yantra.Infrastructure.Authentication;
using Yantra.Infrastructure.Services.Interfaces;
using Yantra.Mongo.Models.Enums;

namespace Yantra.ServiceLevelTests.Shared.Helpers;

public static class TestHelper
{
    public static GraphQLHttpClient CreateGraphQlHttpClient(
        this WebApplicationFactory<Program> factory,
        string? accessToken = null
    )
    {
        var httpClient = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            HandleCookies = true
        });
        
        if (accessToken is not null)
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        
        var cookieContainer = new CookieContainer();
        var handler = new HttpClientHandler
        {
            CookieContainer = cookieContainer,
            UseCookies = true
        };
        
        var graphQlHttpClient = new GraphQLHttpClient(
            new GraphQLHttpClientOptions
            {
                EndPoint = new Uri("http://localhost:5000/graphql"),
                HttpMessageHandler = handler
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

    public static string GetAdminAccessToken(
        this WebApplicationFactory<Program> factory
    )
    {
        var authenticationService = factory.GetRequiredService<IAuthenticationService>();

        return authenticationService.GenerateJwtToken(
            "admin",
            "admin@yantra.com",
            Role.Admin.ToString()
        );
    }

    public static GraphQLHttpClient CreateAdminGraphQlHttpClient(
        this WebApplicationFactory<Program> factory
    )
    {
        var token = factory.GetAdminAccessToken();

        return factory.CreateGraphQlHttpClient(token);
    }
}