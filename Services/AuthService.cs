using System.Net.Http.Json;
using DriverAI.AdminPanel.Models;

namespace DriverAI.AdminPanel.Services;

public class AuthService
{
    public async Task<LoginResponse?> LoginAsync(
        string phoneNumber,
        string password
    )
    {
        var response = await ApiClient.Http.PostAsJsonAsync(
            ApiClient.Url("/api/auth/login"),
            new
            {
                phoneNumber,
                password
            }
        );

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var login = await response.Content
            .ReadFromJsonAsync<LoginResponse>();

        if (login?.Success == true &&
            login.Data != null &&
            !string.IsNullOrWhiteSpace(login.Data.Token))
        {
            ApiClient.SetToken(login.Data.Token);
        }

        return login;
    }
}