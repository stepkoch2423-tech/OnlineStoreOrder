using OnlineStoreOrder.Models;

namespace OnlineStoreOrder.Discounts;

public sealed class NoDiscount : DiscountPolicy
{
    public override Money CalculateDiscount(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        return Money.Zero;
    }
}
