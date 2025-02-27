using MediatR;
using Yantra.Mongo.Models.Entities;
using Yantra.Mongo.Models.Enums;
using Yantra.Mongo.Repositories.Interfaces;

namespace Yantra.Application.Features.Users.Commands;

public record CreateUserCommand(
    string UserName,
    string Email,
    string FirstName,
    string LastName,
    Role Role
) : IRequest<bool>;

public class CreateUserCommandHandler(
    IUsersRepository usersRepository
) : IRequestHandler<CreateUserCommand, bool>
{
    public async Task<bool> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new UserEntity()
        {
            UserName = request.UserName,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Role = request.Role,
            MustChangePassword = true
        };
        
        await usersRepository.InsertOneAsync(user, cancellationToken);
        return true;
    }
}