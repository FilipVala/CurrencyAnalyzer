namespace CurrencyAnalyzer.Core.Models;

public class AnalysisResult
{
    public string BaseCurrency { get; set; } = string.Empty;

    public Dictionary<string, decimal> Rates { get; set; } = new();

    public (string Currency, decimal Rate) Strongest { get; set; }

    public (string Currency, decimal Rate) Weakest { get; set; }

    public decimal AverageRate { get; set; }

    public DateTime CalculatedAt { get; set; }
}