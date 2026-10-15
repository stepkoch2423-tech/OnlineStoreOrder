namespace OnlineStoreOrder.Models;

public sealed class PhysicalProduct : Product
{
    public PhysicalProduct(string name, Money price, decimal weightKg)
        : base(name)
    {
        if (weightKg < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(weightKg), "Вес не может быть отрицательным.");
        }

        Price = price;
        WeightKg = weightKg;
    }

    public Money Price { get; }

    public decimal WeightKg { get; }

    public override Money GetUnitPrice() => Price;

    public override bool RequiresPhysicalDelivery => true;

    public override decimal GetWeightKg() => WeightKg;
}
