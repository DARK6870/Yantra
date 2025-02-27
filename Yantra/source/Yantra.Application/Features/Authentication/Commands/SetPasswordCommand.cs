using MediatR;
using Yantra.Mongo.Repositories.Interfaces;

namespace Yantra.Application.Features.Authentication.Commands;

public record SetPasswordCommand(
    string Email,
    string Password
) : IRequest<bool>;

public class SetPasswordCommandHandler(
    IUsersRepository usersRepository
) : IRequestHandler<SetPasswordCommand, bool>
{
    public async Task<bool> Handle(SetPasswordCommand request, CancellationToken cancellationToken)
    {
        await usersRepository.UpdatePassword(request.Email, request.Password);
        
        return true;
    }
}