using System.Net;
using MediatR;
using Yantra.Infrastructure.Common.Exceptions;
using Yantra.Mongo.Repositories.Interfaces;

namespace Yantra.Application.Features.Authentication.Commands;

public record ChangePasswordCommand(
    string Email,
    string OldPassword,
    string NewPassword
) : IRequest<bool>;

public class ChangePasswordCommandHandler(
    IUsersRepository repository
) : IRequestHandler<ChangePasswordCommand, bool>
{
    public async Task<bool> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        if (await repository.GetUserByCredentialsAsync(request.Email, request.OldPassword) == null)
            throw new ApiErrorException("Incorrect password", HttpStatusCode.BadRequest);

        return await repository.UpdatePasswordAsync(request.Email, request.NewPassword);
    }
}