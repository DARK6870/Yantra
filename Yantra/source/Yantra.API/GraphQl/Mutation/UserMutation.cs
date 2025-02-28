using MediatR;
using Yantra.Application.Features.Users.Commands;

namespace Yantra.GraphQl.Mutation;

[ExtendObjectType(typeof(Mutation))]
public class UserMutation
{
    public async Task<bool> AddUser(
        [Service] IMediator mediator,
        CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(request, cancellationToken);
    }

    public async Task<bool> UpdateUser(
        [Service] IMediator mediator,
        UpdateUserCommand request,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(request, cancellationToken);
    }
    
    public async Task<bool> DeleteUser(
        [Service] IMediator mediator,
        string id,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(new DeleteUserByIdCommand(id), cancellationToken);
    }
}