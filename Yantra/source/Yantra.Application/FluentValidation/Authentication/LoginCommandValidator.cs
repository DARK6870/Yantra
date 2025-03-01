using FluentValidation;
using Yantra.Application.Features.Authentication.Commands;

namespace Yantra.Application.FluentValidation.Authentication;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
                .WithMessage("Email can not be empty")
            .EmailAddress()
                .WithMessage("Invalid email address");

        RuleFor(x => x.Password)
            .NotEmpty()
                .WithMessage("Password can not be empty");
    }
}