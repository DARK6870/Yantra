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
              .WithMessage("UserName cannot be empty")
            .MaximumLength(20)
                .WithMessage("UserName cannot exceed 20 characters")
            .MustAsync(async (command, userName, cancellationToken) =>
                !await usersRepository.ExistsAsync(
                    x => x.UserName == userName &&
                         x.Id != command.Id,
                    cancellationToken)
            )
            .WithMessage("UserName already registered.");


        RuleFor(x => x.Email)
            .NotEmpty()
                .WithMessage("Email cannot be empty")
            .EmailAddress()
                .WithMessage("Invalid email address")
            .MustAsync(async (command, email, cancellationToken) =>
                !await usersRepository.ExistsAsync(
                    x => x.Email == email
                         && x.Id != command.Id,
                    cancellationToken)
            )
            .WithMessage("Email already registered.");

        RuleFor(x => x.FirstName)
            .NotEmpty()
                .WithMessage("FirstName cannot be empty")
            .MaximumLength(20)
                .WithMessage("FirstName cannot exceed 20 characters");

        RuleFor(x => x.LastName)
            .NotEmpty()
                .WithMessage("LastName cannot be empty")
            .MaximumLength(20)
                .WithMessage("LastName cannot exceed 20 characters");
    }
}