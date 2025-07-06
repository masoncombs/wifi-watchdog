using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Timers;
using System.ComponentModel;
using System.Security.Principal;
using System.Net.NetworkInformation;
using System.Linq;
using System.IO;

namespace WifiWatchdogApp
{
    public class TrayAppContext : ApplicationContext
    {
        private NotifyIcon trayIcon;
        private System.Timers.Timer checkTimer;
        private string ssid; // No default, will be set from settings or first-run
        private string wifiPassword;
        private int ssidFailCount = 0;
        private bool runAtStartup = false;
        private string launcherPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WifiWatchdogLauncher.exe");
        private string adapter = "Wi-Fi"; // Default adapter name, can be made user-configurable

        private bool notifyReconnect = true;
        private bool notifySuccess = true;
        private bool notifyFailure = true;
        private int mainCheckInterval = 1500;
        private int failCount = 2;
        private string gatewayPingTarget = "";
        private string internetPingTarget = "www.google.com";

        private DebugLog debugLog;

        public TrayAppContext(bool firstRunOnly = false)
        {
            debugLog = new DebugLog(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WifiWatchdog_DebugLog.txt"));

            // First-run Wi-Fi selection logic
            ssid = Properties.Settings.Default.ProtectedSsid;
            wifiPassword = Properties.Settings.Default.ProtectedWifiPassword;
            if (!Properties.Settings.Default.FirstRunComplete || string.IsNullOrWhiteSpace(ssid))
            {
                // Show new FirstRunForm for first-run Wi-Fi selection
                string[] availableSSIDs = WifiUtils.GetAvailableSSIDs();
                using (var form = new FirstRunForm(availableSSIDs))
                {
                    form.Text = "First Time Wi-Fi Setup";
                    form.StartPosition = FormStartPosition.CenterScreen;
                    form.ShowInTaskbar = true;
                    if (form.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(form.SelectedSSID))
                    {
                        ssid = form.SelectedSSID;
                        wifiPassword = form.WifiPassword;
                        Properties.Settings.Default.ProtectedSsid = ssid;
                        Properties.Settings.Default.ProtectedWifiPassword = wifiPassword;
                        Properties.Settings.Default.FirstRunComplete = true;
                        Properties.Settings.Default.Save();
                        debugLog.Log($"First-run: User selected SSID '{ssid}' for protection.");
                    }
                    else
                    {
                        // User cancelled, exit app
                        Application.Exit();
                        return;
                    }
                }
                if (firstRunOnly)
                {
                    // After first run, exit so launcher can continue
                    Application.Exit();
                    return;
                }
            }

            // Load user settings
            notifyReconnect = Properties.Settings.Default.NotifyReconnect;
            notifySuccess = Properties.Settings.Default.NotifySuccess;
            notifyFailure = Properties.Settings.Default.NotifyFailure;
            mainCheckInterval = Properties.Settings.Default.MainCheckInterval > 0 ? Properties.Settings.Default.MainCheckInterval : 1500;
            failCount = Properties.Settings.Default.FailCount > 0 ? Properties.Settings.Default.FailCount : 2;
            gatewayPingTarget = Properties.Settings.Default.GatewayPingTarget ?? "";
            internetPingTarget = Properties.Settings.Default.InternetPingTarget ?? "www.google.com";

            trayIcon = new NotifyIcon()
            {
                Icon = SystemIcons.Shield, // Use the Windows shield icon for admin indication
                Visible = true,
                Text = "Wi-Fi Watchdog"
            };
            trayIcon.ContextMenuStrip = new ContextMenuStrip();
            // Build tray menu in correct order: Settings, Debug Log, separator, Uninstall, Exit (Exit last)
            var settingsItem = new ToolStripMenuItem("Settings");
            settingsItem.Click += OnSettings;

            var debugLogItem = new ToolStripMenuItem("Debug Log");
            debugLogItem.Click += OnDebugLog;

            // Add Uninstall option to tray menu
            var uninstallItem = new ToolStripMenuItem("Uninstall");
            uninstallItem.Click += OnUninstall;

            trayIcon.ContextMenuStrip.Items.Clear();
            trayIcon.ContextMenuStrip.Items.Add(settingsItem);
            trayIcon.ContextMenuStrip.Items.Add(debugLogItem);
            trayIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
            trayIcon.ContextMenuStrip.Items.Add(uninstallItem);
            trayIcon.ContextMenuStrip.Items.Add(new ToolStripMenuItem("Exit", null, OnExit));

            trayIcon.DoubleClick += (s, e) => ShowStatus();

            checkTimer = new System.Timers.Timer(mainCheckInterval);
            checkTimer.Elapsed += CheckWifiStatus;
            checkTimer.Start();

            // Initial check
            CheckWifiStatus(null, null);

            debugLog.Log($"App started. Monitoring SSID: {ssid}");

            // Apply VS Code dark mode to context menu
            if (IsDarkMode())
            {
                trayIcon.ContextMenuStrip.Renderer = new ToolStripProfessionalRenderer(new DarkMenuColorTable());
                trayIcon.ContextMenuStrip.ForeColor = Color.FromArgb(204, 204, 204);
                trayIcon.ContextMenuStrip.BackColor = Color.FromArgb(37, 37, 38);
            }
        }

        private void ShowStatus()
        {
            trayIcon.ShowBalloonTip(3000, "Wi-Fi Watchdog", "Monitoring Wi-Fi connection...", ToolTipIcon.Info);
        }

        private void CheckWifiStatus(object sender, ElapsedEventArgs e)
        {
            checkTimer.Stop();
            string currentSsid = GetCurrentSsid();
            if (!string.Equals(currentSsid, ssid, StringComparison.OrdinalIgnoreCase))
            {
                // If SSID is not as expected, check connectivity
                if (PingHost(internetPingTarget))
                {
                    debugLog.Log($"Ping to {internetPingTarget} succeeded.");
                    ssidFailCount = 0;
                }
                else
                {
                    debugLog.Log($"Ping to {internetPingTarget} failed.");
                    ssidFailCount++;
                    System.Threading.Thread.Sleep(mainCheckInterval); // Wait before rechecking
                    string recheckSsid = GetCurrentSsid();
                    if (!string.Equals(recheckSsid, ssid, StringComparison.OrdinalIgnoreCase))
                    {
                        if (ssidFailCount >= failCount)
                        {
                            if (notifyReconnect)
                            {
                                trayIcon.ShowBalloonTip(3000, "Wi-Fi Disconnected", "Attempting to reconnect...", ToolTipIcon.Warning);
                                debugLog.Log("Wi-Fi disconnected. Attempting to reconnect.");
                            }
                            // PlayBeep(); // Commented out: SystemSounds is not available without System.Media
                            TryReconnect();
                            debugLog.Log("Reconnect attempt triggered.");
                            ssidFailCount = 0;
                        }
                    }
                    else
                    {
                        ssidFailCount = 0;
                    }
                }
            }
            else
            {
                ssidFailCount = 0;
            }
            checkTimer.Interval = mainCheckInterval;
            checkTimer.Start();
        }

        private void TryReconnect()
        {
            debugLog.Log("TryReconnect called.");
            // Disable adapter
            RunNetsh($"interface set interface name=\"{adapter}\" admin=disable");
            System.Threading.Thread.Sleep(4000);
            // Enable adapter
            RunNetsh($"interface set interface name=\"{adapter}\" admin=enable");
            // Connect to SSID with password if provided
            if (!string.IsNullOrWhiteSpace(wifiPassword))
            {
                // Add or update Wi-Fi profile with password
                CreateOrUpdateWifiProfile(ssid, wifiPassword);
                RunNetsh($"wlan connect name=\"{ssid}\" ssid=\"{ssid}\"");
            }
            else
            {
                RunNetsh($"wlan connect name=\"{ssid}\" ssid=\"{ssid}\"");
            }
            System.Threading.Thread.Sleep(4000);

            // Ping gateway (auto-detect or user setting)
            string gatewayIp = !string.IsNullOrWhiteSpace(gatewayPingTarget) ? gatewayPingTarget : GetGatewayForAdapter(adapter);
            bool pingRouter = false;
            if (!string.IsNullOrEmpty(gatewayIp))
            {
                pingRouter = PingHost(gatewayIp);
                debugLog.Log($"Ping to gateway {gatewayIp}: {(pingRouter ? "Success" : "Fail")}");
            }
            bool pingInternet = PingHost(internetPingTarget);
            debugLog.Log($"Ping to internet target {internetPingTarget}: {(pingInternet ? "Success" : "Fail")}");
            if (pingRouter && pingInternet)
            {
                if (notifySuccess)
                {
                    trayIcon.ShowBalloonTip(3000, "Wi-Fi Reconnected", $"Connected to {ssid}", ToolTipIcon.Info);
                    debugLog.Log("Wi-Fi reconnected successfully.");
                }
                // PlayBeep();
            }
            else
            {
                if (notifyFailure)
                {
                    trayIcon.ShowBalloonTip(3000, "Wi-Fi Reconnect Failed", "Will retry in 10 seconds...", ToolTipIcon.Error);
                    debugLog.Log("Wi-Fi reconnect failed.");
                }
                // PlayBeep();
            }
        }

        private string GetCurrentSsid()
        {
            try
            {
                var psi = new ProcessStartInfo("netsh", "wlan show interfaces")
                {
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                using (var proc = Process.Start(psi))
                {
                    string output = proc.StandardOutput.ReadToEnd();
                    proc.WaitForExit();
                    foreach (var line in output.Split('\n'))
                    {
                        if (line.Trim().StartsWith("SSID", StringComparison.OrdinalIgnoreCase) && !line.Contains("BSSID"))
                        {
                            var parts = line.Split(':');
                            if (parts.Length > 1)
                                return parts[1].Trim();
                        }
                    }
                }
            }
            catch { }
            return string.Empty;
        }

        private string GetGatewayForAdapter(string adapterName)
        {
            foreach (var ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 &&
                    ni.Name.Equals(adapterName, StringComparison.OrdinalIgnoreCase) &&
                    ni.OperationalStatus == OperationalStatus.Up)
                {
                    var gateway = ni.GetIPProperties().GatewayAddresses
                        .FirstOrDefault(g => g.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
                    if (gateway != null)
                        return gateway.Address.ToString();
                }
            }
            return null;
        }

        private void RunNetsh(string args)
        {
            var psi = new ProcessStartInfo("netsh", args)
            {
                UseShellExecute = false,
                CreateNoWindow = true
            };
            using (var proc = Process.Start(psi))
            {
                proc.WaitForExit();
            }
        }

        private bool IsAdministrator()
        {
            var wi = WindowsIdentity.GetCurrent();
            var wp = new WindowsPrincipal(wi);
            return wp.IsInRole(WindowsBuiltInRole.Administrator);
        }

        private void OnExit(object sender, EventArgs e)
        {
            trayIcon.Visible = false;
            Application.Exit();
        }

        private void OnSettings(object sender, EventArgs e)
        {
            using (var form = new SettingsForm(ssid, wifiPassword))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    ssid = form.SelectedSsid;
                    wifiPassword = form.WifiPassword;
                    runAtStartup = form.RunAtStartup;
                    // Reload all settings after save
                    notifyReconnect = Properties.Settings.Default.NotifyReconnect;
                    notifySuccess = Properties.Settings.Default.NotifySuccess;
                    notifyFailure = Properties.Settings.Default.NotifyFailure;
                    mainCheckInterval = Properties.Settings.Default.MainCheckInterval > 0 ? Properties.Settings.Default.MainCheckInterval : 1500;
                    failCount = Properties.Settings.Default.FailCount > 0 ? Properties.Settings.Default.FailCount : 2;
                    gatewayPingTarget = Properties.Settings.Default.GatewayPingTarget ?? "";
                    internetPingTarget = Properties.Settings.Default.InternetPingTarget ?? "www.google.com";
                    checkTimer.Interval = mainCheckInterval;
                    trayIcon.ShowBalloonTip(2000, "Wi-Fi Watchdog", $"Now monitoring: {ssid}", ToolTipIcon.Info);
                }
            }
        }

