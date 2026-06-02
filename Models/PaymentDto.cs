namespace DriverAI.AdminPanel.Models;

public class PaymentDto
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public decimal Amount { get; set; }

    public string Currency { get; set; } = "CRC";

    public string Provider { get; set; } = "";

    public string ProviderReference { get; set; } = "";

    public string Status { get; set; } = "";

    public string PaymentType { get; set; } = "";

    public DateTime? PaidFrom { get; set; }

    public DateTime? PaidUntil { get; set; }

    public DateTime? ApprovedAt { get; set; }

    public string? ApprovedBy { get; set; }

    public string? Notes { get; set; }

    public string? SinpeSenderPhone { get; set; }

    public string? SinpeReferenceNumber { get; set; }

    public DateTime CreatedAt { get; set; }
}