using OnlineStoreOrder.Models;

namespace OnlineStoreOrder.Delivery;

public sealed class DigitalDelivery : DeliveryMethod
{
    public override string Name => "Цифровая доставка";

    public override Money CalculateCost(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        if (order.ContainsPhysicalProducts)
        {
            throw new InvalidOperationException("Цифровая доставка подходит только для заказов без физических товаров.");
        }

        return Money.Zero;
    }
}
