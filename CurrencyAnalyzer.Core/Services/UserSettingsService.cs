using CurrencyAnalyzer.Core.Data;
using CurrencyAnalyzer.Core.Interfaces;
using CurrencyAnalyzer.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace CurrencyAnalyzer.Core.Services;

public class UserSettingsService : IUserSettingsService
{
    private readonly AppDbContext _db;
    private readonly ILogService _log;

    public UserSettingsService(AppDbContext db, ILogService log)
    {
        _db = db;
        _log = log;
    }

    public async Task<UserSettings> GetSettingsAsync()
    {
        var settings = await _db.UserSettings.FirstOrDefaultAsync();

        if (settings == null)
        {
            settings = CreateDefaultSettings();
            _db.UserSettings.Add(settings);

            await _log.LogAsync("Debug", "Settings loaded");
            await _db.SaveChangesAsync();
        }

        return settings;
    }

    public async Task SaveSettingsAsync(UserSettings settings)
    {
        // validace
        if (!settings.SelectedCurrencies.Contains(settings.BaseCurrency))
        {
            settings.SelectedCurrencies.Add(settings.BaseCurrency);
        }

        var existing = await _db.UserSettings.FirstOrDefaultAsync();

        if (existing == null)
        {
            settings.Id = 1;
            _db.UserSettings.Add(settings);
        }
        else
        {
            existing.BaseCurrency = settings.BaseCurrency;
            existing.SelectedCurrencies = settings.SelectedCurrencies;
            existing.LastUpdated = DateTime.UtcNow;
        }

        await _log.LogAsync("Info", "Settings saved");
        await _db.SaveChangesAsync();
    }

    public async Task ResetToDefaultAsync()
    {
        var existing = await _db.UserSettings.FirstOrDefaultAsync();

        if (existing != null)
        {
            _db.UserSettings.Remove(existing);
        }

        var defaults = CreateDefaultSettings();
        _db.UserSettings.Add(defaults);

        await _log.LogAsync("Warning", "Settings reset to default");
        await _db.SaveChangesAsync();
    }

    private static UserSettings CreateDefaultSettings()
    {
        return new UserSettings
        {
            Id = 1,
            BaseCurrency = "EUR",
            SelectedCurrencies = new List<string>
            {
                "EUR","USD","CZK","GBP","JPY"
            },
            LastUpdated = DateTime.UtcNow
        };
    }

    public async Task<UserSettings> AddCurrencyAsync(string currency)
    {
        var settings = await GetSettingsAsync();

        if (!settings.SelectedCurrencies.Contains(currency))
        {
            settings.SelectedCurrencies.Add(currency);
        }

        await SaveSettingsAsync(settings);
        return settings;
    }

    public async Task<UserSettings> RemoveCurrencyAsync(string currency)
    {
        var settings = await GetSettingsAsync();

        if (settings.SelectedCurrencies.Contains(currency))
        {
            settings.SelectedCurrencies.Remove(currency);
        }

        await SaveSettingsAsync(settings);
        return settings;
    }

    public async Task<UserSettings> SetBaseCurrencyAsync(string currency)
    {
        var settings = await GetSettingsAsync();

        settings.BaseCurrency = currency;

        if (!settings.SelectedCurrencies.Contains(currency))
        {
            settings.SelectedCurrencies.Add(currency);
        }

        await SaveSettingsAsync(settings);
        return settings;
    }
}