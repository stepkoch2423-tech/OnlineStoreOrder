namespace OnlineStoreOrder.Models;

public sealed class DigitalProduct : Product
{
    public DigitalProduct(string name, Money price, decimal fileSizeMb)
        : base(name)
    {
        if (fileSizeMb < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(fileSizeMb), "Размер файла не может быть отрицательным.");
        }

        Price = price;
        FileSizeMb = fileSizeMb;
    }

    public Money Price { get; }

    public decimal FileSizeMb { get; }

    public override Money GetUnitPrice() => Price;

    public override bool RequiresPhysicalDelivery => false;

    public override decimal GetWeightKg() => 0m;
}
