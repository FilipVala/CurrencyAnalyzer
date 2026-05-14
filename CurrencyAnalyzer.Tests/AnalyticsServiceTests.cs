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
<<<<<<< HEAD
=======

    [Fact]
    public void GetWeakestCurrency_WithNullDictionary_ReturnsNA()
    {
        var result = _analyticsService.GetWeakestCurrency(null!);

        Assert.Equal("N/A", result.Currency);
        Assert.Equal(0, result.Rate);
    }

    [Fact]
    public void CalculateAverageRate_WithNullDictionary_ReturnsZero()
    {
        var result = _analyticsService.CalculateAverageRate(null!);

        Assert.Equal(0, result);
    }
    [Fact]
    public void GetStrongestCurrency_WithSingleCurrency_ReturnsCorrectResult()
    {
        var rates = new Dictionary<string, decimal>
    {
        { "CZK", 25.4m }
    };

        var result = _analyticsService.GetStrongestCurrency(rates);

        Assert.Equal("CZK", result.Currency);
    }

    [Fact]
    public async Task PerformFullAnalysisAsync_ReturnsCorrectAnalysis()
    {
        // Arrange
        var rates = new Dictionary<string, decimal>
    {
        { "USD", 1.1m },
        { "CZK", 24.5m },
        { "GBP", 0.8m }
    };

        var response = new ExchangeRateResponse
        {
            Success = true,
            Rates = rates
        };

        var exchangeRateServiceMock = new Mock<IExchangeRateService>();

        exchangeRateServiceMock
            .Setup(x => x.GetLatestRatesAsync(
                It.IsAny<string>(),
                It.IsAny<IEnumerable<string>>()))
            .ReturnsAsync(response);

        var loggerMock = new Mock<ILogger<AnalyticsService>>();

        var logServiceMock = new Mock<ILogService>();

        var service = new AnalyticsService(
            exchangeRateServiceMock.Object,
            loggerMock.Object,
            logServiceMock.Object);

        // Act
        var result = await service.PerformFullAnalysisAsync(
            "EUR",
            new[] { "USD", "CZK", "GBP" });

        // Assert
        Assert.Equal("EUR", result.BaseCurrency);

        Assert.Equal("CZK", result.Strongest.Currency);

        Assert.Equal("GBP", result.Weakest.Currency);

        Assert.Equal(8.8m, result.AverageRate);
    }

    [Fact]
    public async Task PerformFullAnalysisAsync_WithFailedResponse_ReturnsFallbackResult()
    {
        // Arrange
        var response = new ExchangeRateResponse
        {
            Success = false,
            Rates = new Dictionary<string, decimal>()
        };

        var exchangeRateServiceMock = new Mock<IExchangeRateService>();

        exchangeRateServiceMock
            .Setup(x => x.GetLatestRatesAsync(
                It.IsAny<string>(),
                It.IsAny<IEnumerable<string>>()))
            .ReturnsAsync(response);

        var loggerMock = new Mock<ILogger<AnalyticsService>>();

        var logServiceMock = new Mock<ILogService>();

        var service = new AnalyticsService(
            exchangeRateServiceMock.Object,
            loggerMock.Object,
            logServiceMock.Object);

        // Act
        var result = await service.PerformFullAnalysisAsync(
            "EUR",
            new[] { "USD" });

        // Assert
        Assert.Equal("N/A", result.Strongest.Currency);

        Assert.Equal("N/A", result.Weakest.Currency);

        Assert.Equal(0, result.AverageRate);
    }

    [Fact]
    public async Task PerformFullAnalysisAsync_WithNullResponse_ReturnsFallbackResult()
    {
        // Arrange
        var exchangeRateServiceMock = new Mock<IExchangeRateService>();

        exchangeRateServiceMock
            .Setup(x => x.GetLatestRatesAsync(
                It.IsAny<string>(),
                It.IsAny<IEnumerable<string>>()))
            .ReturnsAsync((ExchangeRateResponse)null!);

        var loggerMock = new Mock<ILogger<AnalyticsService>>();

        var logServiceMock = new Mock<ILogService>();

        var service = new AnalyticsService(
            exchangeRateServiceMock.Object,
            loggerMock.Object,
            logServiceMock.Object);

        // Act
        var result = await service.PerformFullAnalysisAsync(
            "EUR",
            new[] { "USD" });

        // Assert
        Assert.NotNull(result);

        Assert.Empty(result.Rates);

        Assert.Equal("N/A", result.Strongest.Currency);

        Assert.Equal("N/A", result.Weakest.Currency);

        Assert.Equal(0, result.AverageRate);
    }

    [Fact]
    public async Task PerformFullAnalysisAsync_WithNullRates_ReturnsEmptyRates()
    {
        var response = new ExchangeRateResponse
        {
            Success = true,
            Rates = null!
        };

        var exchangeRateServiceMock = new Mock<IExchangeRateService>();

        exchangeRateServiceMock
            .Setup(x => x.GetLatestRatesAsync(
                It.IsAny<string>(),
                It.IsAny<IEnumerable<string>>()))
            .ReturnsAsync(response);

        var loggerMock = new Mock<ILogger<AnalyticsService>>();

        var logServiceMock = new Mock<ILogService>();

        var service = new AnalyticsService(
            exchangeRateServiceMock.Object,
            loggerMock.Object,
            logServiceMock.Object);

        var result = await service.PerformFullAnalysisAsync(
            "EUR",
            new[] { "USD" });

        Assert.Empty(result.Rates);
    }

    [Fact]
    public void GetWeakestCurrency_WithEmptyDictionary_ReturnsNA()
    {
        var result = _analyticsService.GetWeakestCurrency(
            new Dictionary<string, decimal>());

        Assert.Equal("N/A", result.Currency);
    }
>>>>>>> a11e40f (Add unit tests and improve code coverage)
}