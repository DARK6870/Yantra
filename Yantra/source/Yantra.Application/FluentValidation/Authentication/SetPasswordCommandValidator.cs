using FluentValidation;
using Yantra.Application.Features.Authentication.Commands;
using Yantra.Mongo.Repositories.Interfaces;

namespace Yantra.Application.FluentValidation.Authentication;

public class SetPasswordCommandValidator : AbstractValidator<SetPasswordCommand>
{
    public SetPasswordCommandValidator(IUsersRepository usersRepository)
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .MustAsync(async (email, cancellationToken) =>
            {
                var user = await usersRepository.GetUserByEmail(email);
                
                return user is { MustChangePassword: true };
            })
            .WithMessage("Password can not be changed.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .Length(6, 20)
                .WithMessage("Password must be between 6 and 20 characters.");;
    }
}