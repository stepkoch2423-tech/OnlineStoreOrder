using OnlineStoreOrder.Delivery;
using OnlineStoreOrder.Discounts;
using OnlineStoreOrder.Models;

namespace OnlineStoreOrder.Services;

public sealed class CheckoutService
{
    public CheckoutResult Checkout(Order order, DiscountPolicy discountPolicy, DeliveryMethod deliveryMethod)
    {
        ArgumentNullException.ThrowIfNull(order);
        ArgumentNullException.ThrowIfNull(discountPolicy);
        ArgumentNullException.ThrowIfNull(deliveryMethod);

        if (order.Items.Count == 0)
        {
            throw new InvalidOperationException("Нельзя оформить пустой заказ.");
        }

        var productsTotal = order.ProductsTotal;
        var discount = discountPolicy.CalculateDiscount(order);
        var deliveryCost = deliveryMethod.CalculateCost(order);
        var total = productsTotal - discount + deliveryCost;

        return new CheckoutResult(productsTotal, discount, deliveryCost, total);
    }
}
