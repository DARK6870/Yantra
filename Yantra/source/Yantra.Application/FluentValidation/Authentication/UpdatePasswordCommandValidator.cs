using FluentValidation;
using Yantra.Application.Features.Authentication.Commands;

namespace Yantra.Application.FluentValidation.Authentication;

public class UpdatePasswordCommandValidator : AbstractValidator<UpdatePasswordCommand>
{
    public UpdatePasswordCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty();

        RuleFor(x => x.OldPassword)
            .NotEmpty()
            .Must((model, oldPassword) => oldPassword != model.NewPassword)
                .WithMessage("New password can not be the same as old password.");

        RuleFor(x => x.NewPassword)
            .Length(6, 20)
                .WithMessage("Password must be between 6 and 20 characters.");
    }
}