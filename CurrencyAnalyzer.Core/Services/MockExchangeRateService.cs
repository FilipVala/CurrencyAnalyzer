using CurrencyAnalyzer.Core.DTOs;
using CurrencyAnalyzer.Core.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CurrencyAnalyzer.Core.Services;

/// <summary>
/// Mock implementace – nepoužívá internet
/// </summary>
public class MockExchangeRateService : IExchangeRateService
{
    public Task<ExchangeRateResponse> GetLatestRatesAsync(string baseCurrency, IEnumerable<string> symbols)
    {
        var rates = new Dictionary<string, decimal>();

        // Základní měna má vždy 1.0
        rates[baseCurrency] = 1.0m;

        foreach (var symbol in symbols.Where(s => s != baseCurrency))
        {
            rates[symbol] = GetMockRate(baseCurrency, symbol);
        }

        return Task.FromResult(new ExchangeRateResponse
        {
            Success = true,
            Base = baseCurrency,
            Date = DateTime.UtcNow.ToString("yyyy-MM-dd"),
            Rates = rates
        });
    }

    public Task<ExchangeRateResponse> GetHistoricalRatesAsync(
        string baseCurrency,
        IEnumerable<string> symbols,
        DateTime startDate,
        DateTime endDate)
    {
        return GetLatestRatesAsync(baseCurrency, symbols); // pro mock stačí
    }

    public Task<Dictionary<string, decimal>> GetCurrentRatesForAnalysisAsync(
        string baseCurrency,
        IEnumerable<string> selectedCurrencies)
    {
        var rates = new Dictionary<string, decimal>
        {
            [baseCurrency] = 1.0m
        };

        foreach (var currency in selectedCurrencies.Where(c => c != baseCurrency))
        {
            rates[currency] = GetMockRate(baseCurrency, currency);
        }

        return Task.FromResult(rates);
    }

    private decimal GetMockRate(string baseCurrency, string targetCurrency)
    {
        return targetCurrency switch
        {
            "USD" => 1.10m,
            "GBP" => 0.86m,
            "CZK" => 24.50m,
            "JPY" => 160.0m,
            "CHF" => 0.96m,
            "PLN" => 4.32m,
            _ => 1.05m
        };
    }
}