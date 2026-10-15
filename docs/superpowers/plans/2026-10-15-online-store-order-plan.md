# Online Store Order Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Build and publish a tested C# console application that models an online-store order with polymorphic products, discounts, delivery, payment, and a complete console demonstration.

**Architecture:** A .NET solution contains one console application and one xUnit test project. Domain classes are separated by responsibility into models, discounts, delivery, payments, and services; checkout consumes the abstract policies/interfaces and never branches on concrete product types.

**Tech Stack:** C#, .NET SDK, xUnit, Git, GitHub.

**Spec:** `docs/superpowers/specs/2026-10-15-online-store-order-design.md`

## Global Constraints

- Money is backed by `decimal` and rejects negative values.
- Product-specific behavior is implemented through polymorphism; no concrete product checks in order/checkout.
- Subscription terms of 12 months or longer receive a 10% unit-price discount.
- Courier costs 300 RUB plus 80 RUB per physical kilogram; pickup and digital delivery are free.
- Checkout total is merchandise minus order discount plus delivery.
- Invalid quantities, incompatible delivery, empty orders, and insufficient cash are handled explicitly.

---

### Task 1: Scaffold Solution and Failing Domain Tests

**Files:**
- Create: `OnlineStoreOrder.sln`
- Create: `src/OnlineStoreOrder/OnlineStoreOrder.csproj`
- Create: `tests/OnlineStoreOrder.Tests/OnlineStoreOrder.Tests.csproj`
- Create: `tests/OnlineStoreOrder.Tests/DomainTests.cs`

- [ ] **Step 1: Scaffold the solution and projects**

Run `dotnet new sln`, `dotnet new console`, and `dotnet new xunit`; add both projects to the solution and add the project reference from tests to the application.

- [ ] **Step 2: Write failing tests for money and product contracts**

Test negative money construction, addition, subtraction, integer/fraction multiplication, subscription pricing, and physical/digital delivery characteristics. Tests should reference the intended public APIs and fail because the domain types do not exist yet.

- [ ] **Step 3: Run the focused tests and verify expected compilation failure**

Run `dotnet test tests/OnlineStoreOrder.Tests/OnlineStoreOrder.Tests.csproj`; confirm the failure is missing domain types/API, not a test setup error.

### Task 2: Implement Money and Product Hierarchy

**Files:**
- Create: `src/OnlineStoreOrder/Models/Money.cs`
- Create: `src/OnlineStoreOrder/Models/Product.cs`
- Create: `src/OnlineStoreOrder/Models/PhysicalProduct.cs`
- Create: `src/OnlineStoreOrder/Models/DigitalProduct.cs`
- Create: `src/OnlineStoreOrder/Models/SubscriptionProduct.cs`

- [ ] **Step 1: Implement the smallest Money value type for the failing tests**

Use a readonly struct with a non-negative `decimal Amount`; implement `+`, `-`, `*` with `int`, `*` with `decimal`, equality, and `ToString` using `ru-RU` formatting and `₽`.

- [ ] **Step 2: Implement abstract Product and concrete products**

Expose `Name`, `GetUnitPrice()`, `RequiresPhysicalDelivery`, and `GetWeightKg()`. Physical products return their price and weight; digital products return price and zero weight; subscriptions calculate monthly price times months and apply 10% when months >= 12.

- [ ] **Step 3: Run tests and confirm green**

Run `dotnet test tests/OnlineStoreOrder.Tests/OnlineStoreOrder.Tests.csproj` and confirm all current tests pass.

### Task 3: Implement Orders and Discounts with Tests

**Files:**
- Create: `src/OnlineStoreOrder/Models/OrderItem.cs`
- Create: `src/OnlineStoreOrder/Models/Order.cs`
- Create: `src/OnlineStoreOrder/Discounts/DiscountPolicy.cs`
- Create: `src/OnlineStoreOrder/Discounts/NoDiscount.cs`
- Create: `src/OnlineStoreOrder/Discounts/PercentageDiscount.cs`
- Create: `src/OnlineStoreOrder/Discounts/FixedDiscount.cs`
- Modify: `tests/OnlineStoreOrder.Tests/DomainTests.cs`

- [ ] **Step 1: Add failing tests for order aggregation and discount validation**

Cover both `AddProduct` overloads, rejection of quantities below one, line totals, merchandise totals, physical presence, total physical weight, percentage bounds, and fixed discounts larger than the merchandise total.

