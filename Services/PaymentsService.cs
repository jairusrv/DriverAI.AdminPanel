using System.Net.Http.Json;
using DriverAI.AdminPanel.Models;

namespace DriverAI.AdminPanel.Services;

public class PaymentsService
{
    public async Task<List<PaymentDto>> GetPaymentsAsync()
    {
        var response = await ApiClient.Http.GetAsync(
            ApiClient.Url("/payments")
        );

        response.EnsureSuccessStatusCode();

        var payments = await response.Content
            .ReadFromJsonAsync<List<PaymentDto>>();

        return payments ?? new List<PaymentDto>();
    }

    public async Task ApprovePaymentAsync(int paymentId)
    {
        var response = await ApiClient.Http.PostAsync(
            ApiClient.Url($"/payments/{paymentId}/approve"),
            null
        );

        response.EnsureSuccessStatusCode();
    }

    public async Task RejectPaymentAsync(int paymentId)
    {
        var response = await ApiClient.Http.PostAsync(
            ApiClient.Url($"/payments/{paymentId}/reject"),
            null
        );

        response.EnsureSuccessStatusCode();
    }
}