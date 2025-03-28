using System.Text.Json;
using FluentAssertions;
using GraphQL;
using GraphQL.Client.Http;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver.Linq;
using Yantra.Application.Features.Authentication.Commands;
using Yantra.Application.Features.Users.Commands;
using Yantra.Application.Responses;
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
public class AuthenticationTests(YantraWebApplicationFactory factory)
{
    private readonly GraphQLHttpClient _client = factory.CreateGraphQlHttpClient();
    private readonly IUsersRepository _usersRepository = factory.GetRequiredService<IUsersRepository>();
    private readonly IRefreshTokensRepository _refreshTokensRepository = factory.GetRequiredService<IRefreshTokensRepository>();

    [Fact(DisplayName = "Set Password for New User; Should set password")]
    public async Task SetPassword_ShouldSetPassword()
    {
        // Arrange
        using var scope = factory.Services.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();


        var createUserRequest = new CreateUserCommand(
            "User1",
            "user1@yantra.com",
            "Test1",
            "Test1",
            Role.Manager
        );
        
        await mediator.Send(createUserRequest);
        var user = await _usersRepository.AsQueryable().FirstAsync(x => x.Email == createUserRequest.Email);

        var setPasswordRequest = new SetPasswordCommand(
            user.Email,
            "pass123",
            user.SetPasswordToken!
        );

        var setPasswordGraphQlRequest = new GraphQLRequest
        {
            Query = AuthenticationGraphQlConstants.SetPasswordMutation,
            Variables = new
            {
                request = setPasswordRequest
            }
        };
        
        // Act
        var setPasswordResponse = await _client.SendMutationAsync<JsonDocument>(setPasswordGraphQlRequest);
        var updatedUser = await _usersRepository.AsQueryable().FirstOrDefaultAsync(x => x.Email == createUserRequest.Email);
        
        // Assert
        setPasswordResponse.Errors.Should().BeNull();

        updatedUser.Should().NotBeNull();
        //updatedUser.SetPasswordToken.Should().BeNull();
        updatedUser.PasswordHash.Should().NotBeNullOrEmpty();
        updatedUser.PasswordHash.Should().Be(HashHelper.ComputeHash(setPasswordRequest.Password));
    }

    [Fact(DisplayName = "Log In to Account; Should create refresh token and return access tokens")]
    public async Task LogIntoAccount_ShouldReturnAccessTokens()
    {
        // Arrange
        var user = new UserEntity
        {
            UserName = "User2",
            Email = "user2@yantra.com",
            FirstName = "Test2",
            LastName = "Test2",
            Role = Role.Manager,
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
        
        // Act
        await _usersRepository.InsertOneAsync(user);
        
        var loginResponse = await _client.SendMutationAsync<LoginMutationResponse>(loginGraphQlRequest);
        var refreshToken = await _refreshTokensRepository.AsQueryable().FirstOrDefaultAsync(x => x.UserId == user.Id);

        // Assert
        loginResponse.Errors.Should().BeNull();
        loginResponse.Data.Should().NotBeNull();
        loginResponse.Data.LoginData.Should().NotBeNull();
        loginResponse.Data.LoginData.AccessToken.Should().NotBeNull();
        loginResponse.Data.LoginData.RefreshToken.Should().NotBeNull();
        
        refreshToken.Should().NotBeNull();
        refreshToken.Token.Should().Be(loginResponse.Data.LoginData.RefreshToken);
    }

    [Fact(DisplayName = "Change Password; Should change password")]
    public async Task ChangePassword_ShouldChangePassword()
    {
        // Arrange
        var user = new UserEntity
        {
            UserName = "User3",
            Email = "user3@yantra.com",
            FirstName = "Test3",
            LastName = "Test3",
            Role = Role.Manager,
            PasswordHash = HashHelper.ComputeHash("pass123")
        };
        
        var changePasswordRequest = new ChangePasswordCommand(
            user.Email,
            "pass123",
            "updated123"
        );

        var changePasswordGraphQlRequest = new GraphQLRequest
        {
            Query = AuthenticationGraphQlConstants.ChangePasswordMutation,
            Variables = new
            {
                request = changePasswordRequest
            }
        };
        
        // Act
        await _usersRepository.InsertOneAsync(user);
        
        var changePasswordResponse = await _client.SendMutationAsync<LoginMutationResponse>(changePasswordGraphQlRequest);
        var updatedUser = await _usersRepository.FindByIdAsync(user.Id);

        // Assert
        changePasswordResponse.Errors.Should().BeNull();
        
        updatedUser.Should().NotBeNull();
        updatedUser.PasswordHash.Should().Be(HashHelper.ComputeHash(changePasswordRequest.NewPassword));
    }

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

    /*[Fact(DisplayName = "Refresh Access Token Using Refresh Token; Should refresh access token")]
    public async Task RefreshAccessToken_UsingRefreshToken_ShouldRefreshAccessToken()
    {
        // Arrange
        var user = new UserEntity
        {
            UserName = "user4",
            Email = "user4@yantra.com",
            FirstName = "Test4",
            LastName = "Test4",
            Role = Role.Courier,
            PasswordHash = HashHelper.ComputeHash("pass123")
        };

        var loginRequest = new LoginCommand(
            user.Email,
            "pass123"
        );

        var loginGraphQlRequest = new GraphQLRequest
        {
            Query = AuthenticationGraphQlConstants.LoginCommand,
            Variables = new
            {
                request = loginRequest
            }
        };
        
        // Act
        await _usersRepository.InsertOneAsync(user);
        var loginResponse = await _client.SendMutationAsync<LoginMutationResponse>(loginGraphQlRequest);
        
        foreach (var cookie in _client.HttpClient.DefaultRequestHeaders)
        {
            if (cookie.Key == "Set-Cookie")
            {
                Console.WriteLine($"Cookie: {string.Join(", ", cookie.Value)}");
            }
        }
        
        var refreshAccessTokenResponse = await _client.SendMutationAsync<JsonDocument>(
            new GraphQLRequest(AuthenticationGraphQlConstants.RefreshAccessTokenMutation)
        );

        // Assert
        loginResponse.Errors.Should().BeNull();
        
        refreshAccessTokenResponse.Errors.Should().BeNull();
    }*/

    /*[Fact(DisplayName = "Log Out; Should return successful response")]
    public async Task LogOut_ShouldReturnSuccess()
    {
        // Act
        var logOutResponse = await _client.SendMutationAsync<JsonDocument>(
            new GraphQLRequest(AuthenticationGraphQlConstants.LogoutMutation)
        );

        // Assert
        logOutResponse.Errors.Should().BeNull();
    }*/
}