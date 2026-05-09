using CurrencyAnalyzer.Core.Interfaces;

namespace CurrencyAnalyzer.Core.Services;

public class AuthService : IAuthService
{
    private bool _isLoggedIn = false;
    private string _username = "";

    private const string CorrectUsername = "admin";
    private const string CorrectPassword = "stin2026";

    public async Task<bool> LoginAsync(string username, string password)
    {
        await Task.Delay(300); // simulace

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            return false;

        if (username.Trim().ToLower() == CorrectUsername && password == CorrectPassword)
        {
            _isLoggedIn = true;
            _username = username.Trim();
            return true;
        }

        return false;
    }

    public void Logout()
    {
        _isLoggedIn = false;
        _username = "";
    }

    public bool IsLoggedIn() => _isLoggedIn;
    public string GetCurrentUsername() => _username;
}