        private void OnDebugLog(object sender, EventArgs e)
        {
            bool darkMode = false;
            try
            {
                var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize");
                if (key != null)
                {
                    object value = key.GetValue("AppsUseLightTheme");
                    if (value != null && value is int intValue)
                        darkMode = intValue == 0;
                }
            }
            catch { }
            using (var form = new DebugLogForm(debugLog, darkMode))
            {
                form.ShowDialog();
            }
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
                try
                {
                    // Remove scheduled task if possible
                    var psi = new ProcessStartInfo
                    {
                        FileName = "schtasks",
                        Arguments = "/Delete /TN \"WiFi Watchdog\" /F",
                        Verb = "runas",
                        UseShellExecute = true,
                        CreateNoWindow = true
                    };
                    Process.Start(psi)?.WaitForExit();
                } catch { }
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
                    trayIcon.Visible = false;
                    MessageBox.Show("Uninstall complete. You can now delete the EXE files.", "Uninstall", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Application.Exit();
                }
                catch { }
            }
        }

        private bool PingHost(string host)
        {
            try
            {
                using (var ping = new System.Net.NetworkInformation.Ping())
                {
                    var reply = ping.Send(host, 2000);
                    return reply.Status == System.Net.NetworkInformation.IPStatus.Success;
                }
            }
            catch { return false; }
        }

