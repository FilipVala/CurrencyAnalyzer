namespace CurrencyAnalyzer.Core.Interfaces;

public interface ILogService
{
    Task LogAsync(string level, string message, string? exception = null);
}