using OnlineStoreOrder.Models;

namespace OnlineStoreOrder.Discounts;

public abstract class DiscountPolicy
{
    public abstract Money CalculateDiscount(Order order);
}
