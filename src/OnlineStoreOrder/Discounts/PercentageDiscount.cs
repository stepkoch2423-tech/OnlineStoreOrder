using OnlineStoreOrder.Models;

namespace OnlineStoreOrder.Discounts;

public sealed class PercentageDiscount : DiscountPolicy
{
    public PercentageDiscount(decimal percentage)
    {
        if (percentage is < 0 or > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(percentage), "Процент скидки должен быть от 0 до 100.");
        }

        Percentage = percentage;
    }

    public decimal Percentage { get; }

    public override Money CalculateDiscount(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        return order.ProductsTotal * (Percentage / 100m);
    }
}
