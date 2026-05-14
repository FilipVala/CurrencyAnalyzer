using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CurrencyAnalyzer.Core.Services;
using Microsoft.Extensions.Configuration;
using Xunit;

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
    public async Task GetLatestRatesAsync_ReturnsData_WhenApiReturnsValidResponse()
    {
        // arrange
        var json = """
        {
            "success": true,
            "base": "EUR",
            "rates": {
                "USD": 1.1,
                "CZK": 25.5
            }
        }
        """;

        var service = CreateService(json);

        // act
        var result = await service.GetLatestRatesAsync("EUR", new[] { "USD", "CZK" });

        // assert
        Assert.True(result.Success);
        Assert.Equal("EUR", result.Base);
        Assert.Equal(1.1m, result.Rates["USD"]);
        Assert.Equal(25.5m, result.Rates["CZK"]);
    }

    [Fact]
    public async Task GetLatestRatesAsync_ReturnsNullOrEmpty_WhenApiFails()
    {
        // arrange
        var json = """
        {
            "success": false,
            "error": {
                "code": 101,
                "type": "missing_access_key"
            }
        }
        """;

        var service = CreateService(json);

        // act
        var result = await service.GetLatestRatesAsync("EUR", new[] { "USD" });

        // assert
        Assert.False(result.Success);
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