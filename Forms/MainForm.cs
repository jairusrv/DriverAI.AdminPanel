namespace DriverAI.AdminPanel.Forms;

public class MainForm : Form
{
    private readonly Panel _sidebar = new();
    private readonly Panel _content = new();

    private readonly Color Primary = Color.FromArgb(37, 99, 235);
    private readonly Color Dark = Color.FromArgb(15, 23, 42);
    private readonly Color Background = Color.FromArgb(248, 250, 252);
    private readonly Color Card = Color.White;

    public MainForm()
    {
        Text = "DriverAI Admin Panel";
        Width = 1200;
        Height = 750;
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = Background;

        BuildLayout();
        ShowDashboard();
    }

    private void BuildLayout()
    {
        _sidebar.Dock = DockStyle.Left;
        _sidebar.Width = 240;
        _sidebar.BackColor = Dark;

        _content.Dock = DockStyle.Fill;
        _content.BackColor = Background;
        _content.Padding = new Padding(30);

        Controls.Add(_content);
        Controls.Add(_sidebar);

        BuildSidebar();
    }

    private void BuildSidebar()
    {
        var title = new Label
        {
            Text = "DriverAI",
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 24, FontStyle.Bold),
            Left = 25,
            Top = 30,
            Width = 190,
            Height = 45
        };

        var subtitle = new Label
        {
            Text = "Admin Panel",
            ForeColor = Color.FromArgb(148, 163, 184),
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            Left = 28,
            Top = 75,
            Width = 160,
            Height = 24
        };

        _sidebar.Controls.Add(title);
        _sidebar.Controls.Add(subtitle);

        AddMenuButton("Dashboard", 130, ShowDashboard);
        AddMenuButton("Pagos SINPE", 185, OpenPayments);
        AddMenuButton("Usuarios", 240, OpenUsers);
        AddMenuButton("Cerrar", 610, Close);
    }

    private void AddMenuButton(
        string text,
        int top,
        Action action
    )
    {
        var button = new Button
        {
            Text = text,
            Left = 20,
            Top = top,
            Width = 200,
            Height = 42,
            FlatStyle = FlatStyle.Flat,
            BackColor = Dark,
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 11, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(15, 0, 0, 0),
            Cursor = Cursors.Hand
        };

        button.FlatAppearance.BorderSize = 0;

        button.MouseEnter += (_, _) =>
        {
            button.BackColor = Primary;
        };

        button.MouseLeave += (_, _) =>
        {
            button.BackColor = Dark;
        };

        button.Click += (_, _) => action();

        _sidebar.Controls.Add(button);
    }

    private void ShowDashboard()
    {
        _content.Controls.Clear();

        AddHeader("Dashboard financiero");

        AddKpiCard(
            title: "Pagos pendientes",
            value: "Ver pagos",
            left: 0,
            top: 90,
            color: Color.FromArgb(245, 158, 11),
            onClick: OpenPayments
        );

        AddKpiCard(
            title: "Usuarios",
            value: "Ver usuarios",
            left: 280,
            top: 90,
            color: Primary,
            onClick: OpenUsers
        );

        AddKpiCard(
            title: "Suscripciones",
            value: "Activas / vencidas",
            left: 560,
            top: 90,
            color: Color.FromArgb(34, 197, 94),
            onClick: OpenUsers
        );

        var info = new Label
        {
            Text =
                "Usa el menú lateral para aprobar pagos SINPE, revisar usuarios y consultar el estado general de DriverAI.",
            Left = 0,
            Top = 240,
            Width = 780,
            Height = 60,
            Font = new Font("Segoe UI", 12),
            ForeColor = Color.FromArgb(71, 85, 105)
        };

        _content.Controls.Add(info);
    }

    private void AddHeader(string title)
    {
        var titleLabel = new Label
        {
            Text = title,
            Left = 0,
            Top = 0,
            Width = 700,
            Height = 45,
            Font = new Font("Segoe UI", 26, FontStyle.Bold),
            ForeColor = Color.FromArgb(15, 23, 42)
        };

        var subtitle = new Label
        {
            Text = "Panel administrativo de DriverAI",
            Left = 2,
            Top = 48,
            Width = 500,
            Height = 25,
            Font = new Font("Segoe UI", 10),
            ForeColor = Color.FromArgb(100, 116, 139)
        };

        _content.Controls.Add(titleLabel);
        _content.Controls.Add(subtitle);
    }

    private void AddKpiCard(
        string title,
        string value,
        int left,
        int top,
        Color color,
        Action onClick
    )
    {
        var panel = new Panel
        {
            Left = left,
            Top = top,
            Width = 250,
            Height = 115,
            BackColor = Card,
            Cursor = Cursors.Hand
        };

        panel.Paint += (_, e) =>
        {
            using var pen = new Pen(Color.FromArgb(226, 232, 240), 1);
            e.Graphics.DrawRectangle(
                pen,
                0,
                0,
                panel.Width - 1,
                panel.Height - 1
            );
        };

        var line = new Panel
        {
            Left = 0,
            Top = 0,
            Width = 6,
            Height = 115,
            BackColor = color
        };

        var titleLabel = new Label
        {
            Text = title,
            Left = 20,
            Top = 18,
            Width = 210,
            Height = 24,
            Font = new Font("Segoe UI", 10, FontStyle.Bold),
            ForeColor = Color.FromArgb(100, 116, 139)
        };

        var valueLabel = new Label
        {
            Text = value,
            Left = 20,
            Top = 48,
            Width = 210,
            Height = 38,
            Font = new Font("Segoe UI", 18, FontStyle.Bold),
            ForeColor = Color.FromArgb(15, 23, 42)
        };

        panel.Controls.Add(line);
        panel.Controls.Add(titleLabel);
        panel.Controls.Add(valueLabel);

        panel.Click += (_, _) => onClick();
        titleLabel.Click += (_, _) => onClick();
        valueLabel.Click += (_, _) => onClick();

        _content.Controls.Add(panel);
    }

    private void OpenPayments()
    {
        var form = new PaymentsForm();
        form.ShowDialog();
    }

    private void OpenUsers()
    {
        var form = new UsersForm();
        form.ShowDialog();
    }
}