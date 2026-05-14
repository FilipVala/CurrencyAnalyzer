using CurrencyAnalyzer.Core.Services;
using Xunit;

namespace CurrencyAnalyzer.Tests;

public class MockExchangeRateServiceTests
{
    [Fact]
    public async Task GetLatestRatesAsync_ReturnsRates()
    {
        var service = new MockExchangeRateService();

        var result = await service.GetLatestRatesAsync(
            "EUR",
            new[] { "USD", "CZK" });

        Assert.True(result.Success);

        Assert.Equal("EUR", result.Base);

        Assert.True(result.Rates.ContainsKey("USD"));
    }

    [Fact]
    public async Task GetHistoricalRatesAsync_ReturnsRates()
    {
        var service = new MockExchangeRateService();

        var result = await service.GetHistoricalRatesAsync(
            "EUR",
            new[] { "USD" },
            DateTime.UtcNow.AddDays(-1),
            DateTime.UtcNow);

        Assert.True(result.Success);
    }

    [Fact]
    public async Task GetCurrentRatesForAnalysisAsync_ReturnsRates()
    {
        var service = new MockExchangeRateService();

        var result = await service.GetCurrentRatesForAnalysisAsync(
            "EUR",
            new[] { "USD", "CZK" });

        Assert.True(result.ContainsKey("USD"));

        Assert.True(result.ContainsKey("CZK"));
    }

    [Fact]
    public async Task GetCurrentRatesForAnalysisAsync_ReturnsJpyRate()
    {
        var service = new MockExchangeRateService();

        var result = await service.GetCurrentRatesForAnalysisAsync(
            "EUR",
            new[] { "JPY" });

        Assert.Equal(160.0m, result["JPY"]);
    }

    [Fact]
    public async Task GetCurrentRatesForAnalysisAsync_ReturnsChfRate()
    {
        var service = new MockExchangeRateService();

        var result = await service.GetCurrentRatesForAnalysisAsync(
            "EUR",
            new[] { "CHF" });

        Assert.Equal(0.96m, result["CHF"]);
    }

    [Fact]
    public async Task GetCurrentRatesForAnalysisAsync_ReturnsPlnRate()
    {
        var service = new MockExchangeRateService();

        var result = await service.GetCurrentRatesForAnalysisAsync(
            "EUR",
            new[] { "PLN" });

        Assert.Equal(4.32m, result["PLN"]);
    }

    [Fact]
    public async Task GetCurrentRatesForAnalysisAsync_ReturnsDefaultRate()
    {
        var service = new MockExchangeRateService();

        var result = await service.GetCurrentRatesForAnalysisAsync(
            "EUR",
            new[] { "ABC" });

        Assert.Equal(1.05m, result["ABC"]);
    }
}