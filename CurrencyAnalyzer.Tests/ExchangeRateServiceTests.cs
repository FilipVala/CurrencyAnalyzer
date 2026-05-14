using System.Net;
using System.Text;
<<<<<<< HEAD
using System.Threading;
using System.Threading.Tasks;
using CurrencyAnalyzer.Core.Services;
using Microsoft.Extensions.Configuration;
using Xunit;
=======
using CurrencyAnalyzer.Core.Clients;
using CurrencyAnalyzer.Core.DTOs;
using CurrencyAnalyzer.Core.Services;
>>>>>>> a11e40f (Add unit tests and improve code coverage)

namespace CurrencyAnalyzer.Tests;

public class ExchangeRateServiceTests
{
    private ExchangeRateService CreateService(string responseJson)
    {
        var handler = new FakeHttpMessageHandler(responseJson);

        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new System.Uri("https://api.exchangerate.host/")
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection()
            .Build();

        return new ExchangeRateService(httpClient, configuration);
    }

    [Fact]
    public async Task GetCurrentRatesForAnalysisAsync_ReturnsRates()
    {
        // arrange
        var json = """
        {
            "success": true,
            "base": "EUR",
            "rates": {
                "USD": 1.1,
                "CZK": 25.0
            }
        }
        """;

        var service = CreateService(json);

        // act
<<<<<<< HEAD
        var result = await service.GetLatestRatesAsync("EUR", new[] { "USD", "CZK" });

        // assert
        Assert.True(result.Success);
        Assert.Equal("EUR", result.Base);
        Assert.Equal(1.1m, result.Rates["USD"]);
        Assert.Equal(25.5m, result.Rates["CZK"]);
    }

    [Fact]
    public async Task GetLatestRatesAsync_ReturnsNullOrEmpty_WhenApiFails()
=======
        var result = await service.GetCurrentRatesForAnalysisAsync(
            "EUR",
            new[] { "USD", "CZK" });

        // assert
        Assert.NotNull(result);

        Assert.Equal(2, result.Count);

        Assert.Equal(1.1m, result["USD"]);

        Assert.Equal(25.0m, result["CZK"]);
    }

    [Fact]
    public async Task GetHistoricalRatesAsync_ReturnsData()
>>>>>>> a11e40f (Add unit tests and improve code coverage)
    {
        var json = """
<<<<<<< HEAD
        {
            "success": false,
            "error": {
                "code": 101,
                "type": "missing_access_key"
            }
=======
    {
        "amount": 1.0,
        "base": "EUR",
        "date": "2026-05-10",
        "rates": {
            "USD": 1.1
>>>>>>> a11e40f (Add unit tests and improve code coverage)
        }
    }
    """;

        var service = CreateService(json);

<<<<<<< HEAD
        // act
        var result = await service.GetLatestRatesAsync("EUR", new[] { "USD" });
=======
        var result = await service.GetHistoricalRatesAsync(
            "EUR",
            new[] { "USD" },
            DateTime.UtcNow.AddDays(-1),
            DateTime.UtcNow);
>>>>>>> a11e40f (Add unit tests and improve code coverage)

        Assert.NotNull(result);

        Assert.Single(result.Rates);

        Assert.Equal(1.1m, result.Rates["USD"]);
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
                Content = new StringContent(_response, Encoding.UTF8, "application/json")
            };

            return Task.FromResult(response);
        }
    }
}