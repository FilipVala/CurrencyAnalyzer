using System.Text.Json;
using CurrencyAnalyzer.Core.DTOs;
using CurrencyAnalyzer.Core.Interfaces;
using Microsoft.Extensions.Configuration;

namespace CurrencyAnalyzer.Core.Services;

public class ExchangeRateService : IExchangeRateService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public ExchangeRateService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<ExchangeRateResponse> GetLatestRatesAsync(
        string baseCurrency,
        IEnumerable<string> symbols)
    {
        var symbolsString = string.Join(",", symbols);

        // exchangerate.host funguje bez access_key
        var url = $"https://api.exchangerate.host/v1/latest?base={baseCurrency}&symbols={symbolsString}";

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        var result = JsonSerializer.Deserialize<ExchangeRateResponse>(
            json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        if (result == null)
            throw new Exception("Failed to deserialize API response");

        return result;
    }

    public async Task<ExchangeRateResponse> GetHistoricalRatesAsync(
        string baseCurrency,
        IEnumerable<string> symbols,
        DateTime startDate,
        DateTime endDate)
    {
        return await GetLatestRatesAsync(baseCurrency, symbols);
    }

    public async Task<Dictionary<string, decimal>> GetCurrentRatesForAnalysisAsync(
        string baseCurrency,
        IEnumerable<string> selectedCurrencies)
    {
        var result = await GetLatestRatesAsync(baseCurrency, selectedCurrencies);
        return result.Rates;
    }
}