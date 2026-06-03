using System.Net.Http.Json;
using DriverAI.AdminPanel.Models;

namespace DriverAI.AdminPanel.Services;

/// <summary>
/// Servicio para gestionar pagos desde el panel administrativo.
/// </summary>
public class PaymentsService
{
    /// <summary>
    /// Obtiene todos los pagos registrados.
    /// </summary>
    public async Task<List<PaymentDto>> GetPaymentsAsync()
    {
        var response = await ApiClient.Http.GetAsync(
            ApiClient.Url("/payments")
        );

        response.EnsureSuccessStatusCode();

        var payments = await response.Content
            .ReadFromJsonAsync<List<PaymentDto>>();

        return payments ?? [];
    }

    /// <summary>
    /// Obtiene todos los pagos de un usuario específico.
    /// </summary>
    public async Task<List<PaymentDto>> GetPaymentsByUserAsync(int userId)
    {
        var response = await ApiClient.Http.GetAsync(
            ApiClient.Url($"/payments/user/{userId}")
        );

        response.EnsureSuccessStatusCode();

        var payments = await response.Content
            .ReadFromJsonAsync<List<PaymentDto>>();

        return payments ?? [];
    }

    /// <summary>
    /// Aprueba un pago pendiente.
    /// </summary>
    public async Task ApprovePaymentAsync(int paymentId)
    {
        var response = await ApiClient.Http.PostAsync(
            ApiClient.Url($"/payments/{paymentId}/approve"),
            null
        );

        response.EnsureSuccessStatusCode();
    }

    /// <summary>
    /// Rechaza un pago pendiente.
    /// </summary>
    public async Task RejectPaymentAsync(int paymentId)
    {
        var response = await ApiClient.Http.PostAsync(
            ApiClient.Url($"/payments/{paymentId}/reject"),
            null
        );

        response.EnsureSuccessStatusCode();
    }
}