using System.Net;
using System.Text;
using CurrencyAnalyzer.Core.Clients;
using CurrencyAnalyzer.Core.Exceptions;
using Xunit;

namespace CurrencyAnalyzer.Tests;

public class ExchangeRateClientTests
{
    [Fact]
    public async Task GetLatestRatesAsync_ThrowsApiException_WhenHttpFails()
    {
        // arrange
        var handler = new FakeHttpMessageHandler(
            HttpStatusCode.BadRequest,
            "{}");

        var httpClient = new HttpClient(handler);

        var client = new ExchangeRateClient(httpClient);

        // act + assert
        await Assert.ThrowsAsync<ApiException>(() =>
            client.GetLatestRatesAsync(
                "EUR",
                new[] { "USD" }));
    }

    [Fact]
    public async Task GetLatestRatesAsync_ThrowsApiException_WhenJsonInvalid()
    {
        // arrange
        var handler = new FakeHttpMessageHandler(
            HttpStatusCode.OK,
            "invalid json");

        var httpClient = new HttpClient(handler);

        var client = new ExchangeRateClient(httpClient);

        // act + assert
        await Assert.ThrowsAsync<ApiException>(() =>
            client.GetLatestRatesAsync(
                "EUR",
                new[] { "USD" }));
    }

    [Fact]
    public async Task GetHistoricalRatesAsync_ReturnsData()
    {
        // arrange
        var json = """
        {
            "success": true,
            "base": "EUR",
            "rates": {
                "USD": 1.1
            }
        }
        """;

        var handler = new FakeHttpMessageHandler(
            HttpStatusCode.OK,
            json);

        var httpClient = new HttpClient(handler);

        var client = new ExchangeRateClient(httpClient);

        // act
        var result = await client.GetHistoricalRatesAsync(
            "EUR",
            new[] { "USD" },
            DateTime.UtcNow);

        // assert
        Assert.True(result.Success);
    }

    [Fact]
    public async Task GetHistoricalRatesAsync_ThrowsApiException_WhenHttpFails()
    {
        // arrange
        var handler = new FakeHttpMessageHandler(
            HttpStatusCode.InternalServerError,
            "{}");

        var httpClient = new HttpClient(handler);

        var client = new ExchangeRateClient(httpClient);

        // act + assert
        await Assert.ThrowsAsync<ApiException>(() =>
            client.GetHistoricalRatesAsync(
                "EUR",
                new[] { "USD" },
                DateTime.UtcNow));
    }

    private class FakeHttpMessageHandler : HttpMessageHandler
    {
        private readonly HttpStatusCode _statusCode;
        private readonly string _response;

        public FakeHttpMessageHandler(
            HttpStatusCode statusCode,
            string response)
        {
            _statusCode = statusCode;
            _response = response;
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(_statusCode)
            {
                Content = new StringContent(
                    _response,
                    Encoding.UTF8,
                    "application/json")
            };

            return Task.FromResult(response);
        }
    }
}