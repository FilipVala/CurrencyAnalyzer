using CurrencyAnalyzer.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

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
        var converter = new ValueConverter<List<string>, string>(
            v => string.Join(',', v),
            v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()
        );

        var comparer = new ValueComparer<List<string>>(
            (c1, c2) => c1!.SequenceEqual(c2!),
            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            c => c.ToList()
        );

        modelBuilder.Entity<UserSettings>()
            .Property(x => x.SelectedCurrencies)
            .HasConversion(converter)
            .Metadata
            .SetValueComparer(comparer);
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