using MediatR;
using Yantra.Mongo.Repositories;

namespace Yantra.Application.Features.MenuItems.Commands;

public record DeleteMenuItemCommand(string Id) : IRequest<bool>;

public class DeleteMenuItemHandler(
    IMenuItemsRepository repository
) : IRequestHandler<DeleteMenuItemCommand, bool>
{
    public async Task<bool> Handle(DeleteMenuItemCommand request, CancellationToken cancellationToken)
    {
        await repository.DeleteByIdAsync(request.Id);

        return true;
    }
}