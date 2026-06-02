namespace DriverAI.AdminPanel.Models;

public class LoginResponse
{
    public bool Success { get; set; }

    public string Message { get; set; } = "";

    public LoginData? Data { get; set; }
}

public class LoginData
{
    public string Token { get; set; } = "";

    public LoginUser? User { get; set; }
}

public class LoginUser
{
    public int Id { get; set; }

    public string PhoneNumber { get; set; } = "";

    public string Email { get; set; } = "";

    public string Username { get; set; } = "";

    public string Role { get; set; } = "";
}