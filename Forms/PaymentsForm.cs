using DriverAI.AdminPanel.Models;
using DriverAI.AdminPanel.Services;

namespace DriverAI.AdminPanel.Forms;

public class PaymentsForm : Form
{
    private readonly PaymentsService _paymentsService = new();

    private readonly DataGridView _grid = new();
    private readonly Button _refreshButton = new();
    private readonly Button _approveButton = new();
    private readonly Button _rejectButton = new();
    private readonly ComboBox _statusFilter = new();
    private readonly TextBox _searchBox = new();
    private readonly Label _statusLabel = new();

    private List<PaymentDto> _payments = new();

    public PaymentsForm()
    {
        Text = "Pagos SINPE";
        Width = 1250;
        Height = 720;
        StartPosition = FormStartPosition.CenterParent;
        BackColor = Color.FromArgb(248, 250, 252);

        BuildUi();

        Load += async (_, _) => await LoadPaymentsAsync();
    }

    private void BuildUi()
    {
        var title = new Label
        {
            Text = "Pagos SINPE",
            Left = 20,
            Top = 15,
            Width = 300,
            Height = 40,
            Font = new Font("Segoe UI", 22, FontStyle.Bold),
            ForeColor = Color.FromArgb(15, 23, 42)
        };

        _refreshButton.Text = "Actualizar";
        _refreshButton.Left = 20;
        _refreshButton.Top = 70;
        _refreshButton.Width = 120;
        _refreshButton.Height = 36;
        _refreshButton.Click += async (_, _) => await LoadPaymentsAsync();

        _approveButton.Text = "Aprobar";
        _approveButton.Left = 150;
        _approveButton.Top = 70;
        _approveButton.Width = 120;
        _approveButton.Height = 36;
        _approveButton.BackColor = Color.FromArgb(34, 197, 94);
        _approveButton.ForeColor = Color.White;
        _approveButton.FlatStyle = FlatStyle.Flat;
        _approveButton.FlatAppearance.BorderSize = 0;
        _approveButton.Click += async (_, _) => await ApproveSelectedAsync();

        _rejectButton.Text = "Rechazar";
        _rejectButton.Left = 280;
        _rejectButton.Top = 70;
        _rejectButton.Width = 120;
        _rejectButton.Height = 36;
        _rejectButton.BackColor = Color.FromArgb(239, 68, 68);
        _rejectButton.ForeColor = Color.White;
        _rejectButton.FlatStyle = FlatStyle.Flat;
        _rejectButton.FlatAppearance.BorderSize = 0;
        _rejectButton.Click += async (_, _) => await RejectSelectedAsync();

        _statusFilter.Left = 420;
        _statusFilter.Top = 72;
        _statusFilter.Width = 160;
        _statusFilter.DropDownStyle = ComboBoxStyle.DropDownList;
        _statusFilter.Items.AddRange(
            new object[]
            {
                "Todos",
                "Pendientes",
                "Aprobados",
                "Rechazados"
            }
        );
        _statusFilter.SelectedIndex = 0;
        _statusFilter.SelectedIndexChanged += (_, _) => ApplyFilters();

        _searchBox.Left = 600;
        _searchBox.Top = 72;
        _searchBox.Width = 260;
        _searchBox.PlaceholderText = "Buscar UserId, ref o teléfono...";
        _searchBox.TextChanged += (_, _) => ApplyFilters();

        _statusLabel.Left = 880;
        _statusLabel.Top = 78;
        _statusLabel.Width = 320;
        _statusLabel.ForeColor = Color.FromArgb(71, 85, 105);

        BuildGrid();

        Controls.Add(title);
        Controls.Add(_refreshButton);
        Controls.Add(_approveButton);
        Controls.Add(_rejectButton);
        Controls.Add(_statusFilter);
        Controls.Add(_searchBox);
        Controls.Add(_statusLabel);
        Controls.Add(_grid);
    }

