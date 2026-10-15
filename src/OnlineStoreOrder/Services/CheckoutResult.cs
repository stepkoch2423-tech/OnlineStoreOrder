using OnlineStoreOrder.Models;

namespace OnlineStoreOrder.Services;

public sealed record CheckoutResult(
    Money ProductsTotal,
    Money Discount,
    Money DeliveryCost,
    Money Total);
