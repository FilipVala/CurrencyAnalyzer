using CurrencyAnalyzer.Core.DTOs;
using CurrencyAnalyzer.Core.Interfaces;

namespace CurrencyAnalyzer.Core.Services;

public class ExchangeRateServiceFacade
{
    private readonly IExchangeRateService _exchangeRateService;

    public ExchangeRateServiceFacade(
        IExchangeRateService exchangeRateService)
    {
        _exchangeRateService = exchangeRateService;
    }

    public async Task<Dictionary<string, decimal>>
        GetCurrentRatesForAnalysisAsync(
            string baseCurrency,
            IEnumerable<string> symbols)
    {
        var response = await _exchangeRateService
            .GetLatestRatesAsync(baseCurrency, symbols);

        return response?.Rates
            ?? new Dictionary<string, decimal>();
    }

    public async Task<Dictionary<string, decimal>>
        GetHistoricalRatesAsync(
            string baseCurrency,
            IEnumerable<string> symbols,
            DateTime start,
            DateTime end)
    {
        var response = await _exchangeRateService
            .GetHistoricalRatesAsync(
                baseCurrency,
                symbols,
                start,
                end);

        return response?.Rates
            ?? new Dictionary<string, decimal>();
    }
}