namespace DriverAI.AdminPanel.Models;

public class DashboardSummaryDto
{
    public int PendingPayments { get; set; }

    public int ApprovedPayments { get; set; }

    public int RejectedPayments { get; set; }

    public decimal TotalRevenue { get; set; }

    public decimal MonthlyRevenue { get; set; }

    public int ActiveUsers { get; set; }

    public int ExpiredUsers { get; set; }

    public int TotalUsers { get; set; }

    public int ReferralRewards { get; set; }

    public int FreeDaysGranted { get; set; }

    public PaymentDto? LastApprovedPayment { get; set; }

    public PaymentDto? LastPendingPayment { get; set; }
}