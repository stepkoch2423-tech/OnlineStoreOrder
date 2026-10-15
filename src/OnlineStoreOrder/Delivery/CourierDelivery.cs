using OnlineStoreOrder.Models;

namespace OnlineStoreOrder.Delivery;

public sealed class CourierDelivery : DeliveryMethod
{
    public override string Name => "Курьерская доставка";

    public override Money CalculateCost(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        EnsurePhysicalProducts(order);
        return new Money(300m) + new Money(80m) * order.TotalPhysicalWeightKg;
    }

    private static void EnsurePhysicalProducts(Order order)
    {
        if (!order.ContainsPhysicalProducts)
        {
            throw new InvalidOperationException("Курьерская доставка доступна только для физических товаров.");
        }
    }
}
