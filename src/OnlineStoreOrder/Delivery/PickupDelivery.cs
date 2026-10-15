using OnlineStoreOrder.Models;

namespace OnlineStoreOrder.Delivery;

public sealed class PickupDelivery : DeliveryMethod
{
    public override string Name => "Самовывоз";

    public override Money CalculateCost(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        if (!order.ContainsPhysicalProducts)
        {
            throw new InvalidOperationException("Самовывоз доступен только для физических товаров.");
        }

        return Money.Zero;
    }
}
