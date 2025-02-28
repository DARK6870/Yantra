using FluentValidation;
using Yantra.Application.Features.Authentication.Commands;

namespace Yantra.Application.FluentValidation.Authentication;

public class SetPasswordCommandValidator : AbstractValidator<SetPasswordCommand>
{
    public SetPasswordCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
                .WithMessage("Email can not be empty");
        
        RuleFor(x => x.Password)
            .NotEmpty()
                .WithMessage("Password can not be empty")
            .Length(6, 20)
                .WithMessage("Password must be between 6 and 20 characters");
        
        RuleFor(x => x.SetPasswordToken)
            .NotEmpty()
                .WithMessage("Email can not be empty");
    }
}