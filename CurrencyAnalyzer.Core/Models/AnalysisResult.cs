namespace CurrencyAnalyzer.Core.Models;

public class AnalysisResult
{
    public string BaseCurrency { get; set; } = string.Empty;

    public Dictionary<string, decimal> Rates { get; set; } = new();

    public CurrencyRate Strongest { get; set; } = new();

    public CurrencyRate Weakest { get; set; } = new();

    public decimal AverageRate { get; set; }

    public DateTime CalculatedAt { get; set; }
}