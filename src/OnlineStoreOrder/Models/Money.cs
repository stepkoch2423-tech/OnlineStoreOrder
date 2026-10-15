using System.Globalization;

namespace OnlineStoreOrder.Models;

public readonly struct Money : IEquatable<Money>
{
    public Money(decimal amount)
    {
        if (amount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Денежная сумма не может быть отрицательной.");
        }

        Amount = amount;
    }

    public decimal Amount { get; }

    public static Money Zero => new(0m);

    public static Money operator +(Money left, Money right) => new(left.Amount + right.Amount);

    public static Money operator -(Money left, Money right)
    {
        if (left.Amount < right.Amount)
        {
            throw new InvalidOperationException("Результат вычитания не может быть отрицательным.");
        }

        return new Money(left.Amount - right.Amount);
    }

    public static Money operator *(Money money, int multiplier) => new(money.Amount * multiplier);

    public static Money operator *(Money money, decimal multiplier) => new(money.Amount * multiplier);

    public static Money operator *(int multiplier, Money money) => money * multiplier;

    public static Money operator *(decimal multiplier, Money money) => money * multiplier;

    public bool Equals(Money other) => Amount == other.Amount;

    public override bool Equals(object? obj) => obj is Money other && Equals(other);

    public override int GetHashCode() => Amount.GetHashCode();

    public static bool operator ==(Money left, Money right) => left.Equals(right);

    public static bool operator !=(Money left, Money right) => !left.Equals(right);

    public static bool operator >(Money left, Money right) => left.Amount > right.Amount;

    public static bool operator <(Money left, Money right) => left.Amount < right.Amount;

    public static bool operator >=(Money left, Money right) => left.Amount >= right.Amount;

    public static bool operator <=(Money left, Money right) => left.Amount <= right.Amount;

    public override string ToString()
    {
        var culture = CultureInfo.GetCultureInfo("ru-RU");
        var formatted = Amount.ToString("N2", culture)
            .Replace(culture.NumberFormat.NumberGroupSeparator, " ", StringComparison.Ordinal);
        return $"{formatted} ₽";
    }
}
