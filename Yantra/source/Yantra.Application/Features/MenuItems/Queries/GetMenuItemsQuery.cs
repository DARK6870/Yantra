using MediatR;
using Microsoft.Extensions.Caching.Memory;
using MongoDB.Driver.Linq;
using Yantra.Application.Constants;
using Yantra.Mongo.Models.Entities;
using Yantra.Mongo.Repositories.Interfaces;

namespace Yantra.Application.Features.MenuItems.Queries;

public record GetMenuItemsQuery : IRequest<List<MenuItemEntity>>;

public class GetMenuItemsQueryHandler(
    IMenuItemsRepository repository,
    IMemoryCache cache
) : IRequestHandler<GetMenuItemsQuery, List<MenuItemEntity>>
{
    public async Task<List<MenuItemEntity>> Handle(GetMenuItemsQuery request, CancellationToken cancellationToken)
    {
        var cachedItems = await cache.GetOrCreateAsync(
            CacheConstants.MenuItemsCacheKey,
            async entry =>
            {
                entry.SlidingExpiration = CacheConstants.CacheLifetime;
                return await repository.AsQueryable().ToListAsync(cancellationToken);
            }
        );

        return cachedItems ?? [];
    }
}