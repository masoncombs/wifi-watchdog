using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Media;
using System.Linq;

namespace WifiWatchdogLauncher
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            SystemSounds.Exclamation.Play(); // Play popup sound at launcher start
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            bool? darkModeSetting = SettingsReader.TryGetDarkMode();
            bool darkMode = darkModeSetting ?? IsSystemInDarkMode();
            DialogResult result = DarkPromptForm.Show(
                "Do you want to enable Wi-Fi connection protection?",
                "Wi-Fi Watchdog",
                darkMode
            );
            if (result != DialogResult.Yes)
            {
                // Only exit if user explicitly says No or closes the dialog
                Environment.Exit(0);
            }

            // Wait for first-run Wi-Fi setup to complete before prompting for auto-run
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string configDir = Path.Combine(appData, "WifiWatchdogApp");
            string[] configFiles = Directory.Exists(configDir) ? Directory.GetFiles(configDir, "user.config", SearchOption.AllDirectories) : new string[0];
            bool firstRunComplete = false;
            if (configFiles.Length > 0)
            {
                try
                {
                    var config = System.Xml.Linq.XDocument.Load(configFiles[0]);
                    var elem = config.Descendants("setting").FirstOrDefault(e => (string)e.Attribute("name") == "FirstRunComplete");
                    if (elem != null)
                    {
                        var valueElem = elem.Element("value");
                        if (valueElem != null && bool.TryParse(valueElem.Value, out bool firstRunResult))
                            firstRunComplete = firstRunResult;
                    }
                }
                catch { }
            }
            string appPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WifiWatchdogApp.exe");
            if (!firstRunComplete)
            {
                // Start main app for first-run Wi-Fi setup and wait for it to exit
                if (!File.Exists(appPath))
                {
                    DarkPromptForm.ShowInfo($"Could not find WifiWatchdogApp.exe at {appPath}", "Error", darkMode);
                    return;
                }
                var psi = new ProcessStartInfo(appPath, "--firstrun")
                {
                    UseShellExecute = true,
                    Verb = "runas",
                    WorkingDirectory = Path.GetDirectoryName(appPath)
                };
                try { Process.Start(psi)?.WaitForExit(); } catch (Exception ex)
                {
                    DarkPromptForm.ShowInfo(
                        "Failed to start WifiWatchdogApp.\n" + ex.Message,
                        "Wi-Fi Watchdog Error",
                        darkMode
                    );
                    return;
                }
                // Re-check flag after main app exits
                configFiles = Directory.Exists(configDir) ? Directory.GetFiles(configDir, "user.config", SearchOption.AllDirectories) : new string[0];
                if (configFiles.Length > 0)
                {
                    try
                    {
                        var config = System.Xml.Linq.XDocument.Load(configFiles[0]);
                        var elem = config.Descendants("setting").FirstOrDefault(e => (string)e.Attribute("name") == "FirstRunComplete");
                        if (elem != null)
                        {
                            var valueElem = elem.Element("value");
                            if (valueElem != null && bool.TryParse(valueElem.Value, out bool firstRunResult2))
                                firstRunComplete = firstRunResult2;
                        }
                    }
                    catch { }
                }
                if (!firstRunComplete)
                {
                    // User cancelled first-run setup, exit launcher
                    return;
                }
            }
            // After first-run and auto-run prompt, always start the main app for normal operation
            // Prompt for auto-run if needed
            if (!SettingsReader.GetAutoRunPromptShown())
            {
                string launcherPath = Application.ExecutablePath;
                bool firstRun = !StartupManager.IsRunAtStartupEnabled() && !StartupManager.IsScheduledTaskEnabled();
                if (firstRun)
                {
                    DialogResult runAtLogin = DarkPromptForm.Show(
                        "Do you want Wi-Fi Watchdog to run automatically when you log in?\n\nYes = Run as administrator (no UAC prompt at login, requires admin now)\nNo = Run as normal user (UAC prompt will appear when Wi-Fi Watchdog starts)",
                        "Startup Option",
                        darkMode
                    );
                    if (runAtLogin == DialogResult.Yes)
                    {
                        StartupManager.SetScheduledTaskAtLogin(true);
                        // Remove registry entry if scheduled task is enabled
                        StartupManager.SetRunAtStartup(false);
                        if (!StartupManager.IsScheduledTaskEnabled())
                        {
                            DarkPromptForm.ShowInfo(
                                "Failed to create scheduled task. Please run the launcher as administrator.",
                                "Startup Error",
                                darkMode
                            );
                        }
                    }
                    else if (runAtLogin == DialogResult.No)
                    {
                        StartupManager.SetRunAtStartup(true);
                    }
                    // If user cancels or disables all, do nothing
                }
                // Mark prompt as shown regardless of user choice
                SettingsReader.SetAutoRunPromptShown(true);

                // Create desktop shortcut on first run (late binding, no COM reference needed)
                try
                {
                    string desktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                    string shortcutPath = Path.Combine(desktop, "Wi-Fi Watchdog.lnk");
                    string exePath = Application.ExecutablePath;
                    if (!System.IO.File.Exists(shortcutPath))
                    {
                        Type shellType = Type.GetTypeFromProgID("WScript.Shell");
                        object shell = Activator.CreateInstance(shellType);
                        object shortcut = shellType.InvokeMember("CreateShortcut", System.Reflection.BindingFlags.InvokeMethod, null, shell, new object[] { shortcutPath });
                        var shortcutType = shortcut.GetType();
                        shortcutType.InvokeMember("TargetPath", System.Reflection.BindingFlags.SetProperty, null, shortcut, new object[] { exePath });
                        shortcutType.InvokeMember("WorkingDirectory", System.Reflection.BindingFlags.SetProperty, null, shortcut, new object[] { System.IO.Path.GetDirectoryName(exePath) });
                        shortcutType.InvokeMember("WindowStyle", System.Reflection.BindingFlags.SetProperty, null, shortcut, new object[] { 1 });
                        shortcutType.InvokeMember("Description", System.Reflection.BindingFlags.SetProperty, null, shortcut, new object[] { "Wi-Fi Watchdog Launcher" });
                        shortcutType.InvokeMember("Save", System.Reflection.BindingFlags.InvokeMethod, null, shortcut, null);
                    }
                }
                catch { /* Ignore errors creating shortcut */ }
            }

            // Always start the main app for normal operation
            if (!File.Exists(appPath))
            {
                DarkPromptForm.ShowInfo($"Could not find WifiWatchdogApp.exe at {appPath}", "Error", darkMode);
                return;
            }
            var psi2 = new ProcessStartInfo(appPath)
            {
                UseShellExecute = true,
                Verb = "runas",
                WorkingDirectory = Path.GetDirectoryName(appPath)
            };
            try { Process.Start(psi2); } catch (Exception ex)
            {
                DarkPromptForm.ShowInfo(
                    "Failed to start WifiWatchdogApp.\n" + ex.Message,
                    "Wi-Fi Watchdog Error",
                    darkMode
                );
            }
            // Exit launcher after starting main app
            Environment.Exit(0);
        }

        // Returns true if system is in dark mode, false otherwise
        private static bool IsSystemInDarkMode()
        {
            try
            {
                using (var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize"))
                {
                    if (key != null)
                    {
                        object value = key.GetValue("AppsUseLightTheme");
                        if (value != null && value is int intValue)
                        {
                            return intValue == 0; // 0 = dark, 1 = light
                        }
                    }
                }
            }
            catch { }
            return false;
        }
    }
}
