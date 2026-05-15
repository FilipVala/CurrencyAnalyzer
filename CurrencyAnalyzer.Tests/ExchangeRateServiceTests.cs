using System.Net;
using System.Text;
using System.Threading;
using CurrencyAnalyzer.Core.DTOs;
using CurrencyAnalyzer.Core.Services;
using Xunit;

namespace CurrencyAnalyzer.Tests;

public class ExchangeRateServiceTests
{
    private ExchangeRateService CreateService(string responseJson)
    {
        var handler = new FakeHttpMessageHandler(responseJson);

        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://api.frankfurter.app/")
        };

        return new ExchangeRateService(httpClient);
    }

    [Fact]
    public async Task GetLatestRatesAsync_ReturnsRates()
    {
        var json = """
        {
            "base": "EUR",
            "date": "2026-05-10",
            "rates": {
                "USD": 1.1,
                "CZK": 25.0
            }
        }
        """;

        var service = CreateService(json);

        var result = await service.GetLatestRatesAsync(
            "EUR",
            new[] { "USD", "CZK" });

        Assert.True(result.Success);

        Assert.Equal("EUR", result.Base);

        Assert.Equal(1.1m, result.Rates["USD"]);

        Assert.Equal(25.0m, result.Rates["CZK"]);
    }

    [Fact]
    public async Task GetHistoricalRatesAsync_ReturnsHistoricalData()
    {
        var json = """
        {
            "base": "EUR",
            "rates": {
                "2025-01-01": {
                    "USD": 1.1
                },
                "2025-01-02": {
                    "USD": 1.2
                }
            }
        }
        """;

        var service = CreateService(json);

        var result = await service.GetHistoricalRatesAsync(
            "EUR",
            new[] { "USD" },
            DateTime.UtcNow.AddDays(-30),
            DateTime.UtcNow);

        Assert.NotNull(result);

        Assert.Equal("EUR", result!.Base);

        Assert.Equal(2, result.Rates.Count);

        Assert.Equal(
            1.1m,
            result.Rates["2025-01-01"]["USD"]);

        Assert.Equal(
            1.2m,
            result.Rates["2025-01-02"]["USD"]);
    }

    [Fact]
    public async Task GetLatestRatesAsync_WhenApiFails_ReturnsFailedResponse()
    {
        var handler = new FailedHttpMessageHandler();

        var httpClient = new HttpClient(handler);

        var service = new ExchangeRateService(httpClient);

        var result = await service.GetLatestRatesAsync(
            "EUR",
            new[] { "USD" });

        Assert.False(result.Success);

        Assert.Empty(result.Rates);
    }

    private class FakeHttpMessageHandler : HttpMessageHandler
    {
        private readonly string _response;

        public FakeHttpMessageHandler(string response)
        {
            _response = response;
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(
                    _response,
                    Encoding.UTF8,
                    "application/json")
            };

            return Task.FromResult(response);
        }
    }

    private class FailedHttpMessageHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(
                new HttpResponseMessage(HttpStatusCode.BadRequest));
        }
    }
}