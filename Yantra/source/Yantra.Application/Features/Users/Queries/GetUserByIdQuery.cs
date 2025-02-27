using MediatR;
using Yantra.Mongo.Models.Entities;
using Yantra.Mongo.Repositories.Interfaces;

namespace Yantra.Application.Features.Accounts.Queries;

public record GetUserByIdQuery(string Id) : IRequest<UserEntity?>;

public class GetUserByIdQueryHandler(
    IUsersRepository usersRepository
) : IRequestHandler<GetUserByIdQuery, UserEntity?>
{
    public async Task<UserEntity?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        return await usersRepository.FindByIdAsync(request.Id, cancellationToken);
    }
}