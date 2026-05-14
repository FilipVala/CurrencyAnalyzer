namespace CurrencyAnalyzer.Core.Models;

public class UserSettings
{
    public int Id { get; set; } = 1;
    public string BaseCurrency { get; set; } = "EUR";
    public List<string> SelectedCurrencies { get; set; } = new();
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
}