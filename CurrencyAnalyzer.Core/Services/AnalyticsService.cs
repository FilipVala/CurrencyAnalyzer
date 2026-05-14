using CurrencyAnalyzer.Core.DTOs;
using CurrencyAnalyzer.Core.Interfaces;
using CurrencyAnalyzer.Core.Models;
using Microsoft.Extensions.Logging;

namespace CurrencyAnalyzer.Core.Services;

public class AnalyticsService : IAnalyticsService
{
    private readonly IExchangeRateService _exchangeRateService;
    private readonly ILogger<AnalyticsService> _logger;

    public AnalyticsService(
        IExchangeRateService exchangeRateService,
        ILogger<AnalyticsService> logger)
    {
        _exchangeRateService = exchangeRateService;
        _logger = logger;
    }

    public async Task<AnalysisResult> PerformFullAnalysisAsync(
        string baseCurrency,
        IEnumerable<string> symbols)
    {
        var response = await _exchangeRateService
            .GetLatestRatesAsync(baseCurrency, symbols);

        if (response == null || !response.Success)
        {
            _logger.LogWarning("Exchange rate API returned invalid response");

            return new AnalysisResult
            {
                BaseCurrency = baseCurrency,
                Rates = new Dictionary<string, decimal>(),
                Strongest = new CurrencyRate { Currency = "N/A", Rate = 0 },
                Weakest = new CurrencyRate { Currency = "N/A", Rate = 0 },
                AverageRate = 0,
                CalculatedAt = DateTime.UtcNow
            };
        }

        var rates = response.Rates ?? new Dictionary<string, decimal>();

        var strongest = GetStrongestCurrency(rates);
        var weakest = GetWeakestCurrency(rates);
        var average = CalculateAverageRate(rates);

        return new AnalysisResult
        {
            BaseCurrency = baseCurrency,
            Rates = rates,
            Strongest = strongest,
            Weakest = weakest,
            AverageRate = average,
            CalculatedAt = DateTime.UtcNow
        };
    }

    public CurrencyRate GetStrongestCurrency(Dictionary<string, decimal> rates)
    {
        if (rates == null || rates.Count == 0)
            return new CurrencyRate { Currency = "N/A", Rate = 0 };

        var max = rates.OrderByDescending(x => x.Value).First();

        return new CurrencyRate
        {
            Currency = max.Key,
            Rate = max.Value
        };
    }

    public CurrencyRate GetWeakestCurrency(Dictionary<string, decimal> rates)
    {
        if (rates == null || rates.Count == 0)
            return new CurrencyRate { Currency = "N/A", Rate = 0 };

        var min = rates.OrderBy(x => x.Value).First();

        return new CurrencyRate
        {
            Currency = min.Key,
            Rate = min.Value
        };
    }

    public decimal CalculateAverageRate(Dictionary<string, decimal> rates)
    {
        if (rates == null || rates.Count == 0)
            return 0;

        return rates.Values.Average();
    }

}