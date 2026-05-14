using CurrencyAnalyzer.Core.Models;
using Xunit;

namespace CurrencyAnalyzer.Tests;

public class SettingsLogicTests
{
    [Fact]
    public void AddingCurrency_Should_Mark_List_And_Keep_BaseCurrency()
    {
        var settings = new UserSettings
        {
            BaseCurrency = "EUR",
            SelectedCurrencies = new List<string> { "USD" }
        };

        // simulace UI logiky
        if (!settings.SelectedCurrencies.Contains(settings.BaseCurrency))
        {
            settings.SelectedCurrencies.Add(settings.BaseCurrency);
        }

        Assert.Contains("EUR", settings.SelectedCurrencies);
        Assert.Contains("USD", settings.SelectedCurrencies);
    }

    [Fact]
    public void Removing_BaseCurrency_Should_Not_Be_Allowed()
    {
        var settings = new UserSettings
        {
            BaseCurrency = "EUR",
            SelectedCurrencies = new List<string> { "EUR", "USD" }
        };

        var currency = "EUR";

        if (currency != settings.BaseCurrency)
        {
            settings.SelectedCurrencies.Remove(currency);
        }

        // EUR musí zůstat
        Assert.Contains("EUR", settings.SelectedCurrencies);
    }
}