namespace OnlineStoreOrder.Models;

public sealed class Order
{
    private readonly List<OrderItem> _items = [];

    public IReadOnlyList<OrderItem> Items => _items;

    public Money ProductsTotal => _items.Aggregate(Money.Zero, (total, item) => total + item.Total);

    public bool ContainsPhysicalProducts => _items.Any(item => item.Product.RequiresPhysicalDelivery);

    public decimal TotalPhysicalWeightKg => _items
        .Where(item => item.Product.RequiresPhysicalDelivery)
        .Sum(item => item.Product.GetWeightKg() * item.Quantity);

    public void AddProduct(Product product) => AddProduct(product, 1);

    public void AddProduct(Product product, int quantity)
    {
        _items.Add(new OrderItem(product, quantity));
    }
}
