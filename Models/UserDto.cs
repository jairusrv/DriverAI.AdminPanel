namespace DriverAI.AdminPanel.Models;

public class UserDto
{
    public int Id { get; set; }

    public string PhoneNumber { get; set; } = "";

    public string Email { get; set; } = "";

    public string Username { get; set; } = "";

    public bool IsSubscriptionActive { get; set; }

    public DateTime? SubscriptionExpiryDate { get; set; }

    public bool HasAccess { get; set; }

    public int RemainingTrialDays { get; set; }
}