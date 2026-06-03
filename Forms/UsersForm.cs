using DriverAI.AdminPanel.Models;
using DriverAI.AdminPanel.Services;

namespace DriverAI.AdminPanel.Forms;

public class UsersForm : Form
{
    private readonly UsersService _service = new();

    private readonly DataGridView _grid = new();
    private readonly Button _refreshButton = new();
    private readonly Button _detailsButton = new();
    private readonly ComboBox _statusFilter = new();
    private readonly TextBox _searchBox = new();
    private readonly Label _statusLabel = new();

    private List<UserDto> _users = new();

    public UsersForm()
    {
        Text = "Usuarios";
        Width = 1250;
        Height = 720;
        StartPosition = FormStartPosition.CenterParent;
        BackColor = Color.FromArgb(248, 250, 252);

        BuildUi();

        Load += async (_, _) => await LoadUsersAsync();
    }

    private void BuildUi()
    {
        var title = new Label
        {
            Text = "Usuarios",
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
        _refreshButton.Click += async (_, _) => await LoadUsersAsync();

        _detailsButton.Text = "Ver detalles";
        _detailsButton.Left = 150;
        _detailsButton.Top = 70;
        _detailsButton.Width = 130;
        _detailsButton.Height = 36;
        _detailsButton.BackColor = Color.FromArgb(37, 99, 235);
        _detailsButton.ForeColor = Color.White;
        _detailsButton.FlatStyle = FlatStyle.Flat;
        _detailsButton.FlatAppearance.BorderSize = 0;
        _detailsButton.Click += (_, _) => OpenSelectedUserDetails();

        _statusFilter.Left = 300;
        _statusFilter.Top = 72;
        _statusFilter.Width = 160;
        _statusFilter.DropDownStyle = ComboBoxStyle.DropDownList;
        _statusFilter.Items.AddRange(
            new object[]
            {
                "Todos",
                "Activos",
                "Vencidos",
                "En prueba"
            }
        );
        _statusFilter.SelectedIndex = 0;
        _statusFilter.SelectedIndexChanged += (_, _) => ApplyFilters();

        _searchBox.Left = 480;
        _searchBox.Top = 72;
        _searchBox.Width = 300;
        _searchBox.PlaceholderText = "Buscar teléfono, email o usuario...";
        _searchBox.TextChanged += (_, _) => ApplyFilters();

        _statusLabel.Left = 800;
        _statusLabel.Top = 78;
        _statusLabel.Width = 400;
        _statusLabel.ForeColor = Color.FromArgb(71, 85, 105);

        BuildGrid();

        Controls.Add(title);
        Controls.Add(_refreshButton);
        Controls.Add(_detailsButton);
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
            HeaderText = "Teléfono",
            DataPropertyName = "PhoneNumber",
            FillWeight = 90
        });

        _grid.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "Usuario",
            DataPropertyName = "Username",
            FillWeight = 120
        });

        _grid.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "Email",
            DataPropertyName = "Email",
            FillWeight = 180
        });

        _grid.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "Activo",
            DataPropertyName = "IsSubscriptionActive",
            FillWeight = 65
        });

        _grid.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "Acceso",
            DataPropertyName = "HasAccess",
            FillWeight = 65
        });

        _grid.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "Trial días",
            DataPropertyName = "RemainingTrialDays",
            FillWeight = 75
        });

        _grid.Columns.Add(new DataGridViewTextBoxColumn
        {
            HeaderText = "Vence",
            DataPropertyName = "SubscriptionExpiryDate",
            FillWeight = 120,
            DefaultCellStyle =
            {
                Format = "dd/MM/yyyy"
            }
        });

        _grid.CellFormatting += GridCellFormatting;
        _grid.CellDoubleClick += (_, _) => OpenSelectedUserDetails();
    }

    private async Task LoadUsersAsync()
    {
        try
        {
            _statusLabel.Text = "Cargando usuarios...";

            _users = await _service.GetUsersAsync();

            ApplyFilters();
        }
        catch (Exception ex)
        {
            _statusLabel.Text = $"Error cargando usuarios: {ex.Message}";
        }
    }

    private void ApplyFilters()
    {
        IEnumerable<UserDto> query = _users;

        var selected = _statusFilter.SelectedItem?.ToString() ?? "Todos";

        query = selected switch
        {
            "Activos" => query.Where(x => x.HasAccess),
            "Vencidos" => query.Where(x => !x.HasAccess),
            "En prueba" => query.Where(x =>
                x.RemainingTrialDays > 0 &&
                !x.IsSubscriptionActive),
            _ => query
        };

        var search = _searchBox.Text.Trim().ToLowerInvariant();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(x =>
                x.Id.ToString().Contains(search) ||
                (x.PhoneNumber ?? "")
                    .ToLowerInvariant()
                    .Contains(search) ||
                (x.Email ?? "")
                    .ToLowerInvariant()
                    .Contains(search) ||
                (x.Username ?? "")
                    .ToLowerInvariant()
                    .Contains(search)
            );
        }

        var result = query
            .OrderByDescending(x => x.HasAccess)
            .ThenByDescending(x => x.SubscriptionExpiryDate)
            .ToList();

        _grid.DataSource = null;
        _grid.DataSource = result;

        var active = _users.Count(x => x.HasAccess);
        var expired = _users.Count(x => !x.HasAccess);

        _statusLabel.Text =
            $"Mostrando {result.Count} | Activos: {active} | Vencidos: {expired}";
    }

    private void GridCellFormatting(
        object? sender,
        DataGridViewCellFormattingEventArgs e
    )
    {
        var property = _grid.Columns[e.ColumnIndex].DataPropertyName;

        if (property == "HasAccess")
        {
            var value = e.Value is bool b && b;

            e.CellStyle.BackColor = value
                ? Color.FromArgb(220, 252, 231)
                : Color.FromArgb(254, 226, 226);

            e.CellStyle.ForeColor = value
                ? Color.FromArgb(22, 101, 52)
                : Color.FromArgb(153, 27, 27);

            e.CellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
        }

        if (property == "RemainingTrialDays")
        {
            var value = e.Value is int days ? days : 0;

            if (value > 0)
            {
                e.CellStyle.BackColor = Color.FromArgb(254, 243, 199);
                e.CellStyle.ForeColor = Color.FromArgb(146, 64, 14);
                e.CellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            }
        }
    }

    private UserDto? GetSelectedUser()
    {
        if (_grid.CurrentRow?.DataBoundItem is UserDto user)
        {
            return user;
        }

        return null;
    }

    private void OpenSelectedUserDetails()
    {
        var user = GetSelectedUser();

        if (user == null)
        {
            MessageBox.Show("Seleccione un usuario.");
            return;
        }

        var form = new UserDetailsForm(user);
        form.ShowDialog();
    }

    private static string FormatDate(DateTime? date)
    {
        return date == null
            ? "-"
            : date.Value.ToString("dd/MM/yyyy");
    }
}