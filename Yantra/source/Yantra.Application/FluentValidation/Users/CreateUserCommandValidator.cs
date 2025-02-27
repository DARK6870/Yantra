using FluentValidation;
using Yantra.Application.Features.Users.Commands;
using Yantra.Mongo.Repositories.Interfaces;

namespace Yantra.Application.FluentValidation.Users;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator(IUsersRepository usersRepository)
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
            .MaximumLength(20)
            .MustAsync(async (userName, cancellationToken) =>
                !await usersRepository.ExistsAsync(
                    x => x.UserName == userName,
                    cancellationToken)
            )
            .WithMessage("UserName already registered.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MustAsync(async (email, cancellationToken) =>
                !await usersRepository.ExistsAsync(
                    x => x.Email == email,
                    cancellationToken)
            )
            .WithMessage("Email already registered.");

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(20);
    }
}