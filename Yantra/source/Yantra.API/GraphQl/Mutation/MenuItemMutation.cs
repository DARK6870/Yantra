using MediatR;
using Yantra.Application.Features.MenuItems.Commands;

namespace Yantra.GraphQl.Mutation;

[ExtendObjectType(typeof(Mutation))]
public class MenuItemMutation
{
    public async Task<bool> CreateMenuItem(
        [Service] IMediator mediator,
        CreateMenuItemCommand request,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(request, cancellationToken);
    }

    public async Task<bool> UpdateMenuItem(
        [Service] IMediator mediator,
        UpdateMenuItemCommand request,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(request, cancellationToken);
    }

    public async Task<bool> DeleteMenuItem(
        [Service] IMediator mediator,
        string id,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(new DeleteMenuItemByIdCommand(id), cancellationToken);
    }
}