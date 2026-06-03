using DriverAI.AdminPanel.Models;
using DriverAI.AdminPanel.Services;

namespace DriverAI.AdminPanel.Forms;

public class UserDetailsForm : Form
{
    private readonly UserDto _user;
    private readonly PaymentsService _paymentsService = new();
    private readonly UsersService _usersService = new();

    private readonly DataGridView _paymentsGrid = new();
    private readonly Label _statusLabel = new();
    private readonly Button _extendButton = new();

    public UserDetailsForm(UserDto user)
    {
        _user = user;

        Text = $"Usuario - {_user.Username}";
        Width = 1000;
        Height = 700;
        StartPosition = FormStartPosition.CenterParent;
        BackColor = Color.FromArgb(248, 250, 252);

        BuildUi();

        Load += async (_, _) => await LoadPaymentsAsync();
    }

    private void BuildUi()
    {
        var title = new Label
        {
            Text = _user.Username,
            Left = 24,
            Top = 20,
            Width = 500,
            Height = 40,
            Font = new Font("Segoe UI", 24, FontStyle.Bold),
            ForeColor = Color.FromArgb(15, 23, 42)
        };

        var info = new Label
        {
            Text =
                $"Teléfono: {_user.PhoneNumber}\n" +
                $"Email: {_user.Email}\n" +
                $"Acceso activo: {(_user.HasAccess ? "Sí" : "No")}\n" +
                $"Vence: {FormatDate(_user.SubscriptionExpiryDate)}",
            Left = 28,
            Top = 75,
            Width = 500,
            Height = 95,
            Font = new Font("Segoe UI", 10),
            ForeColor = Color.FromArgb(71, 85, 105)
        };

        var statusPanel = new Panel
        {
            Left = 580,
            Top = 30,
            Width = 360,
            Height = 110,
            BackColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle
        };

        var statusTitle = new Label
        {
            Text = _user.HasAccess ? "ACTIVO" : "VENCIDO",
            Left = 18,
            Top = 18,
            Width = 300,
            Height = 32,
            Font = new Font("Segoe UI", 22, FontStyle.Bold),
            ForeColor = _user.HasAccess
                ? Color.FromArgb(22, 101, 52)
                : Color.FromArgb(153, 27, 27)
        };

        var statusSub = new Label
        {
            Text = _user.HasAccess
                ? "El usuario tiene acceso a DriverAI."
                : "El usuario no tiene acceso activo.",
            Left = 20,
            Top = 60,
            Width = 320,
            Height = 28,
            Font = new Font("Segoe UI", 10),
            ForeColor = Color.FromArgb(71, 85, 105)
        };

        statusPanel.Controls.Add(statusTitle);
        statusPanel.Controls.Add(statusSub);

        _extendButton.Text = "Extender 30 días";
        _extendButton.Left = 580;
        _extendButton.Top = 150;
        _extendButton.Width = 170;
        _extendButton.Height = 38;
        _extendButton.BackColor = Color.FromArgb(37, 99, 235);
        _extendButton.ForeColor = Color.White;
        _extendButton.FlatStyle = FlatStyle.Flat;
        _extendButton.FlatAppearance.BorderSize = 0;
        _extendButton.Click += async (_, _) => await ExtendSubscriptionAsync();

        var paymentsTitle = new Label
        {
            Text = "Historial de pagos",
            Left = 24,
            Top = 185,
            Width = 300,
            Height = 30,
            Font = new Font("Segoe UI", 16, FontStyle.Bold),
            ForeColor = Color.FromArgb(15, 23, 42)
        };

        _statusLabel.Left = 340;
        _statusLabel.Top = 190;
        _statusLabel.Width = 580;
        _statusLabel.ForeColor = Color.FromArgb(71, 85, 105);

        BuildGrid();

        Controls.Add(title);
        Controls.Add(info);
        Controls.Add(statusPanel);
        Controls.Add(_extendButton);
        Controls.Add(paymentsTitle);
        Controls.Add(_statusLabel);
        Controls.Add(_paymentsGrid);
    }

