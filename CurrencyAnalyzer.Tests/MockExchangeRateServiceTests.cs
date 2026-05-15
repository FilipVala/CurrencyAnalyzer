using CurrencyAnalyzer.Core.Services;

namespace CurrencyAnalyzer.Tests;

public class MockExchangeRateServiceTests
{
    [Fact]
    public async Task GetLatestRatesAsync_ReturnsMockData()
    {
        var service = new MockExchangeRateService();

        var result = await service.GetLatestRatesAsync(
            "EUR",
            new[] { "USD", "CZK" });

        Assert.NotNull(result);

        Assert.Equal("EUR", result.Base);

        Assert.True(result.Rates.ContainsKey("USD"));

        Assert.True(result.Rates.ContainsKey("CZK"));
    }

    [Fact]
    public async Task GetHistoricalRatesAsync_ReturnsHistoricalData()
    {
        var service = new MockExchangeRateService();

        var result = await service.GetHistoricalRatesAsync(
            "EUR",
            new[] { "USD" },
            DateTime.UtcNow.AddDays(-7),
            DateTime.UtcNow);

        Assert.NotNull(result);

        Assert.NotEmpty(result.Rates);
    }
}