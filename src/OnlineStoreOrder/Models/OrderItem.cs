namespace OnlineStoreOrder.Models;

public sealed class OrderItem
{
    public OrderItem(Product product, int quantity)
    {
        Product = product ?? throw new ArgumentNullException(nameof(product));
        if (quantity < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), "Количество товара должно быть не меньше 1.");
        }

        Quantity = quantity;
    }

    public Product Product { get; }

    public int Quantity { get; }

    public Money Total => Product.GetUnitPrice() * Quantity;
}
