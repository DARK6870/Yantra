using MediatR;
using Yantra.Mongo.Repositories;

namespace Yantra.Application.Features.MenuItems.Commands;

public record DeleteMenuItemByIdCommand(string Id) : IRequest<bool>;

public class DeleteMenuItemByIdHandler(
    IMenuItemsRepository repository
) : IRequestHandler<DeleteMenuItemByIdCommand, bool>
{
    public async Task<bool> Handle(DeleteMenuItemByIdCommand request, CancellationToken cancellationToken)
    {
        await repository.DeleteByIdAsync(request.Id);

        return true;
    }
}