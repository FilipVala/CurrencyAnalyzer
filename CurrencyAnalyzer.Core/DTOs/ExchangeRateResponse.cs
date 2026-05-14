namespace CurrencyAnalyzer.Core.DTOs;

public class ExchangeRateResponse
{
    public bool Success { get; set; }
<<<<<<< HEAD
    public Dictionary<string, decimal> Rates { get; set; } = new();
    public string Base { get; set; } = string.Empty;
    public DateTime Date { get; set; }
=======

    public decimal Amount { get; set; }

    public string Base { get; set; } = string.Empty;

    public string Date { get; set; } = string.Empty;

    public Dictionary<string, decimal> Rates { get; set; }
        = new();
>>>>>>> a11e40f (Add unit tests and improve code coverage)
}