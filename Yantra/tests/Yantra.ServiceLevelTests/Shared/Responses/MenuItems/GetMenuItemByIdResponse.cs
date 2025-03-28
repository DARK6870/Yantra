using System.Text.Json.Serialization;
using Yantra.Mongo.Models.Entities;

namespace Yantra.ServiceLevelTests.Shared.Responses.MenuItems;

public record GetMenuItemByIdResponse(
    [property: JsonPropertyName("menuItemById")]
    MenuItemEntity MenuItemEntity
);