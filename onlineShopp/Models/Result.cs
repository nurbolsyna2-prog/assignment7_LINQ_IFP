namespace OnlineShop.Models;

/// <summary>Simple success/failure value, used instead of exceptions for expected user errors.</summary>
public record Result(bool IsOk, string? Error = null)
{
    public static Result Success { get; } = new(true);
    public static Result Fail(string error) => new(false, error);
}
