using System.Net.Http.Json;
using DriverAI.AdminPanel.Models;

namespace DriverAI.AdminPanel.Services;

public class UsersService
{
    public async Task<List<UserDto>> GetUsersAsync()
    {
        var response = await ApiClient.Http.GetAsync(
            ApiClient.Url("/users")
        );

        response.EnsureSuccessStatusCode();

        var users =
            await response.Content.ReadFromJsonAsync<List<UserDto>>();

        return users ?? [];
    }
}