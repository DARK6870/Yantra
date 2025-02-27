using FluentValidation;
using MediatR;
using Yantra.Mongo.Models.Entities;
using Yantra.Mongo.Models.Enums;
using Yantra.Mongo.Repositories.Interfaces;

namespace Yantra.Application.Features.Users.Commands;

public record UpdateUserCommand(
    string Id,
    string UserName,
    string Email,
    string FirstName,
    string LastName,
    Role Role
) : IRequest<bool>;

public class UpdateUserCommandHandler(
    IUsersRepository usersRepository
) : IRequestHandler<UpdateUserCommand, bool>
{
    public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await usersRepository.FindByIdAsync(request.Id, cancellationToken)
                   ?? throw new ValidationException("User was not found");
        
        var updatedUser = new UserEntity()
        {
            Id = request.Id,
            UserName = request.UserName,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Role = request.Role,
            PasswordHash = user.PasswordHash,
            MustChangePassword = user.MustChangePassword,
            CreateDate = user.CreateDate,
        };

        await usersRepository.ReplaceOneAsync(updatedUser, cancellationToken);
        return true;
    }
}