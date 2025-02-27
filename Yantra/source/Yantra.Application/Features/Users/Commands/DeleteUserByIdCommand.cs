using MediatR;
using Yantra.Mongo.Repositories.Interfaces;

namespace Yantra.Application.Features.Users.Commands;

public record DeleteUserByIdCommand(string Id) : IRequest<bool>;

public class DeleteUserCommandHandler(
    IUsersRepository usersRepository
) : IRequestHandler<DeleteUserByIdCommand, bool>
{
    public async Task<bool> Handle(DeleteUserByIdCommand request, CancellationToken cancellationToken)
    {
        await usersRepository.DeleteByIdAsync(request.Id, cancellationToken);

        return true;
    }
}