using Microsoft.Win32;
using System.Diagnostics;

namespace WifiWatchdogLauncher
{
    public static class StartupManager
    {
        private const string RUN_KEY = @"Software\\Microsoft\\Windows\\CurrentVersion\\Run";
        private const string APP_NAME = "WifiWatchdogLauncher";

        public static void SetRunAtStartup(bool enable)
        {
            string exePath = System.Windows.Forms.Application.ExecutablePath;
            using (var key = Registry.CurrentUser.OpenSubKey(RUN_KEY, true))
            {
                if (enable)
                    key.SetValue(APP_NAME, '"' + exePath + '"');
                else
                    key.DeleteValue(APP_NAME, false);
            }
        }

        public static bool IsRunAtStartupEnabled()
        {
            string exePath = System.Windows.Forms.Application.ExecutablePath;
            using (var key = Registry.CurrentUser.OpenSubKey(RUN_KEY, false))
            {
                var value = key?.GetValue(APP_NAME) as string;
                return value != null && value.Trim('"').Equals(exePath, System.StringComparison.OrdinalIgnoreCase);
            }
        }

        public static void SetScheduledTaskAtLogin(bool enable)
        {
            string taskName = "WiFi Watchdog";
            string exePath = System.Windows.Forms.Application.ExecutablePath;
            string args = $"/Create /TN \"{taskName}\" /TR \"\"{exePath}\"\" /SC ONLOGON /RL HIGHEST /F";
            string deleteArgs = $"/Delete /TN \"{taskName}\" /F";
            var psi = new ProcessStartInfo
            {
                FileName = "schtasks",
                Arguments = enable ? args : deleteArgs,
                Verb = "runas", // prompt for admin
                UseShellExecute = true,
                CreateNoWindow = true
            };
            try { Process.Start(psi)?.WaitForExit(); } catch { }
        }

        public static bool IsScheduledTaskEnabled()
        {
            string taskName = "WiFi Watchdog";
            var psi = new ProcessStartInfo
            {
                FileName = "schtasks",
                Arguments = $"/Query /TN \"{taskName}\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            try
            {
                using (var proc = Process.Start(psi))
                {
                    string output = proc.StandardOutput.ReadToEnd();
                    proc.WaitForExit();
                    return output.Contains(taskName);
                }
            }
            catch { return false; }
        }
    }
}
