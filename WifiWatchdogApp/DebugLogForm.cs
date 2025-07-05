using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace WifiWatchdogApp
{
    public class DebugLogForm : Form
    {
        private readonly DebugLog debugLog;
        private ListBox listBox;
        private Button btnExport;
        private Button btnClear;
        private Button btnClose;
        private SaveFileDialog saveFileDialog;
        private bool darkMode;
        private Timer refreshTimer;
        private Panel titleBar;
        private Label titleLabel;
        private Button closeBtn;
        private CheckBox checkBoxAutoScroll;
        private Panel bottomPanel;
        private FlowLayoutPanel buttonFlowPanel;
        private bool autoScroll = true;

        public DebugLogForm(DebugLog log, bool darkMode)
        {
            this.debugLog = log;
            this.darkMode = darkMode;
            this.Icon = SystemIcons.Shield;
            InitializeComponent();
            LoadLog();
            ApplyTheme();
            refreshTimer = new Timer();
            refreshTimer.Interval = 2000; // 2 seconds
            refreshTimer.Tick += (s, e) => LoadLog();
            refreshTimer.Start();
            this.FormClosing += (s, e) => refreshTimer.Stop();
        }

        private void InitializeComponent()
        {
            this.Text = "Wi-Fi Watchdog Debug Log";
            this.Size = new Size(700, 500);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Custom title bar
            titleBar = new Panel() { Height = 32, Dock = DockStyle.Top, BackColor = ThemeManager.TitleBar };
            titleLabel = new Label() { Text = this.Text, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft, ForeColor = ThemeManager.TitleBarText, Font = new Font("Segoe UI", 10, FontStyle.Bold), Padding = new Padding(10, 0, 0, 0) };
            closeBtn = new Button() { Text = "✕", Dock = DockStyle.Right, Width = 40, FlatStyle = FlatStyle.Flat, BackColor = ThemeManager.TitleBar, ForeColor = ThemeManager.TitleBarText };
            closeBtn.FlatAppearance.BorderSize = 0;
            closeBtn.Click += (s, e) => this.Close();
            titleBar.Controls.Add(titleLabel);
            titleBar.Controls.Add(closeBtn);
            this.Controls.Add(titleBar);
            titleBar.MouseDown += (s, e) => { if (e.Button == MouseButtons.Left) { ReleaseCapture(); SendMessage(this.Handle, 0xA1, 0x2, 0); } };

            // ListBox for log
            listBox = new ListBox()
            {
                Dock = DockStyle.Fill,
                Font = new Font("Consolas", 10),
                HorizontalScrollbar = true
            };

            // Bottom panel for buttons and checkbox
            bottomPanel = new Panel() { Dock = DockStyle.Bottom, Height = 54, BackColor = ThemeManager.Background };
            buttonFlowPanel = new FlowLayoutPanel() {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(10, 10, 10, 10),
                BackColor = ThemeManager.Background,
                AutoSize = true
            };
            bottomPanel.Controls.Add(buttonFlowPanel);

            // Auto-scroll checkbox
            checkBoxAutoScroll = new CheckBox()
            {
                Text = "Auto-scroll to latest",
                Checked = true,
                AutoSize = true,
                ForeColor = ThemeManager.Foreground,
                BackColor = ThemeManager.Background,
                Margin = new Padding(0, 0, 20, 0)
            };
            checkBoxAutoScroll.CheckedChanged += (s, e) => autoScroll = checkBoxAutoScroll.Checked;
            buttonFlowPanel.Controls.Add(checkBoxAutoScroll);

            // Export, Clear, Close buttons
            btnExport = new Button() { Text = "Export", Width = 90, Height = 28, Margin = new Padding(0, 0, 10, 0) };
            btnClear = new Button() { Text = "Clear", Width = 90, Height = 28, Margin = new Padding(0, 0, 10, 0) };
            btnClose = new Button() { Text = "Close", Width = 90, Height = 28 };
            buttonFlowPanel.Controls.Add(btnExport);
            buttonFlowPanel.Controls.Add(btnClear);
            buttonFlowPanel.Controls.Add(btnClose);

            // Initialize saveFileDialog
            saveFileDialog = new SaveFileDialog()
            {
                Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                Title = "Export Debug Log"
            };

            // Add controls in correct order
            this.Controls.Add(listBox);
            this.Controls.Add(bottomPanel);
            this.Controls.Add(titleBar);

            btnExport.Click += (s, e) => ExportLog();
            btnClear.Click += (s, e) => { debugLog.Clear(); LoadLog(); };
            btnClose.Click += (s, e) => this.Close();

            this.Resize += (s, e) => ResizeControls();
        }

        private void ResizeControls()
        {
            if (listBox != null)
            {
                listBox.Width = this.ClientSize.Width;
                listBox.Height = this.ClientSize.Height - 90;
            }
            if (btnExport != null)
            {
                btnExport.Top = this.ClientSize.Height - 48;
            }
            if (btnClear != null)
            {
                btnClear.Top = this.ClientSize.Height - 48;
            }
            if (btnClose != null)
            {
                btnClose.Left = this.ClientSize.Width - 90;
                btnClose.Top = this.ClientSize.Height - 48;
            }
        }

        private void LoadLog()
        {
            var entries = debugLog.GetEntries();
            if (listBox == null) return;
            listBox.BeginUpdate();
            listBox.Items.Clear();
            foreach (var entry in entries)
                listBox.Items.Add(entry);
            listBox.EndUpdate();
            if (autoScroll && listBox.Items.Count > 0)
                listBox.TopIndex = listBox.Items.Count - 1;
        }

        private void ExportLog()
        {
            if (saveFileDialog == null)
            {
                MessageBox.Show("Save dialog is not available.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (debugLog == null)
            {
                MessageBox.Show("Debug log is not available.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    debugLog.Export(saveFileDialog.FileName);
                    MessageBox.Show("Log exported.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to export log: {ex.Message}", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ApplyTheme()
        {
            ThemeManager.ApplyDark(this);
        }

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
    }
}
