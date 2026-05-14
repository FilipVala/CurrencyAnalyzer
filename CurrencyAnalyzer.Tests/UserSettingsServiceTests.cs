using CurrencyAnalyzer.Core.Data;
using CurrencyAnalyzer.Core.Interfaces;
using CurrencyAnalyzer.Core.Models;
using CurrencyAnalyzer.Core.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace CurrencyAnalyzer.Tests;

public class UserSettingsServiceTests
{
    private AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task AddCurrencyAsync_AddsCurrency()
    {
        var db = CreateDbContext();

        db.UserSettings.Add(new UserSettings
        {
            BaseCurrency = "EUR",
            SelectedCurrencies = new List<string> { "USD" }
        });

        await db.SaveChangesAsync();

        var logService = new Mock<ILogService>();

        var service = new UserSettingsService(
            db,
            logService.Object);

        await service.AddCurrencyAsync("CZK");

        var settings = await db.UserSettings.FirstAsync();

        Assert.Contains("CZK", settings.SelectedCurrencies);
    }

    [Fact]
    public async Task RemoveCurrencyAsync_RemovesCurrency()
    {
        var db = CreateDbContext();

        db.UserSettings.Add(new UserSettings
        {
            BaseCurrency = "EUR",
            SelectedCurrencies = new List<string>
            {
                "USD",
                "CZK"
            }
        });

        await db.SaveChangesAsync();

        var logService = new Mock<ILogService>();

        var service = new UserSettingsService(
            db,
            logService.Object);

        await service.RemoveCurrencyAsync("CZK");

        var settings = await db.UserSettings.FirstAsync();

        Assert.DoesNotContain("CZK", settings.SelectedCurrencies);
    }

    [Fact]
    public async Task SetBaseCurrencyAsync_ChangesBaseCurrency()
    {
        var db = CreateDbContext();

        db.UserSettings.Add(new UserSettings
        {
            BaseCurrency = "EUR",
            SelectedCurrencies = new List<string> { "USD" }
        });

        await db.SaveChangesAsync();

        var logService = new Mock<ILogService>();

        var service = new UserSettingsService(
            db,
            logService.Object);

        await service.SetBaseCurrencyAsync("CZK");

        var settings = await db.UserSettings.FirstAsync();

        Assert.Equal("CZK", settings.BaseCurrency);
    }

    [Fact]
    public async Task ResetToDefaultAsync_ResetsSettings()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var context = new AppDbContext(options);

        context.UserSettings.Add(new UserSettings
        {
            BaseCurrency = "USD",
            SelectedCurrencies = new List<string> { "EUR", "CZK" }
        });

        await context.SaveChangesAsync();

        var logMock = new Mock<ILogService>();

        var service = new UserSettingsService(context, logMock.Object);

        await service.ResetToDefaultAsync();

        var settings = await context.UserSettings.FirstAsync();

        Assert.Equal("EUR", settings.BaseCurrency);
        Assert.Contains("USD", settings.SelectedCurrencies);
    }


}