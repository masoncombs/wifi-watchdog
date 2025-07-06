using System;
using System.Linq;
using System.Windows.Forms;

namespace WifiWatchdogApp
{
    public partial class FirstRunForm : Form
    {
        public string SelectedSSID => comboBoxSSID.SelectedItem?.ToString();
        public string WifiPassword => textBoxPassword.Text;

        private Timer ssidRefreshTimer;
        private string[] lastSsids;

        public FirstRunForm(string[] availableSSIDs)
        {
            InitializeComponent();
            comboBoxSSID.Items.AddRange(availableSSIDs);
            if (comboBoxSSID.Items.Count > 0)
                comboBoxSSID.SelectedIndex = 0;
            comboBoxSSID.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxSSID.PreviewKeyDown += ComboBoxSSID_PreviewKeyDown;
            textBoxPassword.KeyDown += TextBoxPassword_KeyDown;
            // Do NOT set this.AcceptButton = buttonSave;
            // Start SSID refresh timer
            lastSsids = availableSSIDs;
            ssidRefreshTimer = new Timer();
            ssidRefreshTimer.Interval = 4000; // 4 seconds
            ssidRefreshTimer.Tick += SsidRefreshTimer_Tick;
            ssidRefreshTimer.Start();
        }

        private void SsidRefreshTimer_Tick(object sender, EventArgs e)
        {
            string[] newSsids = WifiUtils.GetAvailableSSIDs();
            if (!newSsids.SequenceEqual(lastSsids))
            {
                int prevIndex = comboBoxSSID.SelectedIndex;
                string prevSsid = comboBoxSSID.SelectedItem?.ToString();
                comboBoxSSID.Items.Clear();
                comboBoxSSID.Items.AddRange(newSsids);
                lastSsids = newSsids;
                // Try to restore previous selection
                if (!string.IsNullOrEmpty(prevSsid) && comboBoxSSID.Items.Contains(prevSsid))
                    comboBoxSSID.SelectedItem = prevSsid;
                else if (comboBoxSSID.Items.Count > 0)
                    comboBoxSSID.SelectedIndex = 0;
            }
        }

        private void ComboBoxSSID_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!comboBoxSSID.DroppedDown)
                {
                    comboBoxSSID.DroppedDown = true;
                    e.IsInputKey = true;
                }
                // Suppress AcceptButton/save
            }
        }

        private void TextBoxPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonSave.PerformClick();
                e.Handled = true;
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SelectedSSID) || string.IsNullOrWhiteSpace(WifiPassword))
            {
                MessageBox.Show("Please select a Wi-Fi network and enter the password.", "Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (ssidRefreshTimer != null)
            {
                ssidRefreshTimer.Stop();
                ssidRefreshTimer.Dispose();
            }
            base.OnFormClosed(e);
        }
    }
}
