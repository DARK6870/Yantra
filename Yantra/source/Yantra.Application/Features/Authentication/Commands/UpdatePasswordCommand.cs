using FluentValidation;
using MediatR;
using Yantra.Mongo.Repositories.Interfaces;

namespace Yantra.Application.Features.Authentication.Commands;

public record UpdatePasswordCommand(
    string Email,
    string OldPassword,
    string NewPassword
) : IRequest<bool>;

public class UpdatePasswordCommandHandler(
    IUsersRepository usersRepository
) : IRequestHandler<UpdatePasswordCommand, bool>
{
    public async Task<bool> Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await usersRepository.GetUserByCredentials(request.Email, request.OldPassword)
            ?? throw new ValidationException("Incorrect password.");

        return await usersRepository.UpdatePassword(request.Email, request.NewPassword);
    }
}