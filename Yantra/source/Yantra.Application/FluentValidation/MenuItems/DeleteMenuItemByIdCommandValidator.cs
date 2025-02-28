using FluentValidation;
using Yantra.Application.Features.MenuItems.Commands;

namespace Yantra.Application.FluentValidation.MenuItems;

public class DeleteMenuItemByIdCommandValidator : AbstractValidator<DeleteMenuItemByIdCommand>
{
    public DeleteMenuItemByIdCommandValidator()
    {
        RuleFor(x => x.Id)
            .Must(ValidationHelper.IsValidObjectId)
                .WithMessage("Id must be a valid ObjectId.");
    }
}