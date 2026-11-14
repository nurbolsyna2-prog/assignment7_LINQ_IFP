namespace OnlineShop.Models;

/// <summary>Immutable business rules. Bound from the "Pricing" section of appsettings.json.</summary>
public record PricingRules
{
    public string Currency { get; init; } = "$";
    public decimal TaxRate { get; init; } = 0.12m;
    public decimal FreeShippingThreshold { get; init; } = 100m;
    public decimal ShippingFee { get; init; } = 5m;
    public int BulkQuantity { get; init; } = 5;          // qty of one product that triggers the bulk discount
    public decimal BulkDiscountRate { get; init; } = 0.10m;

    public static PricingRules Default => new();
}

public record Promo(string Code, decimal Rate);

public record PriceBreakdown(
    decimal Subtotal,
    decimal BulkDiscount,
    decimal PromoDiscount,
    decimal Taxable,
    decimal Tax,
    decimal Shipping,
    decimal Total);
