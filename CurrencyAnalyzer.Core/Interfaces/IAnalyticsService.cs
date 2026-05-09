using CurrencyAnalyzer.Core.Models;

namespace CurrencyAnalyzer.Core.Interfaces;

public interface IAnalyticsService
{
    (string Currency, decimal Rate) GetStrongestCurrency(Dictionary<string, decimal> rates);
    (string Currency, decimal Rate) GetWeakestCurrency(Dictionary<string, decimal> rates);

    decimal CalculateAverageRate(Dictionary<string, decimal> rates);

    Task<AnalysisResult> PerformFullAnalysisAsync(string baseCurrency, IEnumerable<string> selectedCurrencies);
}

public class AnalysisResult
{
    public string BaseCurrency { get; set; } = string.Empty;
    public Dictionary<string, decimal> Rates { get; set; } = new();
    public (string Currency, decimal Rate) Strongest { get; set; }
    public (string Currency, decimal Rate) Weakest { get; set; }
    public decimal AverageRate { get; set; }
    public DateTime CalculatedAt { get; set; }
}