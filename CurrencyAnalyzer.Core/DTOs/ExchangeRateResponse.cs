namespace CurrencyAnalyzer.Core.DTOs;

public class ExchangeRateResponse
{
    public bool Success { get; set; }

    public decimal Amount { get; set; }

    public string Base { get; set; } = string.Empty;

    public string Date { get; set; } = string.Empty;

    public Dictionary<string, decimal> Rates { get; set; }
        = new();
}