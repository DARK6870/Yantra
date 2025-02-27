using MediatR;
using Yantra.Mongo.Models.Entities;
using Yantra.Mongo.Models.Enums;
using Yantra.Mongo.Repositories.Interfaces;

namespace Yantra.Application.Features.MenuItems.Commands;

public record AddMenuItemCommand(
    string Name,
    string Description,
    string Image,
    ItemType Type,
    decimal Price
) : IRequest<bool>;

public class AddMenuItemCommandHandler(
    IMenuItemsRepository repository
) : IRequestHandler<AddMenuItemCommand, bool>
{
    public async Task<bool> Handle(AddMenuItemCommand request, CancellationToken cancellationToken)
    {
        var menuItem = new MenuItem
        {
            Name = request.Name,
            Description = request.Description,
            Image = request.Image,
            Type = request.Type,
            Price = request.Price
        };

        await repository.InsertOneAsync(menuItem);

        return true;
    }
}