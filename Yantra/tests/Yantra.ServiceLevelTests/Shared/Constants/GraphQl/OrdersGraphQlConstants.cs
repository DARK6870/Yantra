namespace Yantra.ServiceLevelTests.Shared.Constants.GraphQl;

public static class OrdersGraphQlConstants
{
  public const string GetOrdersQuery =
    """
    query getOrders {
      orders {
        totalCount
        items {
          id
        customerFullName
        customerEmail
        customerPhone
        customerAddress
        courierName
        orderDetails
        orderItems {
          itemName
          price
          quantity
        }
        status
        deliveryPrice
        totalPrice
        dateCreated
        dateUpdated
        }
      }
    }
    """;

  public const string GetOrderByIdQuery =
    """
    query getOrderById($id: String!){
      orderById(id: $id)
      {
        id
        customerFullName
        customerEmail
        customerPhone
        customerAddress
        courierName
        orderDetails
        orderItems {
          itemName
          price
          quantity
        }
        status
        deliveryPrice
        totalPrice
        dateCreated
        dateUpdated
      }
    }
    """;

  public const string GetCustomerOrderByIdQuery =
    """
    query getCustomerOrderById($id: String!){
      customerOrderById(id: $id)
      {
        customerFullName
        orderDetails
        orderItems {
          itemName
          price
          quantity
        }
        status
        deliveryPrice
        totalPrice
        dateCreated
        dateUpdated
      }
    }
    """;

  public const string OnOrderUpdatesSubscription =
    """
    subscription orderUpdates{
      onOrderUpdates{
        id
        customerFullName
        customerEmail
        customerPhone
        customerAddress
        courierName
        orderDetails
        orderItems {
          itemName
          price
          quantity
        }
        status
        deliveryPrice
        totalPrice
        dateCreated
        dateUpdated
      }
    }
    """;

  public const string CreateOrderMutation =
    """
    mutation createOrder($request: CreateOrderCommandInput!){
      createOrder(request: $request)
    }
    """;

  public const string UpdateOrderMutation =
    """
    mutation updateOrder($request: UpdateOrderCommandInput!){
      updateOrder(request: $request)
    }
    """;

  public const string UpdateOrderStatusMutation =
    """
    mutation updateOrderStatus($request: UpdateOrderStatusCommandInput!){
      updateOrderStatus(request: $request)
    }
    """;
}