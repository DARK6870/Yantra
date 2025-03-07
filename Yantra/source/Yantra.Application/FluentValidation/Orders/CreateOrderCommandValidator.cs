using FluentValidation;
using Yantra.Application.Features.Orders.Commands;

namespace Yantra.Application.FluentValidation.Orders;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.CustomerFullName)
            .NotEmpty()
                .WithMessage("Full Name can not be empty")
            .Length(10, 50)
                .WithMessage("Full Name must be between 10 and 50 characters");
        
        RuleFor(x => x.CustomerAddress)
            .NotEmpty()
                .WithMessage("Address can not be empty")
            .Length(10, 100)
                .WithMessage("Address must be between 10 and 100 characters");
        
        RuleFor(x => x.CustomerEmail)
            .NotEmpty()
                .WithMessage("Email can not be empty")
            .EmailAddress()
                .WithMessage("Email address is invalid");
        
        RuleFor(x => x.CustomerPhone)
            .NotEmpty()
                .WithMessage("Phone can not be empty")
            .Length(9)
                .WithMessage("Phone must have exactly 9 digits")
            .Matches(@"^\d{9}$")
                .WithMessage("Phone must consist of numbers only");

        RuleFor(x => x.DeliveryPrice)
            .GreaterThanOrEqualTo(0)
                .WithMessage("Price must be greater than or equal to 0");

        RuleFor(x => x.OrderItems)
            .NotEmpty()
                .WithMessage("OrderItems can not be empty");
    }
}