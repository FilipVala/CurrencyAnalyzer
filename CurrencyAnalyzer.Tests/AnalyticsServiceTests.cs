using CurrencyAnalyzer.Core.Interfaces;
using CurrencyAnalyzer.Core.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace CurrencyAnalyzer.Tests;

public class AnalyticsServiceTests
{
    private readonly AnalyticsService _analyticsService;

    public AnalyticsServiceTests()
    {
        var exchangeRateServiceMock = new Mock<IExchangeRateService>();

        var loggerMock = new Mock<ILogger<AnalyticsService>>();

        _analyticsService = new AnalyticsService(
            exchangeRateServiceMock.Object,
            loggerMock.Object);
    }

    [Fact]
    public void GetStrongestCurrency_ReturnsHighestRate()
    {
        var rates = new Dictionary<string, decimal>
        {
            { "USD", 1.1m },
            { "CZK", 24.5m },
            { "GBP", 0.8m }
        };

        var result = _analyticsService.GetStrongestCurrency(rates);

        Assert.Equal("CZK", result.Currency);
        Assert.Equal(24.5m, result.Rate);
    }

    [Fact]
    public void GetWeakestCurrency_ReturnsLowestRate()
    {
        var rates = new Dictionary<string, decimal>
        {
            { "USD", 1.1m },
            { "CZK", 24.5m },
            { "GBP", 0.8m }
        };

        var result = _analyticsService.GetWeakestCurrency(rates);

        Assert.Equal("GBP", result.Currency);
        Assert.Equal(0.8m, result.Rate);
    }

    [Fact]
    public void CalculateAverageRate_ReturnsCorrectAverage()
    {
        var rates = new Dictionary<string, decimal>
        {
            { "USD", 1.0m },
            { "CZK", 3.0m },
            { "GBP", 2.0m }
        };

        var result = _analyticsService.CalculateAverageRate(rates);

        Assert.Equal(2.0m, result);
    }

    [Fact]
    public void CalculateAverageRate_WithEmptyDictionary_ReturnsZero()
    {
        var rates = new Dictionary<string, decimal>();

        var result = _analyticsService.CalculateAverageRate(rates);

        Assert.Equal(0, result);
    }

    [Fact]
    public void GetStrongestCurrency_WithEmptyDictionary_ReturnsNA()
    {
        var rates = new Dictionary<string, decimal>();

        var result = _analyticsService.GetStrongestCurrency(rates);

        Assert.Equal("N/A", result.Currency);
        Assert.Equal(0, result.Rate);
    }
}