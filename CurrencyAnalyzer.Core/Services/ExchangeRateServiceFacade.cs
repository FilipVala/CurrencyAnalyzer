using CurrencyAnalyzer.Core.Interfaces;

namespace CurrencyAnalyzer.Core.Services;

public class ExchangeRateServiceFacade
{
    private readonly IExchangeRateService _service;

    public ExchangeRateServiceFacade(IExchangeRateService service)
    {
        _service = service;
    }

    public async Task<Dictionary<string, decimal>> GetCurrentRatesForAnalysisAsync(
        string baseCurrency,
        IEnumerable<string> symbols)
    {
        var result = await _service.GetLatestRatesAsync(baseCurrency, symbols);
        return result.Rates;
    }

    public async Task<Dictionary<string, decimal>> GetHistoricalRatesAsync(
        string baseCurrency,
        IEnumerable<string> symbols,
        DateTime start,
        DateTime end)
    {
        // fallback (STIN styl – často se historie neřeší)
        var result = await _service.GetLatestRatesAsync(baseCurrency, symbols);
        return result.Rates;
    }
}