using OnlineShop.Models;

namespace OnlineShop.Services;

/// <summary>In-memory catalog. Replace with EF Core + a database for production (see README).</summary>
public class ProductCatalog
{
    private readonly IReadOnlyList<Product> _products =
    [
        new(1, "Wireless Mouse", "Accessories", 14.99m),
        new(2, "Mechanical Keyboard", "Accessories", 59.90m),
        new(3, "USB-C Cable 2m", "Accessories", 7.50m),
        new(4, "27\" Monitor", "Displays", 189.00m),
        new(5, "Laptop Stand", "Accessories", 24.00m),
        new(6, "Webcam HD", "Video", 39.99m),
        new(7, "Headphones", "Audio", 79.00m),
        new(8, "Bluetooth Speaker", "Audio", 45.50m),
        new(9, "Notebook A5", "Stationery", 3.20m),
        new(10, "Gel Pen Set", "Stationery", 5.75m)
    ];

    public IReadOnlyList<Product> All => _products;

    public Product? GetById(int id) => _products.FirstOrDefault(p => p.Id == id);

    public IEnumerable<Product> Search(string? text) =>
        string.IsNullOrWhiteSpace(text)
            ? _products
            : _products.Where(p =>
                p.Name.Contains(text, StringComparison.OrdinalIgnoreCase) ||
                p.Category.Contains(text, StringComparison.OrdinalIgnoreCase));
}
