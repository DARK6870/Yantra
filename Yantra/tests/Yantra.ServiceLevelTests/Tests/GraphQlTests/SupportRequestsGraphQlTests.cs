using System.Text.Json;
using FluentAssertions;
using GraphQL.Client.Http;
using Yantra.Application.Features.Support.Commands;
using Yantra.ServiceLevelTests.Shared.Collections;
using Yantra.ServiceLevelTests.Shared.Constants.GraphQl;
using Yantra.ServiceLevelTests.Shared.Factory;
using Yantra.ServiceLevelTests.Shared.Helpers;
using GraphQLRequest = GraphQL.GraphQLRequest;

namespace Yantra.ServiceLevelTests.Tests.GraphQlTests;

[Collection(nameof(MainCollection))]
[Trait("Category", "SmokeTest")]
public class SupportRequestsGraphQlTests(YantraWebApplicationFactory factory)
{
    private readonly GraphQLHttpClient _client = factory.CreateGraphQlHttpClient();

    [Fact(DisplayName = "Submit Support Request; Should return successful response")]
    public async Task SubmitSupportRequest_ShouldReturnSuccessfulResponse()
    {
        // Arrange
        var submitSupportRequest = new CreateSupportRequestCommand(
            "I can't create an order",
            "I'm unable to create an order. It shows an error stating that I'm not authorized to access this resource.",
            "Vlad Timbal",
            "tsymbalvlad.6870@gmail.com"
        );

        var submitSupportGraphQlRequest = new GraphQLRequest()
        {
            Query = SupportRequestsGraphQlConstants.SubmitSupportRequestCommand,
            Variables = new
            {
                request = submitSupportRequest
            }
        };

        // Act
        var submitSupportRequestResponse = await _client.SendMutationAsync<JsonDocument>(submitSupportGraphQlRequest);

        // Assert
        submitSupportRequestResponse.Errors.Should().BeNull();
    }
}