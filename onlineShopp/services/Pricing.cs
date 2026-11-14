using OnlineShop.Models;

namespace OnlineShop.Services;

/// <summary>
/// The calculating system. Pure functions only (functional style): the same input always gives
/// the same output, nothing is mutated, there are no side effects. That makes it trivial to unit test.
/// </summary>
public static class Pricing
{
    private static readonly IReadOnlyDictionary<string, decimal> PromoCodes =
        new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase)
        {
            ["SAVE10"] = 0.10m,
            ["WELCOME5"] = 0.05m
        };

    public static Promo? FindPromo(string? code) =>
        !string.IsNullOrWhiteSpace(code) && PromoCodes.TryGetValue(code.Trim(), out var rate)
            ? new Promo(code.Trim().ToUpperInvariant(), rate)
            : null;

    private static decimal Round(decimal value) => Math.Round(value, 2, MidpointRounding.AwayFromZero);

    public static PriceBreakdown Calculate(IEnumerable<CartItem> items, PricingRules rules, Promo? promo)
    {
        var list = items.ToList();
        if (list.Count == 0)
            return new PriceBreakdown(0, 0, 0, 0, 0, 0, 0);

        var subtotal = Round(list.Sum(i => i.LineTotal));

        // Bulk discount is applied per line: buying N or more of one product.
        var bulk = Round(list
            .Where(i => i.Quantity >= rules.BulkQuantity)
            .Sum(i => i.LineTotal * rules.BulkDiscountRate));

        var afterBulk = subtotal - bulk;
        var promoDiscount = promo is null ? 0m : Round(afterBulk * promo.Rate);
        var taxable = afterBulk - promoDiscount;
        var tax = Round(taxable * rules.TaxRate);
        var shipping = taxable >= rules.FreeShippingThreshold ? 0m : rules.ShippingFee;
        var total = taxable + tax + shipping;

        return new PriceBreakdown(subtotal, bulk, promoDiscount, taxable, tax, shipping, total);
    }
}
