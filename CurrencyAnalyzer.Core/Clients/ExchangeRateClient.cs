using CurrencyAnalyzer.Core.DTOs;
using CurrencyAnalyzer.Core.Exceptions;
using System.Text.Json;

namespace CurrencyAnalyzer.Core.Clients;

public class ExchangeRateClient
{
    private readonly HttpClient _httpClient;

    public ExchangeRateClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ExchangeRateResponse> GetLatestRatesAsync(
        string baseCurrency,
        IEnumerable<string> symbols)
    {
        var symbolsString = string.Join(",", symbols);

        var url =
            $"https://api.frankfurter.app/latest?from={baseCurrency}&to={symbolsString}";

        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            throw new ApiException(
                "Failed to fetch exchange rates.");
        }

        var json = await response.Content.ReadAsStringAsync();

        ExchangeRateResponse? result;

        try
        {
            result = JsonSerializer.Deserialize<ExchangeRateResponse>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
        }
        catch (JsonException)
        {
            throw new ApiException(
                "Invalid JSON response from API.");
        }

        if (result == null)
        {
            throw new ApiException(
                "Failed to deserialize API response.");
        }

        result.Success = true;

        return result;
    }

    public async Task<ExchangeRateResponse> GetHistoricalRatesAsync(
        string baseCurrency,
        IEnumerable<string> symbols,
        DateTime date)
    {
        var symbolsString = string.Join(",", symbols);

        var formattedDate = date.ToString("yyyy-MM-dd");

        var url =
            $"https://api.frankfurter.app/{formattedDate}?from={baseCurrency}&to={symbolsString}";

        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            throw new ApiException(
                "Failed to fetch historical exchange rates.");
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
            throw new ApiException(
                "Failed to deserialize historical response.");
        }

        result.Success = true;

        return result;
    }
}