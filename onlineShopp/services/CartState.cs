using OnlineShop.Models;

namespace OnlineShop.Services;

/// <summary>
/// State container: one instance per user session. Components inject it, read from it, and subscribe
/// to OnChange so the UI refreshes whenever the cart changes. The item list is replaced, never mutated.
/// </summary>
public class CartState(PricingRules rules)
{
    public const int MaxQuantity = 99;

    public IReadOnlyList<CartItem> Items { get; private set; } = [];
    public Promo? AppliedPromo { get; private set; }
    public event Action? OnChange;

    public int TotalQuantity => Items.Sum(i => i.Quantity);
    public PriceBreakdown Breakdown => Pricing.Calculate(Items, rules, AppliedPromo);

    public void Add(Product product)
    {
        Items = Items.Any(i => i.Product.Id == product.Id)
            ? Items.Select(i => i.Product.Id == product.Id
                    ? i with { Quantity = Math.Min(i.Quantity + 1, MaxQuantity) }
                    : i).ToList()
            : [.. Items, new CartItem(product, 1)];
        Notify();
    }

    public Result SetQuantity(int productId, int quantity)
    {
        if (quantity < 1 || quantity > MaxQuantity)
            return Result.Fail($"Quantity must be between 1 and {MaxQuantity}.");

        Items = Items.Select(i => i.Product.Id == productId ? i with { Quantity = quantity } : i).ToList();
        Notify();
        return Result.Success;
    }

    public void Remove(int productId)
    {
        Items = Items.Where(i => i.Product.Id != productId).ToList();
        Notify();
    }

    public Result ApplyPromo(string? code)
    {
        var promo = Pricing.FindPromo(code);
        if (promo is null)
            return Result.Fail("This promo code is not valid.");

        AppliedPromo = promo;
        Notify();
        return Result.Success;
    }

    public void RemovePromo()
    {
        AppliedPromo = null;
        Notify();
    }

    public void Clear()
    {
        Items = [];
        AppliedPromo = null;
        Notify();
    }

    private void Notify() => OnChange?.Invoke();
}
