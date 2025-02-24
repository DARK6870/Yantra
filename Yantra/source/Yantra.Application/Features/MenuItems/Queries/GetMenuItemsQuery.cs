using MediatR;
using Yantra.Mongo.Models.Entities;
using Yantra.Mongo.Repositories;

namespace Yantra.Application.Features.MenuItems.Queries;

public record GetMenuItemsQuery : IRequest<IQueryable<MenuItem>>;

public class GetMenuItemsHandler(
    IMenuItemsRepository repository
) : IRequestHandler<GetMenuItemsQuery, IQueryable<MenuItem>>
{
    public Task<IQueryable<MenuItem>> Handle(GetMenuItemsQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(repository.AsQueryable());
    }
}