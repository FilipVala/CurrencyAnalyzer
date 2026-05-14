using CurrencyAnalyzer.Core.Interfaces;
using CurrencyAnalyzer.Core.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CurrencyAnalyzer.Tests;

public class AnalyticsEdgeCaseTests
{
    private readonly AnalyticsService _service;

    public AnalyticsEdgeCaseTests()
    {
        var rateMock = new Mock<IExchangeRateService>();
        var loggerMock = new Mock<ILogger<AnalyticsService>>();

        _service = new AnalyticsService(rateMock.Object, loggerMock.Object);
    }

    [Fact]
    public void GetStrongestCurrency_WithNull_ThrowsOrHandles()
    {
        var result = _service.GetStrongestCurrency(null!);

        Assert.Equal("N/A", result.Currency);
    }

    [Fact]
    public void CalculateAverageRate_WithOneValue_ReturnsSameValue()
    {
        var rates = new Dictionary<string, decimal>
        {
            { "USD", 2.5m }
        };

        var result = _service.CalculateAverageRate(rates);

        Assert.Equal(2.5m, result);
    }
}