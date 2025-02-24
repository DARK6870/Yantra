using FluentValidation;
using Yantra.Application.DTOs;

namespace Yantra.Application.FluentValidation;

public class MenuItemDtoValidator : AbstractValidator<MenuItemDto>
{
    public MenuItemDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
                .WithMessage("Item name cannot be empty")
            .Length(5,40)
                .WithMessage("Item name must be between 5 and 40 characters");
        
        RuleFor(x => x.Description)
            .NotEmpty()
                .WithMessage("Item description cannot be empty")
            .MaximumLength(300)
                .WithMessage("Maximum length of item description is 300");

        RuleFor(x => x.Image)
            .NotEmpty()
                .WithMessage("Item image cannot be empty");
        
        RuleFor(x => x.Price)
            .NotNull()
                .WithMessage("Item price cannot be null")
            .GreaterThan(0)
                .WithMessage("Item price must be greater than 0");
    }
}