    private void BuildGrid()
    {
        _grid.Left = 20;
        _grid.Top = 125;
        _grid.Width = 1190;
        _grid.Height = 530;
        _grid.ReadOnly = true;
        _grid.AllowUserToAddRows = false;
        _grid.AllowUserToDeleteRows = false;
        _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _grid.MultiSelect = false;
        _grid.AutoGenerateColumns = false;
        _grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        _grid.BackgroundColor = Color.White;
        _grid.BorderStyle = BorderStyle.None;
        _grid.RowHeadersVisible = false;

        _grid.EnableHeadersVisualStyles = false;
        _grid.ColumnHeadersDefaultCellStyle.BackColor =
            Color.FromArgb(15, 23, 42);
        _grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        _grid.ColumnHeadersDefaultCellStyle.Font =
            new Font("Segoe UI", 9, FontStyle.Bold);
        _grid.DefaultCellStyle.Font = new Font("Segoe UI", 9);
        _grid.DefaultCellStyle.SelectionBackColor =
            Color.FromArgb(37, 99, 235);
        _grid.DefaultCellStyle.SelectionForeColor = Color.White;
        _grid.AlternatingRowsDefaultCellStyle.BackColor =
            Color.FromArgb(248, 250, 252);

        _grid.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "Id",
            DataPropertyName = "Id",
            FillWeight = 45
        });

        _grid.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "UserId",
            DataPropertyName = "UserId",
            FillWeight = 55
        });

        _grid.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "Monto",
            DataPropertyName = "Amount",
            FillWeight = 70
        });

        _grid.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "Estado",
            DataPropertyName = "Status",
            FillWeight = 80
        });

        _grid.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "Proveedor",
            DataPropertyName = "Provider",
            FillWeight = 95
        });

        _grid.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "Referencia",
            DataPropertyName = "SinpeReferenceNumber",
            FillWeight = 110
        });

        _grid.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "Tel SINPE",
            DataPropertyName = "SinpeSenderPhone",
            FillWeight = 95
        });

        _grid.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "Desde",
            DataPropertyName = "PaidFrom",
            FillWeight = 120,
            DefaultCellStyle =
            {
                Format = "dd/MM/yyyy"
            }
        });

        _grid.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "Hasta",
            DataPropertyName = "PaidUntil",
            FillWeight = 120,
            DefaultCellStyle =
            {
                Format = "dd/MM/yyyy"
            }
        });

        _grid.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "Creado",
            DataPropertyName = "CreatedAt",
            FillWeight = 120,
            DefaultCellStyle =
            {
                Format = "dd/MM/yyyy HH:mm"
            }
        });

        _grid.CellFormatting += GridCellFormatting;
    }

    private async Task LoadPaymentsAsync()
    {
        try
        {
            _statusLabel.Text = "Cargando pagos...";

            _payments = await _paymentsService.GetPaymentsAsync();

            ApplyFilters();
        }
        catch (Exception ex)
        {
            _statusLabel.Text = $"Error cargando pagos: {ex.Message}";
        }
    }

    private void ApplyFilters()
    {
        IEnumerable<PaymentDto> query = _payments;

        var selected = _statusFilter.SelectedItem?.ToString() ?? "Todos";

        query = selected switch
        {
            "Pendientes" => query.Where(x =>
                x.Status.Equals("PENDING", StringComparison.OrdinalIgnoreCase)),
            "Aprobados" => query.Where(x =>
                x.Status.Equals("APPROVED", StringComparison.OrdinalIgnoreCase)),
            "Rechazados" => query.Where(x =>
                x.Status.Equals("REJECTED", StringComparison.OrdinalIgnoreCase)),
            _ => query
        };

        var search = _searchBox.Text.Trim().ToLowerInvariant();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x =>
                x.UserId.ToString().Contains(search) ||
                x.Id.ToString().Contains(search) ||
                (x.ProviderReference ?? "")
                    .ToLowerInvariant()
                    .Contains(search) ||
                (x.SinpeReferenceNumber ?? "")
                    .ToLowerInvariant()
                    .Contains(search) ||
                (x.SinpeSenderPhone ?? "")
                    .ToLowerInvariant()
                    .Contains(search)
            );
        }

        var result = query
            .OrderByDescending(x =>
                x.Status.Equals("PENDING", StringComparison.OrdinalIgnoreCase))
            .ThenByDescending(x => x.CreatedAt)
            .ToList();

        _grid.DataSource = null;
        _grid.DataSource = result;

        var pending = _payments.Count(x =>
            x.Status.Equals("PENDING", StringComparison.OrdinalIgnoreCase));

        _statusLabel.Text =
            $"Mostrando {result.Count} | Pendientes: {pending}";
    }

    private void GridCellFormatting(
        object? sender,
        DataGridViewCellFormattingEventArgs e
    )
    {
        if (_grid.Columns[e.ColumnIndex].DataPropertyName != "Status")
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

    private PaymentDto? GetSelectedPayment()
    {
        if (_grid.CurrentRow?.DataBoundItem is PaymentDto payment)
        {
            return payment;
        }

        return null;
    }

    private async Task ApproveSelectedAsync()
    {
        var payment = GetSelectedPayment();

        if (payment == null)
        {
            MessageBox.Show("Seleccione un pago.");
            return;
        }

        if (!payment.Status.Equals("PENDING", StringComparison.OrdinalIgnoreCase))
        {
            MessageBox.Show("Solo se pueden aprobar pagos pendientes.");
            return;
        }

        var confirm = MessageBox.Show(
            $"¿Aprobar pago #{payment.Id} por ₡{payment.Amount:N0}?",
            "Confirmar aprobación",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question
        );

        if (confirm != DialogResult.Yes)
        {
            return;
        }

        try
        {
            await _paymentsService.ApprovePaymentAsync(payment.Id);
            await LoadPaymentsAsync();
            MessageBox.Show("Pago aprobado correctamente.");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error aprobando pago: {ex.Message}");
        }
    }

    private async Task RejectSelectedAsync()
    {
        var payment = GetSelectedPayment();

        if (payment == null)
        {
            MessageBox.Show("Seleccione un pago.");
            return;
        }

        if (!payment.Status.Equals("PENDING", StringComparison.OrdinalIgnoreCase))
        {
            MessageBox.Show("Solo se pueden rechazar pagos pendientes.");
            return;
        }

        var confirm = MessageBox.Show(
            $"¿Rechazar pago #{payment.Id}?",
            "Confirmar rechazo",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning
        );

        if (confirm != DialogResult.Yes)
        {
            return;
        }

        try
        {
            await _paymentsService.RejectPaymentAsync(payment.Id);
            await LoadPaymentsAsync();
            MessageBox.Show("Pago rechazado correctamente.");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error rechazando pago: {ex.Message}");
        }
    }
}