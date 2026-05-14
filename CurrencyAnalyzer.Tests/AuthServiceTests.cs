using CurrencyAnalyzer.Core.Services;
using CurrencyAnalyzer.Core.Interfaces;
using Xunit;

namespace CurrencyAnalyzer.Tests;

public class AuthServiceTests
{
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _authService = new AuthService();
    }

    [Fact]
    public async Task LoginAsync_WithCorrectCredentials_ReturnsTrue()
    {
        var result = await _authService.LoginAsync("admin", "stin2026");

        Assert.True(result);
        Assert.True(_authService.IsLoggedIn());
        Assert.Equal("admin", _authService.GetCurrentUsername());
    }

    [Fact]
    public async Task LoginAsync_WithWrongPassword_ReturnsFalse()
    {
        var result = await _authService.LoginAsync("admin", "wrong");

        Assert.False(result);
        Assert.False(_authService.IsLoggedIn());
    }

    [Fact]
    public async Task LoginAsync_WithWrongUsername_ReturnsFalse()
    {
        var result = await _authService.LoginAsync("user", "stin2026");

        Assert.False(result);
        Assert.False(_authService.IsLoggedIn());
    }

    [Fact]
    public async Task LoginAsync_WithEmptyValues_ReturnsFalse()
    {
        var result = await _authService.LoginAsync("", "");

        Assert.False(result);
        Assert.False(_authService.IsLoggedIn());
    }

    [Fact]
    public async Task Logout_ClearsUserSession()
    {
        await _authService.LoginAsync("admin", "stin2026");

        Assert.True(_authService.IsLoggedIn());

        _authService.Logout();

        Assert.False(_authService.IsLoggedIn());
        Assert.Equal("", _authService.GetCurrentUsername());
    }
}