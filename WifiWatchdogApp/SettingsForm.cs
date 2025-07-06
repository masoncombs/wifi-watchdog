using System;
using System.Windows.Forms;
using System.Linq;
using System.Diagnostics;
using System.Drawing;

namespace WifiWatchdogApp
{
    public partial class SettingsForm : Form
    {
        private System.Windows.Forms.Timer refreshTimer;

        public string SelectedSsid { get; private set; }
        public string WifiPassword { get; private set; }
        public bool RunAtStartup { get; private set; }
        
        public bool DarkMode { get; private set; }
        public bool NotifyReconnect { get; private set; }
        public bool NotifySuccess { get; private set; }
        public bool NotifyFailure { get; private set; }
        public int MainCheckInterval { get; private set; }
        public int RecheckDelay { get; private set; }
        public int FailCount { get; private set; }
        public string GatewayPingTarget { get; private set; }
        public string InternetPingTarget { get; private set; }

        public SettingsForm(string currentSsid, string currentPassword = "")
        {
            InitializeComponent();
            this.Icon = SystemIcons.Shield;
            SelectedSsid = currentSsid;
            WifiPassword = currentPassword;
            // Use system theme if no user setting exists
            bool? darkModeSetting = null;
            try { darkModeSetting = Properties.Settings.Default.Properties["DarkMode"] != null ? (bool?)Properties.Settings.Default.DarkMode : null; } catch { }
            if (darkModeSetting == null)
                DarkMode = IsSystemInDarkMode();
            else
                DarkMode = darkModeSetting.Value;
            checkBoxDarkMode.Checked = DarkMode;
            LoadWifiNetworks();
            textBoxPassword.Text = WifiPassword;

            // Set startup radio buttons based on current state
            bool reg = StartupManager.IsRunAtStartupEnabled();
            bool task = false;
            try { task = IsScheduledTaskEnabled(); } catch { }
            if (task)
                radioAutoRunTask.Checked = true;
            else if (reg)
                radioAutoRunRegistry.Checked = true;
            else
                radioAutoRunNone.Checked = true;

            // Load settings (replace with persistent storage if desired)
            NotifyReconnect = Properties.Settings.Default.NotifyReconnect;
            NotifySuccess = Properties.Settings.Default.NotifySuccess;
            NotifyFailure = Properties.Settings.Default.NotifyFailure;
            MainCheckInterval = Properties.Settings.Default.MainCheckInterval;
            RecheckDelay = Properties.Settings.Default.RecheckDelay;
            FailCount = Properties.Settings.Default.FailCount;
            GatewayPingTarget = Properties.Settings.Default.GatewayPingTarget;
            InternetPingTarget = Properties.Settings.Default.InternetPingTarget;

            checkBoxNotifyReconnect.Checked = NotifyReconnect;
            checkBoxNotifySuccess.Checked = NotifySuccess;
            checkBoxNotifyFailure.Checked = NotifyFailure;
            numericCheckInterval.Value = MainCheckInterval > 0 ? MainCheckInterval : 1500;
            numericRecheckDelay.Value = RecheckDelay > 0 ? RecheckDelay : 1500;
            numericFailCount.Value = FailCount > 0 ? FailCount : 2;
            textBoxGatewayPing.Text = GatewayPingTarget ?? "";
            textBoxInternetPing.Text = InternetPingTarget ?? "www.google.com";

            ApplyTheme();
            refreshTimer = new System.Windows.Forms.Timer();
            refreshTimer.Interval = 3000; // 3 seconds
            refreshTimer.Tick += (s, e) => LoadWifiNetworks();
            this.Shown += (s, e) => refreshTimer.Start();
            this.FormClosing += (s, e) => refreshTimer.Stop();
        }

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

        private void ApplyTheme()
        {
            if (checkBoxDarkMode.Checked)
            {
                this.BackColor = System.Drawing.Color.FromArgb(32, 32, 32);
                this.ForeColor = System.Drawing.Color.White;
                // Optional: set border style for a more consistent dark look
                // this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
                foreach (Control c in this.Controls)
                {
                    c.BackColor = this.BackColor;
                    c.ForeColor = this.ForeColor;
                    if (c is GroupBox gb)
                    {
                        foreach (Control gc in gb.Controls)
                        {
                            gc.BackColor = this.BackColor;
                            gc.ForeColor = this.ForeColor;
                        }
                    }
                }
            }
            else
            {
                this.BackColor = System.Drawing.SystemColors.Control;
                this.ForeColor = System.Drawing.SystemColors.ControlText;
                // this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
                foreach (Control c in this.Controls)
                {
                    c.BackColor = this.BackColor;
                    c.ForeColor = this.ForeColor;
                    if (c is GroupBox gb)
                    {
                        foreach (Control gc in gb.Controls)
                        {
                            gc.BackColor = this.BackColor;
                            gc.ForeColor = this.ForeColor;
                        }
                    }
                }
            }
            // Note: The window title bar and borders are controlled by Windows and cannot be themed with WinForms alone.
        }

