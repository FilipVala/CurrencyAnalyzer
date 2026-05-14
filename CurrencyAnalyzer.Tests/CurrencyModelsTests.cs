using CurrencyAnalyzer.Core.Models;
using Xunit;

namespace CurrencyAnalyzer.Tests;

public class CurrencyModelsTests
{
    [Fact]
    public void CurrencyRate_Properties_Work()
    {
        var model = new CurrencyRate
        {
            Currency = "USD",
            Rate = 1.1m,
            Date = DateTime.UtcNow
        };

        Assert.Equal("USD", model.Currency);

        Assert.Equal(1.1m, model.Rate);
    }

    [Fact]
    public void CurrencyResult_Properties_Work()
    {
        var model = new CurrencyResult
        {
            Currency = "CZK",
            Rate = 25.4m
        };

        Assert.Equal("CZK", model.Currency);

        Assert.Equal(25.4m, model.Rate);
    }

    [Fact]
    public void LoginModel_Properties_Work()
    {
        var model = new LoginModel
        {
            Username = "admin",
            Password = "password"
        };

        Assert.Equal("admin", model.Username);

        Assert.Equal("password", model.Password);
    }

    [Fact]
    public void AuthState_Properties_Work()
    {
        var model = new AuthState
        {
            IsAuthenticated = true,
            Username = "admin"
        };

        Assert.True(model.IsAuthenticated);

        Assert.Equal("admin", model.Username);
    }
}