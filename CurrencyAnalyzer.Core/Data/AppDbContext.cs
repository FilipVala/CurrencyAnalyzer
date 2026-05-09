using CurrencyAnalyzer.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace CurrencyAnalyzer.Core.Data;

public class AppDbContext : DbContext
{
    public DbSet<UserSettings> UserSettings { get; set; }
    public DbSet<LogEntry> Logs { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserSettings>()
            .HasData(new UserSettings
            {
                Id = 1,
                BaseCurrency = "EUR",
                SelectedCurrencies = new List<string> { "USD", "CZK", "GBP", "JPY" }
            });
    }
}

public class LogEntry
{
    public int Id { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string Level { get; set; } = "Information";
    public string Message { get; set; } = string.Empty;
    public string? Exception { get; set; }
}