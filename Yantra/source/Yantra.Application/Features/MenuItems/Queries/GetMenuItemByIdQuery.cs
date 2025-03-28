using MediatR;
using Microsoft.Extensions.Caching.Memory;
using MongoDB.Driver.Linq;
using Yantra.Application.Constants;
using Yantra.Mongo.Models.Entities;
using Yantra.Mongo.Repositories.Interfaces;

namespace Yantra.Application.Features.MenuItems.Queries;

public record GetMenuItemByIdQuery(string Id) : IRequest<MenuItemEntity?>;

public class GetMenuItemByIdQueryHandler(
    IMenuItemsRepository repository,
    IMemoryCache cache
) : IRequestHandler<GetMenuItemByIdQuery, MenuItemEntity?>
{
    public async Task<MenuItemEntity?> Handle(GetMenuItemByIdQuery request, CancellationToken cancellationToken)
    {
        var cachedItems = await cache.GetOrCreateAsync(
            CacheConstants.MenuItemsCacheKey,
            async entry =>
            {
                entry.SlidingExpiration = CacheConstants.CacheLifetime;
                return await repository.AsQueryable().ToListAsync(cancellationToken);
            }
        );

        return cachedItems?.FirstOrDefault(x => x.Id == request.Id);
    }
}