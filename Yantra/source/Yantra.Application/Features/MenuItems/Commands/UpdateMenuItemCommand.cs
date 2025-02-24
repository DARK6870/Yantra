using MediatR;
using Yantra.Application.DTOs;
using Yantra.Application.FluentValidation;
using Yantra.Mongo.Repositories;

namespace Yantra.Application.Features.MenuItems.Commands;

public record UpdateMenuItemCommand(string Id, MenuItemDto MenuItem) : IRequest<bool>;

public class UpdateMenuItemHandler(
    IMenuItemsRepository repository,
    MenuItemDtoValidator validator
) : IRequestHandler<UpdateMenuItemCommand, bool>
{
    public async Task<bool> Handle(UpdateMenuItemCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAsync(request.MenuItem, cancellationToken);
        var menuItem = request.MenuItem.MapToMenuItem(request.Id);
        
        await repository.ReplaceOneAsync(menuItem);

        return true;
    }
}