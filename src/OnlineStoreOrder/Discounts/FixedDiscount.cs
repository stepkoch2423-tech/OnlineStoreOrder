using OnlineStoreOrder.Models;

namespace OnlineStoreOrder.Discounts;

public sealed class FixedDiscount : DiscountPolicy
{
    public FixedDiscount(Money amount)
    {
        Amount = amount;
    }

    public Money Amount { get; }

    public override Money CalculateDiscount(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        if (Amount > order.ProductsTotal)
        {
            throw new InvalidOperationException("Фиксированная скидка не может превышать стоимость товаров.");
        }

        return Amount;
    }
}
