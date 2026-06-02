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
    private readonly Label _statusLabel = new();

    private List<PaymentDto> _payments = new();

    public PaymentsForm()
    {
        Text = "Pagos SINPE";
        Width = 1100;
        Height = 650;
        StartPosition = FormStartPosition.CenterParent;

        BuildUi();

        Load += async (_, _) => await LoadPaymentsAsync();
    }
/// <summary>
/// Construye la interfaz de usuario del formulario de gestión de pagos, incluyendo botones de acción, etiqueta de estado y una cuadrícula para mostrar los pagos.
/// </summary>
    private void BuildUi()
    {
        _refreshButton.Text = "Actualizar";
        _refreshButton.Left = 20;
        _refreshButton.Top = 20;
        _refreshButton.Width = 120;
        _refreshButton.Click += async (_, _) => await LoadPaymentsAsync();

        _approveButton.Text = "Aprobar";
        _approveButton.Left = 150;
        _approveButton.Top = 20;
        _approveButton.Width = 120;
        _approveButton.Click += async (_, _) => await ApproveSelectedAsync();

        _rejectButton.Text = "Rechazar";
        _rejectButton.Left = 280;
        _rejectButton.Top = 20;
        _rejectButton.Width = 120;
        _rejectButton.Click += async (_, _) => await RejectSelectedAsync();

        _statusLabel.Left = 420;
        _statusLabel.Top = 26;
        _statusLabel.Width = 600;

        _grid.Left = 20;
        _grid.Top = 65;
        _grid.Width = 1040;
        _grid.Height = 520;
        _grid.ReadOnly = true;
        _grid.AllowUserToAddRows = false;
        _grid.AllowUserToDeleteRows = false;
        _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _grid.MultiSelect = false;
        _grid.AutoGenerateColumns = false;
        _grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        _grid.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "Id",
            DataPropertyName = "Id"
        });

        _grid.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "UserId",
            DataPropertyName = "UserId"
        });

        _grid.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "Monto",
            DataPropertyName = "Amount"
        });

        _grid.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "Estado",
            DataPropertyName = "Status"
        });

        _grid.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "Proveedor",
            DataPropertyName = "Provider"
        });

        _grid.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "Ref",
            DataPropertyName = "SinpeReferenceNumber"
        });

        _grid.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "Tel SINPE",
            DataPropertyName = "SinpeSenderPhone"
        });

        _grid.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "Desde",
            DataPropertyName = "PaidFrom"
        });

        _grid.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "Hasta",
            DataPropertyName = "PaidUntil"
        });

        Controls.Add(_refreshButton);
        Controls.Add(_approveButton);
        Controls.Add(_rejectButton);
        Controls.Add(_statusLabel);
        Controls.Add(_grid);
    }
/// <summary>
/// Realiza el proceso de carga de los pagos utilizando el servicio de pagos y actualiza la interfaz de usuario con los resultados.
/// </summary>
/// <returns></returns>
    private async Task LoadPaymentsAsync()
    {
        try
        {
            _statusLabel.Text = "Cargando pagos...";

            _payments = await _paymentsService.GetPaymentsAsync();

            _grid.DataSource = _payments
                .OrderByDescending(x => x.Status == "PENDING")
                .ThenByDescending(x => x.CreatedAt)
                .ToList();

            _statusLabel.Text = $"Pagos cargados: {_payments.Count}";
        }
        catch (Exception ex)
        {
            _statusLabel.Text = $"Error cargando pagos: {ex.Message}";
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
/// <summary>
///     Realiza el proceso de aprobación del pago seleccionado utilizando el servicio de pagos.
/// </summary>
/// <returns></returns>
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
            $"¿Aprobar pago #{payment.Id} por ₡{payment.Amount}?",
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
/// <summary>
/// Realiza el proceso de rechazo del pago seleccionado utilizando el servicio de pagos.
/// </summary>
/// <returns></returns>
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