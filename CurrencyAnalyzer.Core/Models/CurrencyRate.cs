namespace CurrencyAnalyzer.Core.Models;

public class CurrencyRate
{
    public string Currency { get; set; } = string.Empty;
    public decimal Rate { get; set; }
    public DateTime Date { get; set; }
    public string BaseCurrency { get; set; } = string.Empty;
}