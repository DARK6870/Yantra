using Yantra.Mongo.Models.Enums;

namespace Yantra.Infrastructure.Services.Interfaces;

public interface IUserContext
{
    string UserName { get; }
    Role Role { get; }
}