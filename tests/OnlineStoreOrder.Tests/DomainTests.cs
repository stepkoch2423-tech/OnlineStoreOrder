using OnlineStoreOrder.Delivery;
using OnlineStoreOrder.Discounts;
using OnlineStoreOrder.Models;
using OnlineStoreOrder.Payments;
using OnlineStoreOrder.Services;

namespace OnlineStoreOrder.Tests;

public class DomainTests
{
    [Fact]
    public void MoneySupportsArithmeticAndRussianFormatting()
    {
        var money = new Money(1_250m);

        Assert.Equal(new Money(1_275m), money + new Money(25m));
        Assert.Equal(new Money(1_200m), money - new Money(50m));
        Assert.Equal(new Money(2_500m), money * 2);
        Assert.Equal(new Money(1_562.5m), money * 1.25m);
        Assert.Equal("1 250,00 ₽", money.ToString());
    }

    [Fact]
    public void MoneyRejectsNegativeValuesAndSubtractionResult()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Money(-1m));
        Assert.Throws<InvalidOperationException>(() => new Money(10m) - new Money(11m));
    }

    [Fact]
    public void ProductsCalculateTheirOwnPricesAndDeliveryCharacteristics()
    {
        Product physical = new PhysicalProduct("Кофе", new Money(500m), 1.5m);
        Product digital = new DigitalProduct("Курс", new Money(800m), 250m);
        Product subscription = new SubscriptionProduct("Музыка", new Money(300m), 12);

        Assert.Equal(new Money(500m), physical.GetUnitPrice());
        Assert.True(physical.RequiresPhysicalDelivery);
        Assert.Equal(1.5m, physical.GetWeightKg());
        Assert.False(digital.RequiresPhysicalDelivery);
        Assert.Equal(0m, digital.GetWeightKg());
        Assert.Equal(new Money(3_240m), subscription.GetUnitPrice());
    }

    [Fact]
    public void OrderSupportsBothAddOverloadsAndAggregatesPhysicalData()
    {
        var order = new Order();
        order.AddProduct(new PhysicalProduct("Кофе", new Money(500m), 1.5m));
        order.AddProduct(new DigitalProduct("Курс", new Money(800m), 250m), 2);

        Assert.Equal(2, order.Items.Count);
        Assert.Equal(new Money(2_100m), order.ProductsTotal);
        Assert.True(order.ContainsPhysicalProducts);
        Assert.Equal(1.5m, order.TotalPhysicalWeightKg);
    }

    [Fact]
    public void OrderRejectsQuantityLessThanOne()
    {
        var order = new Order();

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            order.AddProduct(new DigitalProduct("Курс", new Money(100m), 10m), 0));
    }

    [Fact]
    public void DiscountsValidateTheirRulesAndCalculateDiscount()
    {
        var order = new Order();
        order.AddProduct(new DigitalProduct("Курс", new Money(1_000m), 10m));

        Assert.Equal(new Money(100m), new PercentageDiscount(10m).CalculateDiscount(order));
        Assert.Equal(new Money(250m), new FixedDiscount(new Money(250m)).CalculateDiscount(order));
        Assert.Equal(new Money(0m), new NoDiscount().CalculateDiscount(order));
        Assert.Throws<ArgumentOutOfRangeException>(() => new PercentageDiscount(101m));
        Assert.Throws<InvalidOperationException>(() => new FixedDiscount(new Money(1_001m)).CalculateDiscount(order));
    }

    [Fact]
    public void DeliveryMethodsCalculateCostAndValidateCompatibility()
    {
        var physicalOrder = new Order();
        physicalOrder.AddProduct(new PhysicalProduct("Кофе", new Money(500m), 2m));
        var digitalOrder = new Order();
        digitalOrder.AddProduct(new DigitalProduct("Курс", new Money(800m), 100m));

        Assert.Equal(new Money(460m), new CourierDelivery().CalculateCost(physicalOrder));
        Assert.Equal(new Money(0m), new PickupDelivery().CalculateCost(physicalOrder));
        Assert.Equal(new Money(0m), new DigitalDelivery().CalculateCost(digitalOrder));
        Assert.Throws<InvalidOperationException>(() => new DigitalDelivery().CalculateCost(physicalOrder));
        Assert.Throws<InvalidOperationException>(() => new CourierDelivery().CalculateCost(digitalOrder));
    }

    [Fact]
    public void CheckoutUsesInjectedDiscountAndDeliveryPolicies()
    {
        var order = new Order();
        order.AddProduct(new PhysicalProduct("Кофе", new Money(1_000m), 1m));

        var result = new CheckoutService().Checkout(
            order,
            new PercentageDiscount(10m),
            new CourierDelivery());

        Assert.Equal(new Money(1_000m), result.ProductsTotal);
        Assert.Equal(new Money(100m), result.Discount);
        Assert.Equal(new Money(380m), result.DeliveryCost);
        Assert.Equal(new Money(1_280m), result.Total);
    }

    [Fact]
    public void CheckoutRejectsEmptyOrders()
    {
        Assert.Throws<InvalidOperationException>(() =>
            new CheckoutService().Checkout(new Order(), new NoDiscount(), new DigitalDelivery()));
    }

    [Fact]
    public void PaymentMethodsHandleCardAndCash()
    {
        var total = new Money(1_000m);
        IPaymentMethod card = new CardPayment();
        IPaymentMethod cash = new CashPayment(new Money(1_200m));

        Assert.Equal(new Money(0m), card.Pay(total));
        Assert.Equal(new Money(200m), cash.Pay(total));
        Assert.Throws<InvalidOperationException>(() => new CashPayment(new Money(999m)).Pay(total));
    }
}
