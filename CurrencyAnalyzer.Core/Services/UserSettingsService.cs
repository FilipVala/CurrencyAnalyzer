using System.Text.Json;
using CurrencyAnalyzer.Core.Interfaces;
using CurrencyAnalyzer.Core.Models;

namespace CurrencyAnalyzer.Core.Services;

public class UserSettingsService : IUserSettingsService
{
    private readonly string _filePath = Path.Combine("wwwroot", "data", "usersettings.json");
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public UserSettingsService()
    {
        var directory = Path.GetDirectoryName(_filePath);
        if (directory != null && !Directory.Exists(directory))
            Directory.CreateDirectory(directory);
    }

    public async Task<UserSettings> GetSettingsAsync()
    {
        await _semaphore.WaitAsync();
        try
        {
            if (!File.Exists(_filePath))
                return CreateDefaultSettings();

            var json = await File.ReadAllTextAsync(_filePath);
            var settings = JsonSerializer.Deserialize<UserSettings>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return settings ?? CreateDefaultSettings();
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task SaveSettingsAsync(UserSettings settings)
    {
        await _semaphore.WaitAsync();
        try
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true
            };
            var json = JsonSerializer.Serialize(settings, options);
            await File.WriteAllTextAsync(_filePath, json);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task ResetToDefaultAsync()
    {
        var defaultSettings = CreateDefaultSettings();
        await SaveSettingsAsync(defaultSettings);
    }

    private UserSettings CreateDefaultSettings()
    {
        return new UserSettings
        {
            BaseCurrency = "EUR",
            SelectedCurrencies = new List<string> { "USD", "CZK", "GBP", "JPY" },
            LastUpdated = DateTime.UtcNow
        };
    }
}