using MediatR;
using Yantra.Mongo.Models.Entities;
using Yantra.Mongo.Repositories.Interfaces;

namespace Yantra.Application.Features.Orders.Queries;

public record GetOrdersQuery : IRequest<IQueryable<OrderEntity>>;

public class GetOrdersQueryHandler(
    IOrdersRepository ordersRepository
) : IRequestHandler<GetOrdersQuery, IQueryable<OrderEntity>>
{
    public Task<IQueryable<OrderEntity>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(ordersRepository.AsQueryable());
    }
}