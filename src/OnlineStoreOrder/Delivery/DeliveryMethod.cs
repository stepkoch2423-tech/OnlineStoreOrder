using OnlineStoreOrder.Models;

namespace OnlineStoreOrder.Delivery;

public abstract class DeliveryMethod
{
    public abstract string Name { get; }

    public abstract Money CalculateCost(Order order);
}
