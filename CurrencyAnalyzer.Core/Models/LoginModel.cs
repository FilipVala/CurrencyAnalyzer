namespace CurrencyAnalyzer.Core.Models;

public class LoginModel
{
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
}

public class AuthState
{
    public bool IsAuthenticated { get; set; } = false;
    public string Username { get; set; } = "";
}
