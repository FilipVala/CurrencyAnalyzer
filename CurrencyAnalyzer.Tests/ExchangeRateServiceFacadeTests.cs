using CurrencyAnalyzer.Core.DTOs;
using CurrencyAnalyzer.Core.Interfaces;
using CurrencyAnalyzer.Core.Services;
using Moq;

namespace CurrencyAnalyzer.Tests;

public class ExchangeRateServiceFacadeTests
{
    [Fact]
    public async Task GetCurrentRatesForAnalysisAsync_ReturnsData()
    {
        var mock = new Mock<IExchangeRateService>();

        mock.Setup(x => x.GetLatestRatesAsync(
                It.IsAny<string>(),
                It.IsAny<IEnumerable<string>>()))
            .ReturnsAsync(new ExchangeRateResponse
            {
                Success = true,
                Rates = new Dictionary<string, decimal>
                {
                    { "USD", 1.1m },
                    { "CZK", 25m }
                }
            });

        var facade = new ExchangeRateServiceFacade(mock.Object);

        var result = await facade.GetCurrentRatesForAnalysisAsync(
            "EUR",
            new[] { "USD", "CZK" });

        Assert.NotNull(result);

        Assert.True(result.ContainsKey("USD"));

        Assert.Equal(1.1m, result["USD"]);

        Assert.Equal(25m, result["CZK"]);
    }

    [Fact]
    public async Task GetHistoricalRatesAsync_ReturnsData()
    {
        var mock = new Mock<IExchangeRateService>();

        mock.Setup(x => x.GetHistoricalRatesAsync(
                It.IsAny<string>(),
                It.IsAny<IEnumerable<string>>(),
                It.IsAny<DateTime>(),
                It.IsAny<DateTime>()))
            .ReturnsAsync(new HistoricalRatesResponse
            {
                Base = "EUR",

                Rates = new Dictionary<string, Dictionary<string, decimal>>
                {
                    {
                        "2025-01-01",
                        new Dictionary<string, decimal>
                        {
                            { "GBP", 0.85m }
                        }
                    }
                }
            });

        var facade = new ExchangeRateServiceFacade(mock.Object);

        var result = await facade.GetHistoricalRatesAsync(
            "EUR",
            new[] { "GBP" },
            DateTime.UtcNow.AddDays(-1),
            DateTime.UtcNow);

        Assert.NotNull(result);

        Assert.True(result.ContainsKey("2025-01-01"));

        Assert.Equal(
            0.85m,
            result["2025-01-01"]["GBP"]);
    }
}