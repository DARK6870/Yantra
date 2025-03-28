using Yantra.Mongo.Models;
using Yantra.Mongo.Models.Entities;
using Yantra.Mongo.Models.Enums;

namespace Yantra.Application.Responses;

public class CustomerOrderResponse
{
    public required string CustomerFullName { get; set; }
    
    public string? OrderDetails { get; set; }

    public List<OrderItem> OrderItems { get; set; } = [];
    
    public OrderStatus Status { get; set; }
    
    public decimal DeliveryPrice { get; set; }
    
    public decimal TotalPrice { get; set; }
    
    public DateTime DateCreated { get; set; }
    
    public DateTime DateUpdated { get; set; }
    

    public static CustomerOrderResponse MapToCustomerOrderResponse(OrderEntity order)
    {
        return new CustomerOrderResponse()
        {
            CustomerFullName = order.CustomerFullName,
            OrderDetails = order.OrderDetails,
            OrderItems = order.OrderItems,
            Status = order.Status,
            DeliveryPrice = order.DeliveryPrice,
            TotalPrice = order.TotalPrice,
            DateCreated = order.DateCreated,
            DateUpdated = order.DateUpdated,
        };
    }
}