using System.Text.Json;
using FluentAssertions;
using GraphQL;
using GraphQL.Client.Http;
using MongoDB.Driver.Linq;
using Yantra.Application.Features.Users.Commands;
using Yantra.Mongo.Models.Entities;
using Yantra.Mongo.Models.Enums;
using Yantra.Mongo.Repositories.Interfaces;
using Yantra.ServiceLevelTests.Shared.Collections;
using Yantra.ServiceLevelTests.Shared.Constants.GraphQl;
using Yantra.ServiceLevelTests.Shared.Factory;
using Yantra.ServiceLevelTests.Shared.Helpers;
using Yantra.ServiceLevelTests.Shared.Responses.Users;

namespace Yantra.ServiceLevelTests.Tests.GraphQlTests;

[Collection(nameof(MainCollection))]
[Trait("Category", "SmokeTest")]
public class UsersGraphQlTests(YantraWebApplicationFactory factory)
{
    private readonly IUsersRepository _usersRepository = factory.GetRequiredService<IUsersRepository>();
    private readonly GraphQLHttpClient _client = factory.CreateAdminGraphQlHttpClient();

    [Fact(DisplayName = "Get Users; Should return users")]
    public async Task GetUsers_ShouldReturnUsers()
    {
        // Arrange
        var user = new UserEntity
        {
            UserName = "User01",
            Email = "user01@yantra.com",
            FirstName = "Test01",
            LastName = "Test01"
        };

        // Act
        await _usersRepository.InsertOneAsync(user);

        var getUsersResponse = await _client.SendQueryAsync<GetUsersResponse>(
            new GraphQLRequest(UsersGraphQlConstants.GetUsersQuery)
        );

        // Assert
        getUsersResponse.Errors.Should().BeNull();
        getUsersResponse.Data.Should().NotBeNull();
        getUsersResponse.Data.Users.Should().NotBeNull();
        getUsersResponse.Data.Users.First(x => x.UserName == user.UserName).Should().NotBeNull();
    }

    [Fact(DisplayName = "Get User By Id; Should return corresponding user")]
    public async Task GetUserById_ShouldReturnUser()
    {
        // Arrange
        var user = new UserEntity
        {
            UserName = "User02",
            Email = "user02@yantra.com",
            FirstName = "Test02",
            LastName = "Test02"
        };

        var getUserByIdGraphQlRequest = new GraphQLRequest
        {
            Query = UsersGraphQlConstants.GetUserByIdQuery,
            Variables = new
            {
                id = user.Id
            }
        };
        
        // Act
        await _usersRepository.InsertOneAsync(user);
        var getUserByIdResponse = await _client.SendQueryAsync<GetUserByIdResponse>(getUserByIdGraphQlRequest);
        
        // Assert
        getUserByIdResponse.Errors.Should().BeNull();
        getUserByIdResponse.Data.Should().NotBeNull();
        getUserByIdResponse.Data.User.Should().NotBeNull();
        getUserByIdResponse.Data.User.Should().BeEquivalentTo(user, options =>
            options
                .Excluding(x => x.SetPasswordToken)
                .Excluding(x => x.DateCreated)
                .Excluding(x => x.DateUpdated)
                .Using<DateTime>(ctx => ctx.Subject.Should().BeCloseTo(ctx.Expectation, TimeSpan.FromSeconds(1)))
                .WhenTypeIs<DateTime>()
        );
    }

    [Fact(DisplayName = "Create User; Should create new user")]
    public async Task CreateUser_ShouldCreateNewUser()
    {
        // Arrange
        var createUserRequest = new CreateUserCommand(
            "User03",
            "user03@yantra.com",
            "Test03",
            "Test04",
            Role.Admin
        );

        var createUserGraphQlRequest = new GraphQLRequest
        {
            Query = UsersGraphQlConstants.CreateUserMutation,
            Variables = new
            {
                request = createUserRequest
            }
        };

        // Act
        var createUserResponse = await _client.SendQueryAsync<JsonDocument>(createUserGraphQlRequest);
        var user = await _usersRepository.AsQueryable().FirstOrDefaultAsync(x => x.Email == createUserRequest.Email);

        // Assert
        createUserResponse.Errors.Should().BeNull();
        
        user.Should().NotBeNull();
    }

    [Fact(DisplayName = "Update User; Should update user")]
    public async Task UpdateUser_ShouldUpdateUser()
    {
        // Arrange
        var user = new UserEntity
        {
            UserName = "User04",
            Email = "user04@yantra.com",
            FirstName = "Test04",
            LastName = "Test04"
        };

        var updateUserRequest = new UpdateUserCommand(
            user.Id,
            "User05",
            "user05@yantra.com",
            "Test05",
            "Test05",
            Role.Manager
        );

        var updateUserGraphQlRequest = new GraphQLRequest
        {
            Query = UsersGraphQlConstants.UpdateUserMutation,
            Variables = new
            {
                request = updateUserRequest
            }
        };
        
        // Act
        await _usersRepository.InsertOneAsync(user);
        var updateUserResponse = await _client.SendQueryAsync<JsonDocument>(updateUserGraphQlRequest);
        var updatedUser = await _usersRepository.FindByIdAsync(user.Id);

        // Assert
        updateUserResponse.Errors.Should().BeNull();
        
        updatedUser.Should().NotBeNull();
        updatedUser.UserName.Should().Be(updateUserRequest.UserName);
        updatedUser.Email.Should().Be(updateUserRequest.Email);
        updatedUser.FirstName.Should().Be(updateUserRequest.FirstName);
        updatedUser.LastName.Should().Be(updateUserRequest.LastName);
        updatedUser.Role.Should().Be(updateUserRequest.Role);
    }

    [Fact(DisplayName = "Delete User; Should delete user")]
    public async Task DeleteUser_ShouldDeleteUser()
    {
        // Arrange
        var user = new UserEntity
        {
            UserName = "User06",
            Email = "user0@yantra.com",
            FirstName = "Test06",
            LastName = "Test06"
        };

        var deleteUserGraphQlRequest = new GraphQLRequest
        {
            Query = UsersGraphQlConstants.DeleteUserMutation,
            Variables = new
            {
                id = user.Id
            }
        };

        // Act
        await _usersRepository.InsertOneAsync(user);
        var deleteUserResponse = await _client.SendMutationAsync<JsonDocument>(deleteUserGraphQlRequest);

        var exists = await _usersRepository.ExistsAsync(user.Id);

        // Assert
        deleteUserResponse.Errors.Should().BeNull();
        
        exists.Should().BeFalse();
    }
}