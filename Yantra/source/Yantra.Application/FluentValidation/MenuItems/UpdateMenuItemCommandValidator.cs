using FluentValidation;
using Yantra.Application.Features.MenuItems.Commands;
using Yantra.Mongo.Repositories.Interfaces;

namespace Yantra.Application.FluentValidation.MenuItems;

public class UpdateMenuItemCommandValidator : AbstractValidator<UpdateMenuItemCommand>
{
    public UpdateMenuItemCommandValidator(IMenuItemsRepository repository)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .Must(ValidationHelper.IsValidObjectId)
                .WithMessage("Id must be a valid ObjectId.")
            .MustAsync(async (id, cancellationToken) =>
                await repository.ExistsAsync(
                    x => x.Id == id,
                    cancellationToken)
            )
            .WithMessage("Id cannot be found.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .Length(5, 40)
                .WithMessage("Item name must be between 5 and 40 characters.");

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