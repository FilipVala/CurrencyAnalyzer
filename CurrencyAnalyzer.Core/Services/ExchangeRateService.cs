using System.Text.Json;
using CurrencyAnalyzer.Core.DTOs;
using CurrencyAnalyzer.Core.Interfaces;

namespace CurrencyAnalyzer.Core.Services;

public class ExchangeRateService : IExchangeRateService
{
    private readonly HttpClient _httpClient;

    public ExchangeRateService(
        HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ExchangeRateResponse> GetLatestRatesAsync(
        string baseCurrency,
        IEnumerable<string> symbols)
    {
        var filteredSymbols = symbols
            .Where(s => s != baseCurrency);

        var symbolsString = string.Join(",", filteredSymbols);

        var url =
            $"https://api.frankfurter.app/latest?from={baseCurrency}&to={symbolsString}";

        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            return new ExchangeRateResponse
            {
                Success = false,
                Rates = new Dictionary<string, decimal>()
            };
        }

        var json = await response.Content.ReadAsStringAsync();

        var result = JsonSerializer.Deserialize<ExchangeRateResponse>(
            json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        if (result == null)
        {
            return new ExchangeRateResponse
            {
                Success = false,
                Rates = new Dictionary<string, decimal>()
            };
        }

        result.Success = true;

        return result;
    }

    public async Task<ExchangeRateResponse> GetHistoricalRatesAsync(
        string baseCurrency,
        IEnumerable<string> symbols,
        DateTime startDate,
        DateTime endDate)
    {
        var filteredSymbols = symbols
            .Where(s => s != baseCurrency);

        var symbolsString = string.Join(",", filteredSymbols);

        var formattedDate = startDate.ToString("yyyy-MM-dd");

        var url =
            $"https://api.frankfurter.app/{formattedDate}?from={baseCurrency}&to={symbolsString}";

        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            return new ExchangeRateResponse
            {
                Success = false,
                Rates = new Dictionary<string, decimal>()
            };
        }

        var json = await response.Content.ReadAsStringAsync();

        var result = JsonSerializer.Deserialize<ExchangeRateResponse>(
            json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        if (result == null)
        {
            return new ExchangeRateResponse
            {
                Success = false,
                Rates = new Dictionary<string, decimal>()
            };
        }

        result.Success = true;

        return result;
    }
}