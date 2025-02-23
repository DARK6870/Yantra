using MediatR;
using Yantra.Application.Features.MenuItems.Queries;
using Yantra.Mongo.Models.Entities;

namespace Yantra.GraphQl.Query;

[ExtendObjectType(typeof(GraphQlQuery))]
public class MenuItemQuery
{
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<MenuItem> GetMenuItems(
        [Service] IMediator mediator,
        CancellationToken cancellationToken
    )
    {
        return mediator.Send(new GetMenuItemsQuery(), cancellationToken).Result;
    }

    [UseProjection]
    public async Task<MenuItem?> GetMenuItemByIdAsync(
        [Service] IMediator mediator,
        string id,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(new GetMenuItemByIdQuery(id), cancellationToken);
    }
}