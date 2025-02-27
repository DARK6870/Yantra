using FluentValidation;
using Yantra.Application.Features.MenuItems.Commands;
using Yantra.Mongo.Repositories.Interfaces;

namespace Yantra.Application.FluentValidation.MenuItems;

public class DeleteMenuItemByIdCommandValidator : AbstractValidator<DeleteMenuItemByIdCommand>
{
    public DeleteMenuItemByIdCommandValidator(IMenuItemsRepository repository)
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
    }
}