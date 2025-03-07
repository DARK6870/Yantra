using System.Net;
using MediatR;
using Yantra.Application.Responses;
using Yantra.Infrastructure.Common.Exceptions;
using Yantra.Mongo.Repositories.Interfaces;

namespace Yantra.Application.Features.Orders.Queries;

public record GetCustomerOrderByIdQuery(string Id) : IRequest<CustomerOrderResponse>;

public class GetCustomerOrderByIdQueryHandler(
    IOrdersRepository ordersRepository
) : IRequestHandler<GetCustomerOrderByIdQuery, CustomerOrderResponse>
{
    public async Task<CustomerOrderResponse> Handle(GetCustomerOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await ordersRepository.FindByIdAsync(request.Id, cancellationToken)
                    ?? throw new ApiErrorException("Order not found", HttpStatusCode.NotFound);
        
        return CustomerOrderResponse.MapToCustomerOrderResponse(order);
    }
}