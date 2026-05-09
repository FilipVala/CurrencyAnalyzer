namespace CurrencyAnalyzer.Core.Models;

public class LoginModel
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class AuthState
{
    public bool IsAuthenticated { get; set; } = false;
    public string Username { get; set; } = string.Empty;
}