using OnlineShop.Components;
using OnlineShop.Models;
using OnlineShop.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents().AddInteractiveServerComponents();

// Business rules come from appsettings.json, so they can change without recompiling.
var rules = builder.Configuration.GetSection("Pricing").Get<PricingRules>() ?? PricingRules.Default;
builder.Services.AddSingleton(rules);
builder.Services.AddSingleton<ProductCatalog>();
builder.Services.AddScoped<CartState>(); // one cart per user session (Blazor circuit)

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
app.Run();
