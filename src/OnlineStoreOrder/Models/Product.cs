namespace OnlineStoreOrder.Models;

public abstract class Product
{
    protected Product(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Название товара не может быть пустым.", nameof(name));
        }

        Name = name;
    }

    public string Name { get; }

    public abstract Money GetUnitPrice();

    public abstract bool RequiresPhysicalDelivery { get; }

    public abstract decimal GetWeightKg();
}
