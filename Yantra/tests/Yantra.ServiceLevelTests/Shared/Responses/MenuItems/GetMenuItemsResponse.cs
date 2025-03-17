using Yantra.Mongo.Models.Entities;

namespace Yantra.ServiceLevelTests.Shared.Responses.MenuItems;

public record GetMenuItemsResponse(List<MenuItemEntity> MenuItems);