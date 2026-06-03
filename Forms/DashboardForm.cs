using DriverAI.AdminPanel.Services;

namespace DriverAI.AdminPanel.Forms;

public class DashboardForm : Form
{
    private readonly DashboardService _service = new();

    private readonly Label _statusLabel = new();

    public DashboardForm()
    {
        Text = "Dashboard Financiero";
        Width = 900;
        Height = 600;
        StartPosition = FormStartPosition.CenterParent;

        Load += async (_, _) => await LoadDashboardAsync();
    }

    private async Task LoadDashboardAsync()
    {
        Controls.Clear();

        _statusLabel.Text = "Cargando dashboard...";
        _statusLabel.Left = 30;
        _statusLabel.Top = 30;
        _statusLabel.Width = 500;

        Controls.Add(_statusLabel);

        try
        {
            var summary = await _service.GetSummaryAsync();

            if (summary == null)
            {
                _statusLabel.Text = "No se pudo cargar información.";
                return;
            }

            Controls.Clear();

            AddTitle();
            AddCard("Pagos pendientes", summary.PendingPayments.ToString(), 30, 90);
            AddCard("Pagos aprobados", summary.ApprovedPayments.ToString(), 300, 90);
            AddCard("Pagos rechazados", summary.RejectedPayments.ToString(), 570, 90);

            AddCard("Ingresos totales", $"₡{summary.TotalRevenue:N0}", 30, 220);
            AddCard("Ingresos del mes", $"₡{summary.MonthlyRevenue:N0}", 300, 220);
            AddCard("Usuarios activos", summary.ActiveUsers.ToString(), 570, 220);

            AddCard("Usuarios vencidos", summary.ExpiredUsers.ToString(), 30, 350);
            AddCard("Usuarios totales", summary.TotalUsers.ToString(), 300, 350);
            AddCard("Premios referidos", summary.ReferralRewards.ToString(), 570, 350);

            var refreshButton = new Button
            {
                Text = "Actualizar",
                Left = 30,
                Top = 500,
                Width = 140,
                Height = 40
            };

            refreshButton.Click += async (_, _) => await LoadDashboardAsync();

            Controls.Add(refreshButton);
        }
        catch (Exception ex)
        {
            _statusLabel.Text = $"Error cargando dashboard: {ex.Message}";
        }
    }

    private void AddTitle()
    {
        var title = new Label
        {
            Text = "Dashboard Financiero",
            Font = new Font("Segoe UI", 24, FontStyle.Bold),
            AutoSize = true,
            Left = 30,
            Top = 25
        };

        Controls.Add(title);
    }

    private void AddCard(string title, string value, int left, int top)
    {
        var panel = new Panel
        {
            Left = left,
            Top = top,
            Width = 240,
            Height = 100,
            BorderStyle = BorderStyle.FixedSingle,
            BackColor = Color.White
        };

        var titleLabel = new Label
        {
            Text = title,
            Left = 12,
            Top = 12,
            Width = 210,
            Font = new Font("Segoe UI", 10, FontStyle.Regular),
            ForeColor = Color.DimGray
        };

        var valueLabel = new Label
        {
            Text = value,
            Left = 12,
            Top = 38,
            Width = 210,
            Font = new Font("Segoe UI", 22, FontStyle.Bold),
            ForeColor = Color.Black
        };

        panel.Controls.Add(titleLabel);
        panel.Controls.Add(valueLabel);

        Controls.Add(panel);
    }
}