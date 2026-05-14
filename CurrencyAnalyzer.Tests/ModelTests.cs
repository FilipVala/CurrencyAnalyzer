using CurrencyAnalyzer.Core.Models;
using Xunit;

namespace CurrencyAnalyzer.Tests;

public class ModelsTests
{
    [Fact]
    public void CurrencyRate_Properties_Work()
    {
        var model = new CurrencyRate
        {
            Currency = "USD",
            Rate = 1.1m,
            Date = DateTime.Today
        };

        Assert.Equal("USD", model.Currency);

        Assert.Equal(1.1m, model.Rate);

        Assert.Equal(DateTime.Today, model.Date);
    }

    [Fact]
    public void CurrencyResult_Properties_Work()
    {
        var model = new CurrencyResult
        {
            Currency = "CZK",
            Rate = 25.5m
        };

        Assert.Equal("CZK", model.Currency);

        Assert.Equal(25.5m, model.Rate);
    }

    [Fact]
    public void LoginModel_Properties_Work()
    {
        var model = new LoginModel
        {
            Username = "admin",
            Password = "1234"
        };

        Assert.Equal("admin", model.Username);

        Assert.Equal("1234", model.Password);
    }

    [Fact]
    public void AuthState_Properties_Work()
    {
        var model = new AuthState
        {
            IsAuthenticated = true,
            Username = "filip"
        };

        Assert.True(model.IsAuthenticated);

        Assert.Equal("filip", model.Username);
    }
}