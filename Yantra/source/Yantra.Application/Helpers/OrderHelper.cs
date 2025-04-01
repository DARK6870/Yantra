using Yantra.Mongo.Models.Entities;

namespace Yantra.Application.Helpers;

public static class OrderHelper
{
    private const decimal DeliveryPrice = 2.5m;

    public static void SetDeliveryAndTotalPrice(this OrderEntity orderEntity, decimal itemsPrice)
    {
        orderEntity.DeliveryPrice = itemsPrice >= 25m ? 0 : DeliveryPrice;
        orderEntity.TotalPrice = orderEntity.DeliveryPrice + itemsPrice;
    }
}
