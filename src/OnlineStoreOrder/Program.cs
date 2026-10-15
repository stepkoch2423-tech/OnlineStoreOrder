using OnlineStoreOrder.Delivery;
using OnlineStoreOrder.Discounts;
using OnlineStoreOrder.Models;
using OnlineStoreOrder.Payments;
using OnlineStoreOrder.Services;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("=== Интернет-магазин: оформление заказа ===");

var coffee = new PhysicalProduct("Кофе в зернах", new Money(1_250m), 1.2m);
var course = new DigitalProduct("Онлайн-курс C#", new Money(2_000m), 850m);
var subscription = new SubscriptionProduct("Музыкальная подписка", new Money(450m), 12);

var order = new Order();
order.AddProduct(coffee);
order.AddProduct(course, 2);
order.AddProduct(subscription);

Console.WriteLine("\nСостав заказа:");
foreach (var item in order.Items)
{
    Console.WriteLine($"- {item.Product.Name}: {item.Quantity} x {item.Product.GetUnitPrice()} = {item.Total}");
}

var checkout = new CheckoutService();
var result = checkout.Checkout(order, new PercentageDiscount(10m), new CourierDelivery());

Console.WriteLine("\nРасчёт заказа:");
Console.WriteLine($"Стоимость товаров: {result.ProductsTotal}");
Console.WriteLine($"Скидка: {result.Discount}");
Console.WriteLine($"Доставка: {result.DeliveryCost}");
Console.WriteLine($"Итого: {result.Total}");

IPaymentMethod payment = new CardPayment();
payment.Pay(result.Total);
Console.WriteLine("Оплата картой выполнена успешно.");

Console.WriteLine("\nПроверка обработки ошибок:");
Try("Некорректное количество", () => order.AddProduct(course, 0));
Try("Пустой заказ", () => checkout.Checkout(new Order(), new NoDiscount(), new DigitalDelivery()));
Try("Неподходящая доставка", () => checkout.Checkout(order, new NoDiscount(), new DigitalDelivery()));
Try("Недостаточная сумма наличных", () => new CashPayment(new Money(1m)).Pay(result.Total));

static void Try(string description, Action action)
{
    try
    {
        action();
        Console.WriteLine($"ОШИБКА: сценарий '{description}' не был отклонён.");
    }
    catch (Exception exception) when (exception is ArgumentException or InvalidOperationException)
    {
        Console.WriteLine($"- {description}: обработано ({exception.Message})");
    }
}
