using FluentValidation;
using Yantra.Application.Features.Orders.Commands;

namespace Yantra.Application.FluentValidation.Orders;

public class UpdateOrderStatusCommandValidator : AbstractValidator<UpdateOrderStatusCommand>
{
    public UpdateOrderStatusCommandValidator()
    {
        RuleFor(x => x.Id)
            .Must(ValidationHelper.IsValidObjectId)
                .WithMessage("Id must be a valid ObjectId.");
    }
}