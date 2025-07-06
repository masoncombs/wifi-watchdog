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
            // Single-instance Mutex to prevent multiple tray icons
            bool createdNew;
            using (var mutex = new System.Threading.Mutex(true, "WifiWatchdogApp_SingleInstance", out createdNew))
            {
                if (!createdNew)
                {
                    // Already running, exit silently
                    return;
                }

                if (!IsAdministrator())
                {
                    MessageBox.Show(
                        "Wi-Fi Watchdog must be run as administrator. Please use the launcher.",
                        "Wi-Fi Watchdog",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    return;
                }

                bool isFirstRunOnly = args != null && args.Length > 0 && args[0] == "--firstrun";
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                if (isFirstRunOnly)
                {
                    // Only do first-run setup, then exit
                    var ctx = new TrayAppContext(true); // pass flag for first-run only
                    // TrayAppContext will exit after setup
                }
                else
                {
                    Application.Run(new TrayAppContext());
                }
            }
        }

        private static bool IsAdministrator()
        {
            var wi = WindowsIdentity.GetCurrent();
            var wp = new WindowsPrincipal(wi);
            return wp.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
}
