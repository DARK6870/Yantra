using MediatR;
using Yantra.Mongo.Repositories.Interfaces;

namespace Yantra.Application.Features.MenuItems.Commands;

public record DeleteMenuItemByIdCommand(string Id) : IRequest<bool>;

public class DeleteMenuItemByIdCommandHandler(
    IMenuItemsRepository repository
) : IRequestHandler<DeleteMenuItemByIdCommand, bool>
{
    public async Task<bool> Handle(DeleteMenuItemByIdCommand request, CancellationToken cancellationToken)
    {
        await repository.DeleteByIdAsync(request.Id, cancellationToken);

        return true;
    }
}