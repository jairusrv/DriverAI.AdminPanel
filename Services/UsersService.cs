using System.Net.Http.Json;
using DriverAI.AdminPanel.Models;

namespace DriverAI.AdminPanel.Services;
/// <summary>
/// Servicio para gestionar las operaciones relacionadas con los usuarios en el panel de administración de DriverAI.
/// </summary>
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

    public async Task ExtendSubscriptionAsync(
    int userId,
    int days,
    string paymentMethod,
    string notes
)
{
    var response = await ApiClient.Http.PutAsJsonAsync(
        ApiClient.Url($"/users/{userId}/extend"),
        new
        {
            days,
            paymentMethod,
            notes
        }
    );

    response.EnsureSuccessStatusCode();
}
}