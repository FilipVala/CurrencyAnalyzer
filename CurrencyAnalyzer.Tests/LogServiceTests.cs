using CurrencyAnalyzer.Core.Data;
using CurrencyAnalyzer.Core.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CurrencyAnalyzer.Tests;

public class LogServiceTests
{
    [Fact]
    public async Task LogAsync_AddsLogEntry()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var db = new AppDbContext(options);

        var service = new LogService(db);

        await service.LogAsync(
            "INFO",
            "Test message");

        Assert.Single(db.Logs);

        var log = db.Logs.First();

        Assert.Equal("INFO", log.Level);

        Assert.Equal("Test message", log.Message);
    }
}