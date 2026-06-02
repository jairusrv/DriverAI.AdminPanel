namespace DriverAI.AdminPanel.Forms;

public class MainForm : Form
{
    public MainForm()
    {
        Text = "DriverAI Admin Panel";
        Width = 900;
        Height = 600;
        StartPosition = FormStartPosition.CenterScreen;

        BuildUi();
    }
    /// <summary>
    /// Construye la interfaz de usuario del formulario principal del panel de administración.
    /// </summary>
    private void BuildUi()
    {
        var title = new Label
        {
            Text = "DriverAI Admin Panel",
            Font = new Font("Segoe UI", 22, FontStyle.Bold),
            AutoSize = true,
            Left = 30,
            Top = 25
        };

        var paymentsButton = new Button
        {
            Text = "Pagos SINPE",
            Left = 30,
            Top = 90,
            Width = 200,
            Height = 45
        };

        paymentsButton.Click += (_, _) =>
        {
            var form = new PaymentsForm();
            form.ShowDialog();
        };

        Controls.Add(title);
        Controls.Add(paymentsButton);

        var usersButton = new Button
        {
            Text = "Usuarios",
            Left = 250,
            Top = 90,
            Width = 200,
            Height = 45
        };

        usersButton.Click += (_, _) =>
        {
            var form = new UsersForm();
            form.ShowDialog();
        };

        Controls.Add(usersButton);
    }


}