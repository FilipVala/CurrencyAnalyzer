namespace CurrencyAnalyzer.Core.Interfaces;

public interface IAuthService
{
    Task<bool> LoginAsync(string username, string password);
    void Logout();
    bool IsLoggedIn();
    string GetCurrentUsername();
}