    private void BuildGrid()
    {
        _paymentsGrid.Left = 24;
        _paymentsGrid.Top = 230;
        _paymentsGrid.Width = 930;
        _paymentsGrid.Height = 390;
        _paymentsGrid.ReadOnly = true;
        _paymentsGrid.AllowUserToAddRows = false;
        _paymentsGrid.AllowUserToDeleteRows = false;
        _paymentsGrid.SelectionMode =
            DataGridViewSelectionMode.FullRowSelect;
        _paymentsGrid.MultiSelect = false;
        _paymentsGrid.AutoGenerateColumns = false;
        _paymentsGrid.AutoSizeColumnsMode =
            DataGridViewAutoSizeColumnsMode.Fill;
        _paymentsGrid.BackgroundColor = Color.White;
        _paymentsGrid.BorderStyle = BorderStyle.None;
        _paymentsGrid.RowHeadersVisible = false;

        _paymentsGrid.EnableHeadersVisualStyles = false;
        _paymentsGrid.ColumnHeadersDefaultCellStyle.BackColor =
            Color.FromArgb(15, 23, 42);
        _paymentsGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        _paymentsGrid.ColumnHeadersDefaultCellStyle.Font =
            new Font("Segoe UI", 9, FontStyle.Bold);

        _paymentsGrid.DefaultCellStyle.Font =
            new Font("Segoe UI", 9);

        _paymentsGrid.AlternatingRowsDefaultCellStyle.BackColor =
            Color.FromArgb(248, 250, 252);

        _paymentsGrid.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "Id",
            DataPropertyName = "Id",
            FillWeight = 45
        });

        _paymentsGrid.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "Monto",
            DataPropertyName = "Amount",
            FillWeight = 80
        });

        _paymentsGrid.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "Estado",
            DataPropertyName = "Status",
            FillWeight = 90
        });

        _paymentsGrid.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "Tipo",
            DataPropertyName = "PaymentType",
            FillWeight = 120
        });

        _paymentsGrid.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "Desde",
            DataPropertyName = "PaidFrom",
            FillWeight = 110,
            DefaultCellStyle =
            {
                Format = "dd/MM/yyyy"
            }
        });

        _paymentsGrid.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "Hasta",
            DataPropertyName = "PaidUntil",
            FillWeight = 110,
            DefaultCellStyle =
            {
                Format = "dd/MM/yyyy"
            }
        });

        _paymentsGrid.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "Referencia",
            DataPropertyName = "SinpeReferenceNumber",
            FillWeight = 120
        });

        _paymentsGrid.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "Creado",
            DataPropertyName = "CreatedAt",
            FillWeight = 120,
            DefaultCellStyle =
            {
                Format = "dd/MM/yyyy HH:mm"
            }
        });

        _paymentsGrid.CellFormatting += GridCellFormatting;
    }

    private async Task LoadPaymentsAsync()
    {
        try
        {
            _statusLabel.Text = "Cargando pagos...";

            var userPayments =
                await _paymentsService.GetPaymentsByUserAsync(_user.Id);

            userPayments = userPayments
                .OrderByDescending(x => x.CreatedAt)
                .ToList();

            _paymentsGrid.DataSource = null;
            _paymentsGrid.DataSource = userPayments;

            var approved = userPayments
                .Where(x => x.Status.Equals(
                    "APPROVED",
                    StringComparison.OrdinalIgnoreCase))
                .ToList();

            _statusLabel.Text =
                $"Pagos: {userPayments.Count} | Aprobados: {approved.Count} | Total: ₡{approved.Sum(x => x.Amount):N0}";
        }
        catch (Exception ex)
        {
            _statusLabel.Text = $"Error cargando pagos: {ex.Message}";
        }
    }

    private async Task ExtendSubscriptionAsync()
    {
        var confirm = MessageBox.Show(
            $"¿Extender 30 días a {_user.Username}?",
            "Confirmar extensión",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question
        );

        if (confirm != DialogResult.Yes)
        {
            return;
        }

        try
        {
            await _usersService.ExtendSubscriptionAsync(
                _user.Id,
                30,
                "MANUAL_ADMIN",
                "Extensión manual desde AdminPanel"
            );

            MessageBox.Show("Suscripción extendida correctamente.");

            await LoadPaymentsAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Error extendiendo suscripción: {ex.Message}",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
        }
    }

    private void GridCellFormatting(
        object? sender,
        DataGridViewCellFormattingEventArgs e
    )
    {
        var property =
            _paymentsGrid.Columns[e.ColumnIndex].DataPropertyName;

        if (property == "PaymentType")
        {
            var type = e.Value?.ToString()?.ToUpperInvariant();

            if (type == "MANUAL_ADJUSTMENT")
            {
                e.CellStyle.BackColor =
                    Color.FromArgb(219, 234, 254);
                e.CellStyle.ForeColor =
                    Color.FromArgb(30, 64, 175);
                e.CellStyle.Font =
                    new Font("Segoe UI", 9, FontStyle.Bold);
            }

            if (type == "REFERRAL_REWARD")
            {
                e.CellStyle.BackColor =
                    Color.FromArgb(220, 252, 231);
                e.CellStyle.ForeColor =
                    Color.FromArgb(22, 101, 52);
                e.CellStyle.Font =
                    new Font("Segoe UI", 9, FontStyle.Bold);
            }

            return;
        }

        if (property != "Status")
        {
            return;
        }

        var value = e.Value?.ToString()?.ToUpperInvariant();

        if (value == "APPROVED")
        {
            e.CellStyle.BackColor = Color.FromArgb(220, 252, 231);
            e.CellStyle.ForeColor = Color.FromArgb(22, 101, 52);
            e.CellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
        }
        else if (value == "PENDING")
        {
            e.CellStyle.BackColor = Color.FromArgb(254, 243, 199);
            e.CellStyle.ForeColor = Color.FromArgb(146, 64, 14);
            e.CellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
        }
        else if (value == "REJECTED")
        {
            e.CellStyle.BackColor = Color.FromArgb(254, 226, 226);
            e.CellStyle.ForeColor = Color.FromArgb(153, 27, 27);
            e.CellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
        }
    }

    private static string FormatDate(DateTime? date)
    {
        return date == null
            ? "-"
            : date.Value.ToString("dd/MM/yyyy");
    }
}