        private void CreateOrUpdateWifiProfile(string ssid, string password)
        {
            string profileXml = $@"<?xml version=""1.0""?>
<WLANProfile xmlns=""http://www.microsoft.com/networking/WLAN/profile/v1"">
    <name>{ssid}</name>
    <SSIDConfig>
        <SSID>
            <name>{ssid}</name>
        </SSID>
    </SSIDConfig>
    <connectionType>ESS</connectionType>
    <connectionMode>auto</connectionMode>
    <MSM>
        <security>
            <authEncryption>
                <authentication>WPA2PSK</authentication>
                <encryption>AES</encryption>
                <useOneX>false</useOneX>
            </authEncryption>
            <sharedKey>
                <keyType>passPhrase</keyType>
                <protected>false</protected>
                <keyMaterial>{password}</keyMaterial>
            </sharedKey>
        </security>
    </MSM>
</WLANProfile>";
            string tempProfile = System.IO.Path.Combine(System.IO.Path.GetTempPath(), $"wifi_{ssid}.xml");
            System.IO.File.WriteAllText(tempProfile, profileXml);
            RunNetsh($"wlan add profile filename=\"{tempProfile}\" user=all");
        }

        private bool IsDarkMode()
        {
            try
            {
                var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\\Microsoft\\Windows\\CurrentVersion\\Themes\\Personalize");
                if (key != null)
                {
                    object value = key.GetValue("AppsUseLightTheme");
                    if (value != null && value is int intValue)
                        return intValue == 0;
                }
            }
            catch { }
            return false;
        }
    }

    public class DarkMenuColorTable : ProfessionalColorTable
    {
        public override Color MenuItemSelected => Color.FromArgb(60, 60, 60);
        public override Color MenuItemBorder => Color.FromArgb(0, 122, 204);
        public override Color ToolStripDropDownBackground => Color.FromArgb(37, 37, 38);
        public override Color ImageMarginGradientBegin => Color.FromArgb(37, 37, 38);
        public override Color ImageMarginGradientMiddle => Color.FromArgb(37, 37, 38);
        public override Color ImageMarginGradientEnd => Color.FromArgb(37, 37, 38);
        public override Color MenuBorder => Color.FromArgb(51, 51, 51);
        public override Color MenuItemPressedGradientBegin => Color.FromArgb(60, 60, 60);
        public override Color MenuItemPressedGradientEnd => Color.FromArgb(60, 60, 60);
        public override Color MenuItemSelectedGradientBegin => Color.FromArgb(60, 60, 60);
        public override Color MenuItemSelectedGradientEnd => Color.FromArgb(60, 60, 60);
    }
}
