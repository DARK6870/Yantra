using System.Text.Json.Serialization;
using Yantra.Application.Responses;

namespace Yantra.ServiceLevelTests.Shared.Responses.Authorization;

public record LoginMutationResponse(
    [property: JsonPropertyName("login")]
    LoginResponse LoginData
);