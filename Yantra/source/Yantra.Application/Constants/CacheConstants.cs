namespace Yantra.Application.Constants;

public static class CacheConstants
{
    public static readonly TimeSpan CacheLifetime = TimeSpan.FromMinutes(20);
    public const string MenuItemsCacheKey = "menu-items";
}