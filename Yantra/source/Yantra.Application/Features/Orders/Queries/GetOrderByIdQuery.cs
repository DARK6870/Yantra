using System.Net;
using MediatR;
using Yantra.Infrastructure.Common.Exceptions;
using Yantra.Mongo.Models.Entities;
using Yantra.Mongo.Repositories.Interfaces;

namespace Yantra.Application.Features.Orders.Queries;

public record GetOrderByIdQuery(string Id) : IRequest<OrderEntity>;

public class GetOrderByIdQueryHandler(
    IOrdersRepository ordersRepository
) : IRequestHandler<GetOrderByIdQuery, OrderEntity>
{
    public async Task<OrderEntity> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        return await ordersRepository.FindByIdAsync(request.Id, cancellationToken)
               ?? throw new ApiErrorException("Order not found", HttpStatusCode.NotFound);
    }
}