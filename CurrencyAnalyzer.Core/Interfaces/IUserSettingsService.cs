using CurrencyAnalyzer.Core.Models;

namespace CurrencyAnalyzer.Core.Interfaces;

public interface IUserSettingsService
{
    Task<UserSettings> GetSettingsAsync();
    Task SaveSettingsAsync(UserSettings settings);
    Task ResetToDefaultAsync();
}