using MediatR;
using Yantra.Application.Features.Accounts.Queries;
using Yantra.Mongo.Models.Entities;

namespace Yantra.GraphQl.Query;

[ExtendObjectType(typeof(Query))]
public class UserQuery
{
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<UserEntity> GetUsers(
        [Service] IMediator mediator
    )
    {
        return mediator.Send(new GetUsersQuery()).Result;
    }

    [UseProjection]
    public async Task<UserEntity?> GetUserById(
        [Service] IMediator mediator,
        string id,
        CancellationToken cancellationToken
    )
    {
        return await mediator.Send(new GetUserByIdQuery(id), cancellationToken);
    }
}