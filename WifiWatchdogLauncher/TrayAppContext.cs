using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace WifiWatchdogLauncher
{
    public class TrayAppContext : ApplicationContext
    {
        private NotifyIcon trayIcon;
        private bool darkMode;

        public TrayAppContext(bool darkMode)
        {
            this.darkMode = darkMode;
            trayIcon = new NotifyIcon()
            {
                Icon = System.Drawing.SystemIcons.Shield,
                Visible = true,
                Text = "Wi-Fi Watchdog Launcher"
            };
            var menu = new ContextMenuStrip();
            var uninstallItem = new ToolStripMenuItem("Uninstall");
            uninstallItem.Click += OnUninstall;
            menu.Items.Add(uninstallItem);
            menu.Items.Add(new ToolStripMenuItem("Exit", null, (s, e) => Application.Exit()));
            trayIcon.ContextMenuStrip = menu;
        }

        private void OnUninstall(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Are you sure you want to uninstall Wi-Fi Watchdog and remove all settings?",
                "Uninstall Confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );
            if (result == DialogResult.Yes)
            {
                try { StartupManager.SetRunAtStartup(false); } catch { }
                try { StartupManager.SetScheduledTaskAtLogin(false); } catch { }
                try
                {
                    string desktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                    string shortcutPath = Path.Combine(desktop, "Wi-Fi Watchdog.lnk");
                    if (File.Exists(shortcutPath)) File.Delete(shortcutPath);
                }
                catch { }
                try
                {
                    string appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                    string configDir = Path.Combine(appData, "WifiWatchdogApp");
                    if (Directory.Exists(configDir)) Directory.Delete(configDir, true);
                }
                catch { }
                try
                {
                    string exePath = Application.ExecutablePath;
                    trayIcon.Visible = false;
                    MessageBox.Show("Uninstall complete. You can now delete the EXE file.", "Uninstall", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Application.Exit();
                    // Optionally, schedule self-delete on next reboot
                }
                catch { }
            }
        }
    }
}