- [ ] **Step 2: Implement Order and OrderItem**

Store a read-only list of order items. Let `OrderItem` calculate `Product.GetUnitPrice() * Quantity`; have `Order` aggregate totals and weight through product polymorphic properties/methods.

- [ ] **Step 3: Implement DiscountPolicy implementations**

Define `CalculateDiscount(Order order)` abstractly. Return zero for `NoDiscount`, merchandise total multiplied by percentage for `PercentageDiscount`, and the configured fixed amount for `FixedDiscount` after constructor/order validation.

- [ ] **Step 4: Run tests and confirm green**

Run the complete test project and confirm all order and discount tests pass.

### Task 4: Implement Delivery, Checkout, and Payment with Tests

**Files:**
- Create: `src/OnlineStoreOrder/Delivery/DeliveryMethod.cs`
- Create: `src/OnlineStoreOrder/Delivery/CourierDelivery.cs`
- Create: `src/OnlineStoreOrder/Delivery/PickupDelivery.cs`
- Create: `src/OnlineStoreOrder/Delivery/DigitalDelivery.cs`
- Create: `src/OnlineStoreOrder/Payments/IPaymentMethod.cs`
- Create: `src/OnlineStoreOrder/Payments/CardPayment.cs`
- Create: `src/OnlineStoreOrder/Payments/CashPayment.cs`
- Create: `src/OnlineStoreOrder/Services/CheckoutResult.cs`
- Create: `src/OnlineStoreOrder/Services/CheckoutService.cs`
- Modify: `tests/OnlineStoreOrder.Tests/DomainTests.cs`

- [ ] **Step 1: Add failing tests for delivery, checkout, and cash payment**

Cover courier pricing by kilograms, free pickup/digital delivery, invalid delivery combinations, checkout formula with arbitrary policy implementations, successful card payment, sufficient cash with change, and insufficient cash exceptions.

- [ ] **Step 2: Implement delivery methods**

Define an abstract `CalculateCost(Order order)` and compatibility validation. Courier and pickup require physical products; digital delivery requires none. Mixed orders use physical aggregates for courier/pickup while digital items remain available without separate shipping.

- [ ] **Step 3: Implement payment methods and checkout**

Define `IPaymentMethod.Pay(Money amount)`. Card payment returns a success result; cash payment returns change or throws when cash is insufficient. `CheckoutService.Checkout` rejects empty orders, validates delivery, computes discount and final total, and returns a `CheckoutResult`.

- [ ] **Step 4: Run tests and confirm green**

Run `dotnet test tests/OnlineStoreOrder.Tests/OnlineStoreOrder.Tests.csproj` and confirm all tests pass.

### Task 5: Console Demonstration and Documentation

**Files:**
- Modify: `src/OnlineStoreOrder/Program.cs`
- Create: `README.md`
- Create: `.gitignore`

- [ ] **Step 1: Implement the requested Main demonstration**

Create physical, digital, and 12-month subscription products; use both add overloads; print lines and totals; check out with 10% percentage discount and courier delivery; print merchandise, discount, delivery, and final values; pay through `IPaymentMethod`; demonstrate and catch all four required invalid scenarios.

- [ ] **Step 2: Document build, test, run, design, and GitHub usage**

Include Visual Studio opening instructions, CLI commands, class responsibilities, and a sample of expected behavior without hard-coding volatile output values.

- [ ] **Step 3: Run build, tests, and console application**

Run `dotnet build`, `dotnet test`, and `dotnet run --project src/OnlineStoreOrder/OnlineStoreOrder.csproj`; inspect output for the required demonstration and error handling.

### Task 6: Review, Commit, and Publish

**Files:**
- Git metadata only, plus any fixes found during review.

- [ ] **Step 1: Inspect the complete diff and run fresh verification**

Check `git diff --check`, `dotnet build`, `dotnet test`, and `dotnet run`; verify no concrete product type checks exist with `rg " is (PhysicalProduct|DigitalProduct|SubscriptionProduct)"`.

- [ ] **Step 2: Commit the finished solution**

Create a focused commit with message `feat: add online store order console app`.

- [ ] **Step 3: Publish to GitHub**

Create or select the GitHub repository, add it as `origin`, push the default branch, and record the resulting repository URL. If GitHub authentication or repository creation is unavailable, leave the local commit ready and report the exact blocker.
