using MediatR;
using Yantra.Mongo.Models.Entities;
using Yantra.Mongo.Repositories.Interfaces;

namespace Yantra.Application.Features.Accounts.Queries;

public class GetUsersQuery : IRequest<IQueryable<UserEntity>>;

public class GetUsersQueryHandler(
    IUsersRepository repository
) : IRequestHandler<GetUsersQuery, IQueryable<UserEntity>>
{
    public Task<IQueryable<UserEntity>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(repository.AsQueryable());
    }
}