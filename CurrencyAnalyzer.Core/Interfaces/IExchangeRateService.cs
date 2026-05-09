using CurrencyAnalyzer.Core.DTOs;
using CurrencyAnalyzer.Core.Models;

namespace CurrencyAnalyzer.Core.Interfaces;

public interface IExchangeRateService
{
    Task<ExchangeRateResponse> GetLatestRatesAsync(string baseCurrency, IEnumerable<string> symbols);

    Task<ExchangeRateResponse> GetHistoricalRatesAsync(string baseCurrency,
        IEnumerable<string> symbols, DateTime startDate, DateTime endDate);

    Task<Dictionary<string, decimal>> GetCurrentRatesForAnalysisAsync(string baseCurrency,
        IEnumerable<string> selectedCurrencies);
}