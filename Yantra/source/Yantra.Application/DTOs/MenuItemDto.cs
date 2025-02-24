using Yantra.Mongo.Models.Entities;
using Yantra.Mongo.Models.Enums;

namespace Yantra.Application.DTOs;

public record MenuItemDto(
    string Name,
    string Description,
    string Image,
    ItemType Type,
    Decimal Price
)
{
    public MenuItem MapToMenuItem()
    {
        return new MenuItem
        {
            Name = Name,
            Description = Description,
            Image = Image,
            Type = Type,
            Price = Price
        };
    }
}