using CurrencyAnalyzer.Core.Data;
using CurrencyAnalyzer.Core.Interfaces;

namespace CurrencyAnalyzer.Core.Services;

public class LogService : ILogService
{
    private readonly AppDbContext _db;

    public LogService(AppDbContext db)
    {
        _db = db;
    }

    public async Task LogAsync(string level, string message, string? exception = null)
    {
        _db.Logs.Add(new LogEntry
        {
            Level = level,
            Message = message,
            Exception = exception,
            Timestamp = DateTime.UtcNow
        });

        await _db.SaveChangesAsync();
    }
}