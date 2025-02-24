using FluentValidation;
using MediatR;
using Yantra.Application.DTOs;
using Yantra.Application.FluentValidation;
using Yantra.Mongo.Repositories;

namespace Yantra.Application.Features.MenuItems.Commands;

public record AddMenuItemCommand(MenuItemDto MenuItem) : IRequest<bool>;

public class AddMenuItemHandler(
    IMenuItemsRepository repository,
    MenuItemDtoValidator validator
) : IRequestHandler<AddMenuItemCommand, bool>
{
    public async Task<bool> Handle(AddMenuItemCommand request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request.MenuItem, cancellationToken);

        var menuItem = request.MenuItem.MapToMenuItem();
        await repository.InsertOneAsync(menuItem);

        return true;
    }
}