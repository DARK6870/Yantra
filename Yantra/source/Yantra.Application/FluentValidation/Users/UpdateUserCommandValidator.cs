using FluentValidation;
using Yantra.Application.Features.Users.Commands;
using Yantra.Mongo.Repositories.Interfaces;

namespace Yantra.Application.FluentValidation.Users;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator(IUsersRepository usersRepository)
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
            .MaximumLength(20)
            .MustAsync(async (command, userName, cancellationToken) =>
                !await usersRepository.ExistsAsync(
                    x => x.UserName == userName &&
                         x.Id != command.Id,
                    cancellationToken)
            )
            .WithMessage("UserName already registered.");


        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MustAsync(async (command, email, cancellationToken) =>
                !await usersRepository.ExistsAsync(
                    x => x.Email == email
                         && x.Id != command.Id,
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