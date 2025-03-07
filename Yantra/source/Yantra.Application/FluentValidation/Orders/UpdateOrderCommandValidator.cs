using FluentValidation;
using Yantra.Application.Features.Orders.Commands;

namespace Yantra.Application.FluentValidation.Orders;

public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderCommandValidator()
    {
        RuleFor(x => x.Id)
            .Must(ValidationHelper.IsValidObjectId)
                .WithMessage("Id must be a valid ObjectId.");
        
        RuleFor(x => x.CustomerAddress)
            .NotEmpty()
                .WithMessage("Address can not be empty")
            .Length(10, 100)
                .WithMessage("Address must be between 10 and 100 characters");
        
        RuleFor(x => x.OrderItems)
            .NotEmpty()
                .WithMessage("OrderItems can not be empty");
    }
}