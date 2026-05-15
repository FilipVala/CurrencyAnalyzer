using CurrencyAnalyzer.Core.DTOs;
using CurrencyAnalyzer.Core.Interfaces;

namespace CurrencyAnalyzer.Core.Services;

/// <summary>
/// Mock implementace – nepoužívá internet
/// </summary>
public class MockExchangeRateService : IExchangeRateService
{
    public Task<ExchangeRateResponse> GetLatestRatesAsync(
        string baseCurrency,
        IEnumerable<string> symbols)
    {
        var rates = new Dictionary<string, decimal>();

        // základní měna
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

    public Task<HistoricalRatesResponse?> GetHistoricalRatesAsync(
    string baseCurrency,
    IEnumerable<string> symbols,
    DateTime startDate,
    DateTime endDate)
    {
        var historicalRates =
            new Dictionary<string, Dictionary<string, decimal>>();

        for (var date = startDate.Date;
             date <= endDate.Date;
             date = date.AddDays(1))
        {
            var dailyRates = new Dictionary<string, decimal>();

            foreach (var symbol in symbols)
            {
                if (symbol == baseCurrency)
                    continue;

                dailyRates[symbol] =
                    GetMockRate(baseCurrency, symbol)
                    + Random.Shared.Next(-5, 5) * 0.01m;
            }

            historicalRates[date.ToString("yyyy-MM-dd")]
                = dailyRates;
        }

        return Task.FromResult<HistoricalRatesResponse?>(
            new HistoricalRatesResponse
            {
                Base = baseCurrency,
                Rates = historicalRates
            });
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

    private decimal GetMockRate(
        string baseCurrency,
        string targetCurrency)
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

    private decimal GetMockHistoricalRate(
        string baseCurrency,
        string targetCurrency,
        DateTime date)
    {
        var baseRate = GetMockRate(baseCurrency, targetCurrency);

        var variation =
            (decimal)Math.Sin(date.DayOfYear * 0.3) * 0.05m;

        return Math.Round(baseRate + variation, 4);
    }
}