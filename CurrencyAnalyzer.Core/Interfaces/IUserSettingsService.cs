using CurrencyAnalyzer.Core.Models;

namespace CurrencyAnalyzer.Core.Interfaces;

public interface IUserSettingsService
{
    Task<UserSettings> GetSettingsAsync();

    Task SaveSettingsAsync(UserSettings settings);

    Task ResetToDefaultAsync();

    // NEW – business logika přesunuta ze UI
    Task<UserSettings> AddCurrencyAsync(string currency);

    Task<UserSettings> RemoveCurrencyAsync(string currency);

    Task<UserSettings> SetBaseCurrencyAsync(string currency);
}