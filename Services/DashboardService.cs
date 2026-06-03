using System.Net.Http.Json;
using DriverAI.AdminPanel.Models;

namespace DriverAI.AdminPanel.Services;

public class DashboardService
{
    public async Task<DashboardSummaryDto?> GetSummaryAsync()
    {
        var response = await ApiClient.Http.GetAsync(
            ApiClient.Url("/payments/dashboard-summary")
        );

        response.EnsureSuccessStatusCode();

        return await response.Content
            .ReadFromJsonAsync<DashboardSummaryDto>();
    }
}