using OnlineStoreOrder.Models;

namespace OnlineStoreOrder.Payments;

public interface IPaymentMethod
{
    Money Pay(Money amount);
}
