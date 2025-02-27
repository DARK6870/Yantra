using FluentValidation;
using Yantra.Application.Features.Users.Commands;
using Yantra.Mongo.Repositories.Interfaces;

namespace Yantra.Application.FluentValidation.Users;

public class DeleteUserCommandValidator : AbstractValidator<DeleteUserByIdCommand>
{
    public DeleteUserCommandValidator(IUsersRepository usersRepository)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .Must(ValidationHelper.IsValidObjectId)
                .WithMessage("Id must be a valid objectId.")
            .MustAsync(async (id, cancellationToken) =>
                await usersRepository.ExistsAsync(
                    x => x.Id == id,
                    cancellationToken)
            )
            .WithMessage("Id can not be found.");
    }
}