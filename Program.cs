using CurrencyAnalyzer.Components;
using CurrencyAnalyzer.Core.Data;
using CurrencyAnalyzer.Core.Interfaces;
using CurrencyAnalyzer.Core.Services;
using Microsoft.EntityFrameworkCore;




var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// === Služby ===
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=CurrencyAnalyzer.db"));

builder.Services.AddScoped<IUserSettingsService, UserSettingsService>();
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddSingleton<IAuthService, AuthService>();
builder.Services.AddScoped<MockExchangeRateService>();
builder.Services.AddScoped<ExchangeRateService>();

// DŮLEŽITÉ: Pouze jeden z těchto dvou!
//builder.Services.AddScoped<IExchangeRateService, MockExchangeRateService>();     // ← Mock (doporučeno teď)
builder.Services.AddScoped<IExchangeRateService, ExchangeRateService>();     // ← reálné API (až později)

// HTTP client pro případ, že bys chtěl reálnou službu
builder.Services.AddHttpClient<ExchangeRateService>(client =>
{
    client.BaseAddress = new Uri("https://api.exchangerate.host");
    client.Timeout = TimeSpan.FromSeconds(10);
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    db.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();