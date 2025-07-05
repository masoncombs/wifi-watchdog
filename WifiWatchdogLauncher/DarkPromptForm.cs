using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;

namespace WifiWatchdogLauncher
{
    public class DarkPromptForm : Form
    {
        public DialogResult Result { get; private set; } = DialogResult.None;
        private Button btnYes;
        private Button btnNo;
        private Label lblMessage;
        private Panel titleBar;
        private Label titleLabel;
        private Button closeBtn;
        private Timer countdownTimer;
        private int secondsLeft = 10;

        public DarkPromptForm(string message, string title, bool darkMode)
        {
            this.Text = title;
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ClientSize = new Size(360, 150); // Increased height for more space
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowInTaskbar = false;
            this.Font = new Font("Segoe UI", 10);
            this.Icon = SystemIcons.Shield;

            // Custom title bar
            titleBar = new Panel() { Height = 32, Dock = DockStyle.Top, BackColor = Color.FromArgb(30, 30, 30) };
            titleLabel = new Label() { Text = title, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft, ForeColor = Color.FromArgb(204, 204, 204), Font = new Font("Segoe UI", 10, FontStyle.Bold), Padding = new Padding(10, 0, 0, 0) };
            closeBtn = new Button() { Text = "✕", Dock = DockStyle.Right, Width = 40, FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(30, 30, 30), ForeColor = Color.FromArgb(204, 204, 204) };
            closeBtn.FlatAppearance.BorderSize = 0;
            closeBtn.Click += (s, e) => this.Close();
            titleBar.Controls.Add(titleLabel);
            titleBar.Controls.Add(closeBtn);
            this.Controls.Add(titleBar);
            titleBar.MouseDown += (s, e) => { if (e.Button == MouseButtons.Left) { ReleaseCapture(); SendMessage(this.Handle, 0xA1, 0x2, 0); } };

            // Message label
            lblMessage = new Label()
            {
                Text = message,
                AutoSize = false,
                Dock = DockStyle.Top,
                Height = 60,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.FromArgb(204, 204, 204),
                Padding = new Padding(10, 10, 10, 10)
            };
            this.Controls.Add(lblMessage);
            lblMessage.BringToFront();

            // Buttons panel
            var buttonPanel = new Panel() { Dock = DockStyle.Bottom, Height = 50, Padding = new Padding(0, 10, 0, 10) };
            btnYes = new Button() { Text = "Yes", DialogResult = DialogResult.Yes, Width = 90, Height = 32, Left = 60, Top = 8 };
            btnNo = new Button() { Text = "No", DialogResult = DialogResult.No, Width = 90, Height = 32, Left = 200, Top = 8 };
            btnYes.Anchor = btnNo.Anchor = AnchorStyles.None;
            buttonPanel.Controls.Add(btnYes);
            buttonPanel.Controls.Add(btnNo);
            this.Controls.Add(buttonPanel);
            buttonPanel.BringToFront();

            btnYes.Text = $"Yes ({secondsLeft})";
            countdownTimer = new Timer();
            countdownTimer.Interval = 1000;
            countdownTimer.Tick += (s, e) => {
                secondsLeft--;
                btnYes.Text = $"Yes ({secondsLeft})";
                if (secondsLeft <= 0)
                {
                    countdownTimer.Stop();
                    this.Result = DialogResult.Yes;
                    this.Close();
                }
            };
            countdownTimer.Start();
            this.FormClosing += (s, e) => countdownTimer.Stop();
            btnYes.Click += (s, e) => { countdownTimer.Stop(); this.Result = DialogResult.Yes; this.Close(); };
            btnNo.Click += (s, e) => { countdownTimer.Stop(); this.Result = DialogResult.No; this.Close(); };

            if (darkMode)
            {
                this.BackColor = Color.FromArgb(37, 37, 38);
                btnYes.BackColor = btnNo.BackColor = Color.FromArgb(51, 51, 51);
                btnYes.ForeColor = btnNo.ForeColor = Color.FromArgb(204, 204, 204);
                btnYes.FlatStyle = btnNo.FlatStyle = FlatStyle.Flat;
                btnYes.FlatAppearance.BorderColor = btnNo.FlatAppearance.BorderColor = Color.FromArgb(51, 51, 51);
                btnYes.FlatAppearance.BorderSize = btnNo.FlatAppearance.BorderSize = 1;
            }
        }

        public static DialogResult Show(string message, string title, bool darkMode)
        {
            using (var form = new DarkPromptForm(message, title, darkMode))
            {
                form.ShowDialog();
                return form.Result;
            }
        }

        public static void ShowInfo(string message, string title, bool darkMode)
        {
            using (var form = new DarkPromptForm(message, title, darkMode))
            {
                form.btnNo.Visible = false;
                form.btnYes.Text = "OK";
                form.btnYes.Location = new Point((form.ClientSize.Width - form.btnYes.Width) / 2, form.btnYes.Location.Y);
                form.ShowDialog();
            }
        }

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
    }
}
