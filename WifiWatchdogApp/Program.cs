using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Security.Principal;
using System.IO;

namespace WifiWatchdogApp
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            if (!IsAdministrator())
            {
                // Optionally show a message, or just exit silently
                MessageBox.Show(
                    "Wi-Fi Watchdog must be run as administrator. Please use the launcher.",
                    "Wi-Fi Watchdog",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }
            // If admin, skip popup and start monitoring
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new TrayAppContext());
        }

        private static bool IsAdministrator()
        {
            var wi = WindowsIdentity.GetCurrent();
            var wp = new WindowsPrincipal(wi);
            return wp.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}