        private void LoadWifiNetworks()
        {
            var currentSelection = comboBoxSsids.SelectedItem?.ToString();
            var userHasSelected = !string.IsNullOrEmpty(currentSelection);
            var ssids = GetAvailableSsids();
            comboBoxSsids.Items.Clear();
            foreach (var ssid in ssids)
                comboBoxSsids.Items.Add(ssid);
            // If user has selected an SSID, keep it selected even if not in the refreshed list
            if (userHasSelected)
            {
                if (!comboBoxSsids.Items.Contains(currentSelection))
                    comboBoxSsids.Items.Add(currentSelection); // Add missing SSID back for selection
                comboBoxSsids.SelectedItem = currentSelection;
            }
            else if (comboBoxSsids.Items.Contains(SelectedSsid))
            {
                comboBoxSsids.SelectedItem = SelectedSsid;
            }
        }

        private string[] GetAvailableSsids()
        {
            try
            {
                var psi = new ProcessStartInfo("netsh", "wlan show networks")
                {
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                using (var proc = Process.Start(psi))
                {
                    string output = proc.StandardOutput.ReadToEnd();
                    proc.WaitForExit();
                    var lines = output.Split('\n');
                    return lines.Where(l => l.Trim().StartsWith("SSID "))
                        .Select(l => l.Split(':')[1].Trim())
                        .Where(ssid => !string.IsNullOrWhiteSpace(ssid))
                        .Distinct()
                        .ToArray();
                }
            }
            catch { return new string[0]; }
        }

        private bool IsScheduledTaskEnabled()
        {
            try
            {
                var psi = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "schtasks",
                    Arguments = "/Query /TN \"WiFi Watchdog\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                using (var proc = System.Diagnostics.Process.Start(psi))
                {
                    string output = proc.StandardOutput.ReadToEnd();
                    proc.WaitForExit();
                    return output.Contains("WiFi Watchdog");
                }
            }
            catch { return false; }
        }

        private void RemoveScheduledTask()
        {
            try
            {
                var psi = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "schtasks",
                    Arguments = "/Delete /TN \"WiFi Watchdog\" /F",
                    Verb = "runas",
                    UseShellExecute = true,
                    CreateNoWindow = true
                };
                System.Diagnostics.Process.Start(psi)?.WaitForExit();
            }
            catch { }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Always get the current values from the controls
            SelectedSsid = comboBoxSsids.SelectedItem?.ToString() ?? SelectedSsid;
            WifiPassword = textBoxPassword.Text;
            DarkMode = checkBoxDarkMode.Checked;
            NotifyReconnect = checkBoxNotifyReconnect.Checked;
            NotifySuccess = checkBoxNotifySuccess.Checked;
            NotifyFailure = checkBoxNotifyFailure.Checked;
            MainCheckInterval = (int)numericCheckInterval.Value;
            RecheckDelay = (int)numericRecheckDelay.Value;
            FailCount = (int)numericFailCount.Value;
            GatewayPingTarget = textBoxGatewayPing.Text.Trim();
            InternetPingTarget = textBoxInternetPing.Text.Trim();
            // Save settings
            Properties.Settings.Default.DarkMode = DarkMode;
            Properties.Settings.Default.NotifyReconnect = NotifyReconnect;
            Properties.Settings.Default.NotifySuccess = NotifySuccess;
            Properties.Settings.Default.NotifyFailure = NotifyFailure;
            Properties.Settings.Default.MainCheckInterval = MainCheckInterval;
            Properties.Settings.Default.RecheckDelay = RecheckDelay;
            Properties.Settings.Default.FailCount = FailCount;
            Properties.Settings.Default.GatewayPingTarget = GatewayPingTarget;
            Properties.Settings.Default.InternetPingTarget = InternetPingTarget;
            Properties.Settings.Default.Save();
            // Handle startup options
            if (radioAutoRunNone.Checked)
            {
                StartupManager.SetRunAtStartup(false);
                RemoveScheduledTask();
            }
            else if (radioAutoRunRegistry.Checked)
            {
                StartupManager.SetRunAtStartup(true);
                RemoveScheduledTask();
            }
            else if (radioAutoRunTask.Checked)
            {
                StartupManager.SetRunAtStartup(false);
                // Create scheduled task
                try { CreateScheduledTask(); } catch { }
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private void CreateScheduledTask()
        {
            var psi = new System.Diagnostics.ProcessStartInfo
            {
                FileName = "schtasks",
                Arguments = $"/Create /TN \"WiFi Watchdog\" /TR \"\"{System.Windows.Forms.Application.ExecutablePath}\"\" /SC ONLOGON /RL HIGHEST /F",
                Verb = "runas",
                UseShellExecute = true,
                CreateNoWindow = true
            };
            System.Diagnostics.Process.Start(psi)?.WaitForExit();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void checkBoxDarkMode_CheckedChanged(object sender, EventArgs e)
        {
            ApplyTheme();
        }

        private void btnResetFirstRun_Click(object sender, EventArgs e)
        {
            // Remove registry and scheduled task, and reset AutoRunPromptShown
            try { StartupManager.SetRunAtStartup(false); } catch { }
            try { RemoveScheduledTask(); } catch { }
            try
            {
                // Reset AutoRunPromptShown and clear protected Wi-Fi settings
                Properties.Settings.Default["AutoRunPromptShown"] = false;
                Properties.Settings.Default["ProtectedSsid"] = string.Empty;
                Properties.Settings.Default["ProtectedWifiPassword"] = string.Empty;
                Properties.Settings.Default.Save();
            }
            catch { }
            MessageBox.Show("First-run state has been reset. The launcher will prompt for Wi-Fi selection next time.", "Reset Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
