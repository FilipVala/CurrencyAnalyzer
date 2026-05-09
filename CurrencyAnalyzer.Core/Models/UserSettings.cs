namespace CurrencyAnalyzer.Core.Models;

public class UserSettings
{
    public int Id { get; set; } = 1;
    public string BaseCurrency { get; set; } = "EUR";
    public List<string> SelectedCurrencies { get; set; } = new List<string> { "USD", "CZK", "GBP", "JPY" };
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
}

// Přidej tuto třídu do stejného souboru jako UserSettings
public class UserSettingsConfiguration
{
    public const int DefaultId = 1;
}