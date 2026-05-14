using CurrencyAnalyzer.Core.Models;

namespace CurrencyAnalyzer.Core.Interfaces;

public interface IAnalyticsService
{
    Task<AnalysisResult> PerformFullAnalysisAsync(string baseCurrency, IEnumerable<string> symbols);

    CurrencyRate GetStrongestCurrency(Dictionary<string, decimal> rates);
    CurrencyRate GetWeakestCurrency(Dictionary<string, decimal> rates);
    decimal CalculateAverageRate(Dictionary<string, decimal> rates);
}