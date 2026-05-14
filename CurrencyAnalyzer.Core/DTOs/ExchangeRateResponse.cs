namespace CurrencyAnalyzer.Core.DTOs;

public class ExchangeRateResponse
{
    public bool Success { get; set; }
    public Dictionary<string, decimal> Rates { get; set; } = new();
    public string Base { get; set; } = string.Empty;
    public DateTime Date { get; set; }
}