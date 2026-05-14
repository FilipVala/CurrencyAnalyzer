using CurrencyAnalyzer.Core.Data;
using CurrencyAnalyzer.Core.Models;
using CurrencyAnalyzer.Core.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using CurrencyAnalyzer.Core.Interfaces;
using Xunit;

namespace CurrencyAnalyzer.Tests;

public class UserSettingsServiceTests
{
    private static AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task GetSettings_ReturnsDefault_WhenDatabaseEmpty()
    {
        var dbContext = CreateDbContext();
        var logMock = new Mock<ILogService>();
        var service = new UserSettingsService(dbContext, logMock.Object);

        var result = await service.GetSettingsAsync();

        Assert.NotNull(result);
        Assert.Equal("EUR", result.BaseCurrency);
        Assert.Contains("USD", result.SelectedCurrencies);
    }

    [Fact]
    public async Task UpdateSettings_PersistsChanges()
    {
        var dbContext = CreateDbContext();
        var logMock = new Mock<ILogService>();
        var service = new UserSettingsService(dbContext, logMock.Object);

        var settings = await service.GetSettingsAsync();

        settings.BaseCurrency = "USD";
        settings.SelectedCurrencies = new List<string> { "USD", "EUR" };

        await service.SaveSettingsAsync(settings);

        var updated = await service.GetSettingsAsync();

        Assert.Equal("USD", updated.BaseCurrency);
        Assert.Contains("EUR", updated.SelectedCurrencies);
    }

    [Fact]
    public async Task Reset_RemovesAndRecreatesDefaults()
    {
        var dbContext = CreateDbContext();
        var logMock = new Mock<ILogService>();
        var service = new UserSettingsService(dbContext, logMock.Object);

        var settings = await service.GetSettingsAsync();
        settings.BaseCurrency = "USD";
        await service.SaveSettingsAsync(settings);

        await service.ResetToDefaultAsync();

        var reset = await service.GetSettingsAsync();

        Assert.Equal("EUR", reset.BaseCurrency);
        Assert.Contains("GBP", reset.SelectedCurrencies);
    }

    [Fact]
    public async Task Settings_PersistBetweenCalls()
    {
        var dbContext = CreateDbContext();
        var logMock = new Mock<ILogService>();
        var service = new UserSettingsService(dbContext, logMock.Object);

        var s1 = await service.GetSettingsAsync();
        s1.BaseCurrency = "CZK";
        await service.SaveSettingsAsync(s1);

        var s2 = await service.GetSettingsAsync();

        Assert.Equal("CZK", s2.BaseCurrency);
    }
}