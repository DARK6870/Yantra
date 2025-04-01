using FluentAssertions;
using GraphQL;
using GraphQL.Client.Http;
using Yantra.Application.Features.Authentication.Commands;
using Yantra.Mongo.Common.Helpers;
using Yantra.Mongo.Models.Entities;
using Yantra.Mongo.Models.Enums;
using Yantra.Mongo.Repositories.Interfaces;
using Yantra.ServiceLevelTests.Shared.Collections;
using Yantra.ServiceLevelTests.Shared.Constants.GraphQl;
using Yantra.ServiceLevelTests.Shared.Factory;
using Yantra.ServiceLevelTests.Shared.Helpers;
using Yantra.ServiceLevelTests.Shared.Responses.Authorization;
using Yantra.ServiceLevelTests.Shared.Responses.Users;

namespace Yantra.ServiceLevelTests.Tests.Authentication;

[Collection(nameof(MainCollection))]
[Trait("Category", "SmokeTest")]
public class AuthorizationTests(YantraWebApplicationFactory factory)
{
    private readonly GraphQLHttpClient _client = factory.CreateGraphQlHttpClient();
    private readonly IUsersRepository _usersRepository = factory.GetRequiredService<IUsersRepository>();
    
    [Fact(DisplayName = "Get Users Without Access Token; Should return Unauthorized")]
    public async Task GetUsers_WithoutAccessToken_ShouldReturnUnauthorized()
    {
        try
        {
            // Act
            await _client.SendQueryAsync<GetUsersResponse>(new GraphQLRequest(UsersGraphQlConstants.GetUsersQuery));
        }
        catch (GraphQLHttpRequestException ex)
        {
            // Assert
            ex.Message.Should().Be("The HTTP request failed with status code Unauthorized");
        }
    }
    
    [Fact(DisplayName = "Get Users Without Admin Role; Should return Forbidden")]
    public async Task GetUsers_WithoutAdminRole_ShouldReturnForbidden()
    {
        // Arrange
        var user = new UserEntity
        {
            UserName = "User5",
            Email = "user5@yantra.com",
            FirstName = "Test5",
            LastName = "Test5",
            Role = Role.Courier,
            PasswordHash = HashHelper.ComputeHash("pass123")
        };

        var loginRequest = new LoginCommand(
            user.Email,
            "pass123"
        );

        var loginGraphQlRequest = new GraphQLRequest
        {
            Query = AuthenticationGraphQlConstants.LoginMutation,
            Variables = new
            {
                request = loginRequest
            }
        };
        
        try
        {
            // Act
            await _usersRepository.InsertOneAsync(user);
            
            var loginResponse = await _client.SendQueryAsync<LoginMutationResponse>(loginGraphQlRequest);
            var secureClient = factory.CreateGraphQlHttpClient(loginResponse.Data.LoginData.AccessToken);
            
            await secureClient.SendQueryAsync<GetUsersResponse>(new GraphQLRequest(UsersGraphQlConstants.GetUsersQuery));
        }
        catch (GraphQLHttpRequestException ex)
        {
            // Assert
            ex.Message.Should().Be("The HTTP request failed with status code Forbidden");
        }
    }
}