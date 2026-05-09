using System.Text.Json;
using CurrencyAnalyzer.Core.DTOs;
using CurrencyAnalyzer.Core.Interfaces;
using CurrencyAnalyzer.Core.Models;

namespace CurrencyAnalyzer.Core.Services;

public class ExchangeRateService : IExchangeRateService
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://api.exchangerate.host";

    public ExchangeRateService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ExchangeRateResponse> GetLatestRatesAsync(string baseCurrency, IEnumerable<string> symbols)
    {
        var symbolsParam = string.Join(",", symbols);
        var url = $"{BaseUrl}/latest?base={baseCurrency}&symbols={symbolsParam}";

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ExchangeRateResponse>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? throw new Exception("Failed to deserialize API response");
    }

    public async Task<ExchangeRateResponse> GetHistoricalRatesAsync(string baseCurrency,
        IEnumerable<string> symbols, DateTime startDate, DateTime endDate)
    {
        var symbolsParam = string.Join(",", symbols);
        var url = $"{BaseUrl}/timeseries?base={baseCurrency}&symbols={symbolsParam}" +
                  $"&start_date={startDate:yyyy-MM-dd}&end_date={endDate:yyyy-MM-dd}";

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ExchangeRateResponse>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? throw new Exception("Failed to deserialize API response");
    }

    public async Task<Dictionary<string, decimal>> GetCurrentRatesForAnalysisAsync(string baseCurrency,
        IEnumerable<string> selectedCurrencies)
    {
        var response = await GetLatestRatesAsync(baseCurrency, selectedCurrencies);
        return response.Rates;
    }
}