using MediatR;
using Yantra.Application.DTOs;
using Yantra.Application.Features.MenuItems.Commands;

namespace Yantra.GraphQl.Mutation;

[ExtendObjectType(typeof(GraphQlMutation))]
public class MenuItemMutation
{
    public async Task<bool> AddMenuItem(
        [Service] IMediator mediator,
        MenuItemDto menuItem
    )
    {
        return await mediator.Send(new AddMenuItemCommand(menuItem));
    }
}