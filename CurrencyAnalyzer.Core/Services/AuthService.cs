using CurrencyAnalyzer.Core.Interfaces;

namespace CurrencyAnalyzer.Core.Services;
public class AuthService : IAuthService
{
    private bool _isLoggedIn;
    private string _username = "";

    private const string CorrectUsername = "admin";
    private const string CorrectPassword = "stin2026";

    public Task<bool> LoginAsync(string username, string password)
    {
        Console.WriteLine("AUTH SERVICE CALLED");
        if (username?.Trim().ToLower() == CorrectUsername &&
          password == CorrectPassword)
        {
            _isLoggedIn = true;
            _username = username.Trim();
            return Task.FromResult(true);
        }

        _isLoggedIn = false;
        _username = "";
        return Task.FromResult(false);
    }

    public void Logout()
    {
        _isLoggedIn = false;
        _username = "";
    }

    public bool IsLoggedIn() => _isLoggedIn;

    public string GetCurrentUsername() => _username;
}
