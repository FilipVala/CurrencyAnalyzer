using CurrencyAnalyzer.Core.Interfaces;
using CurrencyAnalyzer.Core.Models;

namespace CurrencyAnalyzer.Core.Services;

public class AnalyticsService : IAnalyticsService
{
    public (string Currency, decimal Rate) GetStrongestCurrency(Dictionary<string, decimal> rates)
    {
        if (rates == null || rates.Count == 0)
            throw new ArgumentException("Rates cannot be empty");

        var strongest = rates.MaxBy(x => x.Value);
        return (strongest.Key, strongest.Value);
    }

    public (string Currency, decimal Rate) GetWeakestCurrency(Dictionary<string, decimal> rates)
    {
        if (rates == null || rates.Count == 0)
            throw new ArgumentException("Rates cannot be empty");

        var weakest = rates.MinBy(x => x.Value);
        return (weakest.Key, weakest.Value);
    }

    public decimal CalculateAverageRate(Dictionary<string, decimal> rates)
    {
        if (rates == null || rates.Count == 0)
            return 0;

        return Math.Round(rates.Values.Average(), 4);
    }

    public async Task<AnalysisResult> PerformFullAnalysisAsync(string baseCurrency,
        IEnumerable<string> selectedCurrencies)
    {
        // Tady bude později volání IExchangeRateService
        throw new NotImplementedException("Bude implementováno po propojení se service");
    }
}