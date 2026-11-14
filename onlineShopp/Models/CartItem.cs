namespace OnlineShop.Models;

public record CartItem(Product Product, int Quantity)
{
    public decimal LineTotal => Product.Price * Quantity;
}
