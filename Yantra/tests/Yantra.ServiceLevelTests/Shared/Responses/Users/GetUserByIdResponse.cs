using System.Text.Json.Serialization;
using Yantra.Mongo.Models.Entities;

namespace Yantra.ServiceLevelTests.Shared.Responses.Users;

public record GetUserByIdResponse(
    [property: JsonPropertyName("userById")]
    UserEntity User
);