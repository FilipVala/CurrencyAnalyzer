using CurrencyAnalyzer.Components;
using CurrencyAnalyzer.Core.Clients;
using CurrencyAnalyzer.Core.Data;
using CurrencyAnalyzer.Core.Interfaces;
using CurrencyAnalyzer.Core.Services;
using CurrencyAnalyzer.Middleware;

using Microsoft.EntityFrameworkCore;

using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// =============================
// RAZOR COMPONENTS
// =============================

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// =============================
// MUD BLAZOR
// =============================

builder.Services.AddMudServices();

// =============================
// DATABASE
// =============================

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=CurrencyAnalyzer.db"));

// =============================
// SERVICES
// =============================

builder.Services.AddScoped<IUserSettingsService, UserSettingsService>();

builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();

builder.Services.AddScoped<ILogService, LogService>();

builder.Services.AddSingleton<IAuthService, AuthService>();

builder.Services.AddScoped<MockExchangeRateService>();

builder.Services.AddHttpClient<ExchangeRateClient>();

// MOCK / REAL API

// builder.Services.AddScoped<IExchangeRateService, MockExchangeRateService>();

builder.Services.AddScoped<IExchangeRateService, ExchangeRateService>();

// =============================
// BUILD APP
// =============================

var app = builder.Build();

// =============================
// DATABASE INIT
// =============================

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider
        .GetRequiredService<AppDbContext>();

    db.Database.EnsureCreated();
}

// =============================
// ERROR HANDLING
// =============================

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error",
        createScopeForErrors: true);

    app.UseHsts();
}

// =============================
// MIDDLEWARE
// =============================

// app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseAntiforgery();

// =============================
// ROUTING
// =============================

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// =============================
// RUN
// =============================

app.Run();