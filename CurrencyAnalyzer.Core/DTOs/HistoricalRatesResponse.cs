namespace CurrencyAnalyzer.Core.DTOs;

public class HistoricalRatesResponse
{
    public string Base { get; set; } = string.Empty;

    public Dictionary<string, Dictionary<string, decimal>> Rates
    { get; set; } = new();
}