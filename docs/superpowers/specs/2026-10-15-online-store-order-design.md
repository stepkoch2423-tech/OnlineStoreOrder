# Online Store Order Design

## Goal

Create a C# console application that models adding physical, digital, and subscription products to an order, applying discounts, selecting compatible delivery, and completing payment.

## Architecture

The domain is split into small polymorphic components. `Product` defines the common product contract while each concrete product calculates its own unit price and delivery characteristics. `Order` owns `OrderItem` instances and exposes aggregate calculations without checking concrete product types. `DiscountPolicy`, `DeliveryMethod`, and `IPaymentMethod` are extension points consumed by `CheckoutService`, so checkout does not depend on specific implementations.

## Components

- `Money`: immutable value type backed by `decimal`; rejects negative values, supports addition, subtraction, multiplication by `int` and `decimal`, and formats values as Russian rubles.
- Products: `PhysicalProduct`, `DigitalProduct`, and `SubscriptionProduct`. Subscription prices include a 10% term discount when the duration is at least 12 months.
- Order: validates quantities, supports `AddProduct(Product)` and `AddProduct(Product, int)`, calculates item totals, physical presence, and physical weight.
- Discounts: `NoDiscount`, `PercentageDiscount`, and `FixedDiscount`. Percentages are restricted to 0..100; fixed discounts cannot exceed the merchandise total.
- Delivery: courier charges 300 RUB plus 80 RUB per kilogram; pickup is free; digital delivery is free and valid only for orders without physical products. Courier and pickup require at least one physical product.
- Payments: card payment succeeds; cash payment validates sufficient cash and returns change.
- Checkout: rejects empty orders, validates delivery compatibility, and calculates `items - discount + delivery`.

## User-visible demonstration

`Program.Main` creates one physical product, one digital product, and a 12-month subscription; adds them using both overloads; prints each line and aggregate; checks out with a 10% percentage discount and courier delivery; prints merchandise total, discount, delivery, and final total; then pays through `IPaymentMethod`. It also demonstrates handling of invalid quantity, empty order, incompatible delivery, and insufficient cash using readable messages.

## Error handling

Invalid domain inputs throw `ArgumentException` or `ArgumentOutOfRangeException`. Money subtraction that would become negative throws `InvalidOperationException`. Cash shortfalls and invalid delivery choices throw `InvalidOperationException`. The console demonstration catches these expected exceptions and continues.

## Testing

The solution includes an xUnit test project covering money arithmetic and validation, product polymorphism, order quantity and aggregate rules, discounts, delivery compatibility and pricing, checkout totals, and cash payment validation.

## Constraints

- Target modern .NET (the installed SDK will determine the exact target framework).
- No concrete product type checks such as `product is PhysicalProduct` in order or checkout logic.
- No external runtime services are required.
- Include a README with build, test, run, and GitHub publication instructions.
