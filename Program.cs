using CurrencyAnalyzer.Components;
using CurrencyAnalyzer.Core.Interfaces;
using CurrencyAnalyzer.Core.Services;
using CurrencyAnalyzer.Core.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// === Přidat tyto řádky ===
builder.Services.AddHttpClient<IExchangeRateService, ExchangeRateService>(client =>
{
    client.BaseAddress = new Uri("https://api.exchangerate.host");
    client.Timeout = TimeSpan.FromSeconds(10);
});

// === Služby z Core projektu ===
builder.Services.AddHttpClient<IExchangeRateService, ExchangeRateService>();

//builder.Services.AddScoped<IExchangeRateService, ExchangeRateService>();
builder.Services.AddScoped<IExchangeRateService, MockExchangeRateService>();
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
builder.Services.AddScoped<IUserSettingsService, UserSettingsService>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=CurrencyAnalyzer.db"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
