using OnlineStoreOrder.Models;

namespace OnlineStoreOrder.Payments;

public sealed class CardPayment : IPaymentMethod
{
    public Money Pay(Money amount) => Money.Zero;
}
