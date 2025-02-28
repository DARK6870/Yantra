using FluentValidation;
using Yantra.Application.Features.MenuItems.Commands;

namespace Yantra.Application.FluentValidation.MenuItems;

public class AddMenuItemCommandValidator : AbstractValidator<AddMenuItemCommand>
{
    public AddMenuItemCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
                .WithMessage("Name can not be empty")
            .Length(5, 40)
                .WithMessage("Name must be between 5 and 30 characters");

        RuleFor(x => x.Description)
            .NotEmpty()
                .WithMessage("Description can not be empty")
            .MaximumLength(300)
                .WithMessage("Description length can not exceed 300 characters");

        RuleFor(x => x.Image)
            .NotEmpty()
                .WithMessage("Image can not be empty");
        
        RuleFor(x => x.Price)
            .GreaterThan(0)
                .WithMessage("Item price must be greater than 0.");
    }
}