using DriverAI.AdminPanel.Services;

namespace DriverAI.AdminPanel.Forms;

/// <summary>
/// Formulario de inicio de sesión para el panel de administración de DriverAI.
/// </summary>
public class LoginForm : Form
{
    private readonly AuthService _authService = new();

    private readonly TextBox _phoneTextBox = new();
    private readonly TextBox _passwordTextBox = new();
    private readonly Button _loginButton = new();
    private readonly Label _statusLabel = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="LoginForm"/> class.
    /// </summary>
    public LoginForm()
    {
        Text = "DriverAI Admin - Login";
        Width = 420;
        Height = 300;
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;

        BuildUi();
    }
/// <summary>
/// Construye la interfaz de usuario del formulario de inicio de sesión.
/// </summary>
    private void BuildUi()
    {
        var title = new Label
        {
            Text = "DriverAI Admin",
            Font = new Font("Segoe UI", 20, FontStyle.Bold),
            AutoSize = true,
            Left = 95,
            Top = 25
        };

        var phoneLabel = new Label
        {
            Text = "Teléfono",
            Left = 50,
            Top = 85,
            Width = 120
        };

        _phoneTextBox.Left = 160;
        _phoneTextBox.Top = 80;
        _phoneTextBox.Width = 190;

        var passLabel = new Label
        {
            Text = "Contraseña",
            Left = 50,
            Top = 125,
            Width = 120
        };

        _passwordTextBox.Left = 160;
        _passwordTextBox.Top = 120;
        _passwordTextBox.Width = 190;
        _passwordTextBox.UseSystemPasswordChar = true;

        _loginButton.Text = "Ingresar";
        _loginButton.Left = 160;
        _loginButton.Top = 165;
        _loginButton.Width = 190;
        _loginButton.Height = 35;
        _loginButton.Click += async (_, _) => await LoginAsync();

        _statusLabel.Left = 50;
        _statusLabel.Top = 215;
        _statusLabel.Width = 310;
        _statusLabel.ForeColor = Color.Red;

        Controls.Add(title);
        Controls.Add(phoneLabel);
        Controls.Add(_phoneTextBox);
        Controls.Add(passLabel);
        Controls.Add(_passwordTextBox);
        Controls.Add(_loginButton);
        Controls.Add(_statusLabel);
    }
/// <summary>
/// Realiza el proceso de inicio de sesión utilizando el servicio de autenticación.
/// </summary>
/// <returns></returns>
    private async Task LoginAsync()
    {
        _statusLabel.Text = "";

        var phone = _phoneTextBox.Text.Trim();
        var password = _passwordTextBox.Text;

        if (phone.Length != 8)
        {
            _statusLabel.Text = "El teléfono debe tener 8 dígitos.";
            return;
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            _statusLabel.Text = "Ingrese la contraseña.";
            return;
        }

        _loginButton.Enabled = false;
        _loginButton.Text = "Ingresando...";

        try
        {
            var result = await _authService.LoginAsync(
                phone,
                password
            );

            if (result?.Success != true ||
                result.Data?.User == null)
            {
                _statusLabel.Text = "Credenciales inválidas.";
                return;
            }

            if (!string.Equals(
                    result.Data.User.Role,
                    "Admin",
                    StringComparison.OrdinalIgnoreCase))
            {
                _statusLabel.Text =
                    "Este usuario no tiene permisos de administrador.";
                return;
            }

            Hide();

            var main = new MainForm();
            main.FormClosed += (_, _) => Close();
            main.Show();
        }
        catch (Exception ex)
        {
            _statusLabel.Text = $"Error: {ex.Message}";
        }
        finally
        {
            _loginButton.Enabled = true;
            _loginButton.Text = "Ingresar";
        }
    }
}