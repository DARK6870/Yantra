using MediatR;
using Yantra.Application.Features.Users.Commands;

namespace Yantra.GraphQl.Mutation;

[ExtendObjectType(typeof(Mutation))]
public class UserMutation
{
    public async Task<bool> AddUser(
        [Service] Mediator mediator,
        CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(request, cancellationToken);
    }

    public async Task<bool> UpdateUser(
        [Service] Mediator mediator,
        UpdateUserCommand request,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(request, cancellationToken);
    }
    
    public async Task<bool> DeleteUser(
        [Service] Mediator mediator,
        string id,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(new DeleteUserByIdCommand(id), cancellationToken);
    }
}