using Yantra.Mongo.Models.Entities;

namespace Yantra.ServiceLevelTests.Shared.Responses.Users;

public record GetUsersResponse(List<UserEntity> Users);