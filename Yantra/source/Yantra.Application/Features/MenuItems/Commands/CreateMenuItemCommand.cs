using System.Text.Json.Serialization;
using MediatR;
using Yantra.Mongo.Models.Entities;
using Yantra.Mongo.Models.Enums;
using Yantra.Mongo.Repositories.Interfaces;

namespace Yantra.Application.Features.MenuItems.Commands;

public record CreateMenuItemCommand(
    string Name,
    string Description,
    string Image,
    ItemType Type,
    decimal Price
) : IRequest<bool>;

public class CreateMenuItemCommandHandler(
    IMenuItemsRepository repository
) : IRequestHandler<CreateMenuItemCommand, bool>
{
    public async Task<bool> Handle(CreateMenuItemCommand request, CancellationToken cancellationToken)
    {
        var menuItem = new MenuItemEntity
        {
            Name = request.Name,
            Description = request.Description,
            Image = request.Image,
            Type = request.Type,
            Price = request.Price
        };

        await repository.InsertOneAsync(menuItem, cancellationToken);

        return true;
    }
}