using FluentValidation;
using Yantra.Application.Features.Support.Commands;

namespace Yantra.Application.FluentValidation.Support;

public class CreateSupportRequestCommandValidator : AbstractValidator<CreateSupportRequestCommand>
{
    public CreateSupportRequestCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
                .WithMessage("Title can not be empty")
            .Length(10, 50)
                .WithMessage("Title must be between 10 and 50 characters");
        
        RuleFor(x => x.Message)
            .NotEmpty()
                .WithMessage("Message can not be empty")
            .Length(10, 500)
                .WithMessage("Message must be between 10 and 500 characters");
        
        RuleFor(x => x.FullName)
            .NotEmpty()
                .WithMessage("FullName can not be empty")
            .Length(10, 50)
                .WithMessage("FullName must be between 10 and 50 characters");
        
        RuleFor(x => x.ReplyEmail)
            .NotEmpty()
                .WithMessage("ReplyEmail can not be empty")
            .EmailAddress()
                .WithMessage("Invalid Email Address");
    }
}