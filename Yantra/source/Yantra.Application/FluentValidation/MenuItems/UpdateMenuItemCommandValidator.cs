using FluentValidation;
using Yantra.Application.Features.MenuItems.Commands;
using Yantra.Mongo.Repositories.Interfaces;

namespace Yantra.Application.FluentValidation.MenuItems;

public class UpdateMenuItemCommandValidator : AbstractValidator<UpdateMenuItemCommand>
{
    public UpdateMenuItemCommandValidator()
    {
        RuleFor(x => x.Id)
            .Must(ValidationHelper.IsValidObjectId)
                .WithMessage("Id must be a valid ObjectId.");

        RuleFor(x => x.Name)
            .NotEmpty()
                .WithMessage("Name can not be empty")
            .Length(5, 40)
                .WithMessage("Name must be between 5 and 40 characters.");

        RuleFor(x => x.Description)
            .NotEmpty()
                .WithMessage("Description can not be empty")
            .MaximumLength(300)
                .WithMessage("Description can not exceed 300 characters");

        RuleFor(x => x.Image)
            .NotEmpty()
                .WithMessage("Image can not be empty");
        
        RuleFor(x => x.Price)
            .GreaterThan(0)
                .WithMessage("Price must be greater than 0.");
    }
}