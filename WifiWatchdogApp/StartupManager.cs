using Microsoft.Win32;
using System.Windows.Forms;
using System.IO;

namespace WifiWatchdogApp
{
    public static class StartupManager
    {
        private const string RUN_KEY = @"Software\\Microsoft\\Windows\\CurrentVersion\\Run";
        private const string APP_NAME = "WifiWatchdogLauncher";
        private static string LauncherPath => Path.Combine(Application.StartupPath, "WifiWatchdogLauncher.exe");

        public static void SetRunAtStartup(bool enable)
        {
            using (var key = Registry.CurrentUser.OpenSubKey(RUN_KEY, true))
            {
                if (enable)
                    key.SetValue(APP_NAME, '"' + LauncherPath + '"');
                else
                    key.DeleteValue(APP_NAME, false);
            }
        }

        public static bool IsRunAtStartupEnabled()
        {
            using (var key = Registry.CurrentUser.OpenSubKey(RUN_KEY, false))
            {
                var value = key?.GetValue(APP_NAME) as string;
                return value != null && value.Trim('"').Equals(LauncherPath, System.StringComparison.OrdinalIgnoreCase);
            }
        }
    }
}
