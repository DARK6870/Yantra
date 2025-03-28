using FluentValidation;
using Yantra.Application.Features.Authentication.Commands;

namespace Yantra.Application.FluentValidation.Authentication;

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
                .WithMessage("Email can not be empty");

        RuleFor(x => x.OldPassword)
            .NotEmpty()
                .WithMessage("OldPassword can not be empty")
            .Must((model, oldPassword) => oldPassword != model.NewPassword)
                .WithMessage("New password can not be the same as old password.");

        RuleFor(x => x.NewPassword)
            .Length(6, 20)
                .WithMessage("Password must be between 6 and 20 characters.");
    }
}