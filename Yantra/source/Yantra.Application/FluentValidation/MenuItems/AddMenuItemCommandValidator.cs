using FluentValidation;
using Yantra.Application.Features.MenuItems.Commands;

namespace Yantra.Application.FluentValidation.MenuItems;

public class AddMenuItemCommandValidator : AbstractValidator<AddMenuItemCommand>
{
    public AddMenuItemCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .Length(5, 40);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(300);

        RuleFor(x => x.Image)
            .NotEmpty();
        
        RuleFor(x => x.Price)
            .NotNull()
            .GreaterThan(0)
                .WithMessage("Item price must be greater than 0.");
    }
}