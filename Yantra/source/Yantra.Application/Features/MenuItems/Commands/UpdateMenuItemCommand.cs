using System.Net;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Yantra.Application.Constants;
using Yantra.Infrastructure.Common.Exceptions;
using Yantra.Mongo.Models.Entities;
using Yantra.Mongo.Models.Enums;
using Yantra.Mongo.Repositories.Interfaces;

namespace Yantra.Application.Features.MenuItems.Commands;

public record UpdateMenuItemCommand(
    string Id,
    string Name,
    string Description,
    string Image,
    ItemType Type,
    decimal Price
) : IRequest<bool>;

public class UpdateMenuItemCommandHandler(
    IMenuItemsRepository repository,
    IMemoryCache cache
) : IRequestHandler<UpdateMenuItemCommand, bool>
{
    public async Task<bool> Handle(UpdateMenuItemCommand request, CancellationToken cancellationToken)
    {
        if (!await repository.ExistsAsync(request.Id, cancellationToken))
            throw new ApiErrorException("MenuItem does not exist", HttpStatusCode.NotFound);
        
        var menuItem = new MenuItemEntity()
        {
            Id = request.Id,
            Name = request.Name,
            Description = request.Description,
            Image = request.Image,
            Type = request.Type,
            Price = request.Price
        };

        await repository.ReplaceOneAsync(menuItem, cancellationToken);
        cache.Remove(CacheConstants.MenuItemsCacheKey);
        
        return true;
    }
}