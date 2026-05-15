using CurrencyAnalyzer.Core.DTOs;

namespace CurrencyAnalyzer.Core.Interfaces;

public interface IExchangeRateService
{
    Task<ExchangeRateResponse> GetLatestRatesAsync(
        string baseCurrency,
        IEnumerable<string> symbols);

    Task<HistoricalRatesResponse?> GetHistoricalRatesAsync(
        string baseCurrency,
        IEnumerable<string> symbols,
        DateTime startDate,
        DateTime endDate);
}