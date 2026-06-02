using DriverAI.AdminPanel.Forms;

namespace DriverAI.AdminPanel;
/// <summary>
/// The main entry point for the application.
/// </summary>
internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new LoginForm());
    }
}