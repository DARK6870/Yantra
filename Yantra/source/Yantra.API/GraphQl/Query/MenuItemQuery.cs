using HotChocolate.Authorization;
using MediatR;
using Yantra.Application.Features.MenuItems.Queries;
using Yantra.Mongo.Models.Entities;

namespace Yantra.GraphQl.Query;

[ExtendObjectType(typeof(Query))]
public class MenuItemQuery
{
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    [AllowAnonymous]
    public async Task<List<MenuItemEntity>> GetMenuItems(
        [Service] IMediator mediator,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(new GetMenuItemsQuery(), cancellationToken);
    }

    [UseProjection]
    [AllowAnonymous]
    public async Task<MenuItemEntity?> GetMenuItemByIdAsync(
        [Service] IMediator mediator,
        string id,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(new GetMenuItemByIdQuery(id), cancellationToken);
    }
}