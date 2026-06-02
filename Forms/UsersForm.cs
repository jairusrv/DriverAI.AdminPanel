using DriverAI.AdminPanel.Models;
using DriverAI.AdminPanel.Services;

namespace DriverAI.AdminPanel.Forms;

public class UsersForm : Form
{
    private readonly UsersService _service = new();

    private readonly DataGridView _grid = new();

    private readonly Button _refreshButton = new();

    private List<UserDto> _users = [];

    public UsersForm()
    {
        Text = "Usuarios";
        Width = 1200;
        Height = 700;

        BuildUi();

        Load += async (_, _) =>
        {
            await LoadUsers();
        };
    }

    private void BuildUi()
    {
        _refreshButton.Text = "Actualizar";
        _refreshButton.Left = 20;
        _refreshButton.Top = 20;
        _refreshButton.Width = 120;

        _refreshButton.Click += async (_, _) =>
        {
            await LoadUsers();
        };

        _grid.Left = 20;
        _grid.Top = 70;
        _grid.Width = 1140;
        _grid.Height = 560;

        _grid.AutoGenerateColumns = true;

        Controls.Add(_refreshButton);
        Controls.Add(_grid);
    }

    private async Task LoadUsers()
    {
        _users = await _service.GetUsersAsync();

        _grid.DataSource = null;
        _grid.DataSource = _users;
    }
}