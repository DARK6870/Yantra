using MediatR;
using Yantra.Mongo.Models.Entities;
using Yantra.Mongo.Repositories;

namespace Yantra.Application.Features.MenuItems.Queries;

public record GetMenuItemByIdQuery(string Id) : IRequest<MenuItem?>;

public class GetMenuItemByIdHandler(
    IMenuItemsRepository repository
) : IRequestHandler<GetMenuItemByIdQuery, MenuItem?>
{
    public async Task<MenuItem?> Handle(GetMenuItemByIdQuery request, CancellationToken cancellationToken)
    {
        return await repository.FindByIdAsync(request.Id);
    }
}