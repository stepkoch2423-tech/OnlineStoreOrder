namespace OnlineStoreOrder.Models;

public sealed class SubscriptionProduct : Product
{
    public SubscriptionProduct(string name, Money monthlyPrice, int months)
        : base(name)
    {
        if (months < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(months), "Продолжительность подписки должна быть больше нуля.");
        }

        MonthlyPrice = monthlyPrice;
        Months = months;
    }

    public Money MonthlyPrice { get; }

    public int Months { get; }

    public override Money GetUnitPrice()
    {
        var fullPrice = MonthlyPrice * Months;
        return Months >= 12 ? fullPrice * 0.90m : fullPrice;
    }

    public override bool RequiresPhysicalDelivery => false;

    public override decimal GetWeightKg() => 0m;
}
