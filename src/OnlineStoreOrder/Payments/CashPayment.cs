using OnlineStoreOrder.Models;

namespace OnlineStoreOrder.Payments;

public sealed class CashPayment : IPaymentMethod
{
    public CashPayment(Money cashAmount)
    {
        CashAmount = cashAmount;
    }

    public Money CashAmount { get; }

    public Money Pay(Money amount)
    {
        if (CashAmount < amount)
        {
            throw new InvalidOperationException("Недостаточно наличных для оплаты заказа.");
        }

        return CashAmount - amount;
    }
}
