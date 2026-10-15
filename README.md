# OnlineStoreOrder

Консольное приложение на C#, моделирующее оформление заказа в интернет-магазине.

## Что реализовано

- `Money` с безопасными арифметическими операциями и форматом `1 250,00 ₽`.
- Полиморфные физические, цифровые товары и подписки.
- Скидка 10% для подписки сроком от 12 месяцев.
- Заказ с количеством товаров, итоговой стоимостью и расчётом физического веса.
- `NoDiscount`, процентная и фиксированная скидки.
- Курьерская доставка, самовывоз и цифровая доставка.
- Оплата картой и наличными со сдачей.
- `CheckoutService` с формулой `товары - скидка + доставка`.
- xUnit-тесты для основных требований и ошибок.

## Запуск в Visual Studio

1. Откройте файл `OnlineStoreOrder.slnx`.
2. Установите `OnlineStoreOrder` стартовым проектом.
3. Запустите проект клавишей `Ctrl+F5` или `F5`.

## Запуск из терминала

```powershell
dotnet build OnlineStoreOrder.slnx
dotnet test OnlineStoreOrder.slnx
dotnet run --project src/OnlineStoreOrder/OnlineStoreOrder.csproj
```

## Структура проекта

- `src/OnlineStoreOrder/Models` — деньги, товары и заказ.
- `src/OnlineStoreOrder/Discounts` — абстракция и реализации скидок.
- `src/OnlineStoreOrder/Delivery` — способы доставки.
- `src/OnlineStoreOrder/Payments` — способы оплаты.
- `src/OnlineStoreOrder/Services` — оформление заказа и результат расчёта.
- `tests/OnlineStoreOrder.Tests` — автоматические тесты.

В `Order` и `CheckoutService` нет проверок конкретного типа товара: характеристики и стоимость берутся через полиморфный API `Product`.
