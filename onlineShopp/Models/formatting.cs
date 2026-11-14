namespace OnlineShop.Models;

public static class Formatting
{
    public static string Money(this decimal value, string currency) => $"{currency}{value:N2}";
}
