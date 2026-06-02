using System.Net.Http.Headers;

namespace DriverAI.AdminPanel.Services;

public static class ApiClient
{
    public static readonly HttpClient Http = new();

    public static string BaseUrl { get; set; } =
        "https://driverai-api.onrender.com";

    public static string? Token { get; private set; }

    public static void SetToken(string token)
    {
        Token = token;

        Http.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                token
            );
    }

    public static string Url(string endpoint)
    {
        if (!endpoint.StartsWith("/"))
        {
            endpoint = "/" + endpoint;
        }

        return BaseUrl + endpoint;
    }
}