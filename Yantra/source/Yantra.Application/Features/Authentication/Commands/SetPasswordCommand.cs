using System.Net;
using MediatR;
using MongoDB.Driver.Linq;
using Yantra.Infrastructure.Common.Exceptions;
using Yantra.Mongo.Repositories.Interfaces;

namespace Yantra.Application.Features.Authentication.Commands;

public record SetPasswordCommand(
    string Email,
    string Password,
    string SetPasswordToken
) : IRequest<bool>;

public class SetPasswordCommandHandler(
    IUsersRepository repository
) : IRequestHandler<SetPasswordCommand, bool>
{
    public async Task<bool> Handle(SetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await repository.AsQueryable()
            .FirstOrDefaultAsync(
                x => x.Email == request.Email &&
                     x.SetPasswordToken == request.SetPasswordToken,
                cancellationToken
            ) ?? throw new ApiErrorException("Invalid credentials", HttpStatusCode.BadRequest);

        await repository.UpdatePasswordAsync(request.Email, request.Password);

        return true;
    }
}