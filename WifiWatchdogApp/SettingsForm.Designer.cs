namespace WifiWatchdogApp
{
    partial class SettingsForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ComboBox comboBoxSsids;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label labelPassword;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.GroupBox groupBoxAppearance;
        private System.Windows.Forms.CheckBox checkBoxDarkMode;
        private System.Windows.Forms.GroupBox groupBoxNotifications;
        private System.Windows.Forms.CheckBox checkBoxNotifyReconnect;
        private System.Windows.Forms.CheckBox checkBoxNotifySuccess;
        private System.Windows.Forms.CheckBox checkBoxNotifyFailure;
        private System.Windows.Forms.GroupBox groupBoxTiming;
        private System.Windows.Forms.Label labelCheckInterval;
        private System.Windows.Forms.NumericUpDown numericCheckInterval;
        private System.Windows.Forms.Label labelRecheckDelay;
        private System.Windows.Forms.NumericUpDown numericRecheckDelay;
        private System.Windows.Forms.Label labelFailCount;
        private System.Windows.Forms.NumericUpDown numericFailCount;
        private System.Windows.Forms.GroupBox groupBoxPing;
        private System.Windows.Forms.Label labelGatewayPing;
        private System.Windows.Forms.TextBox textBoxGatewayPing;
        private System.Windows.Forms.Label labelInternetPing;
        private System.Windows.Forms.TextBox textBoxInternetPing;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox groupBoxStartup;
        private System.Windows.Forms.RadioButton radioAutoRunNone;
        private System.Windows.Forms.RadioButton radioAutoRunRegistry;
        private System.Windows.Forms.RadioButton radioAutoRunTask;
        private System.Windows.Forms.Button btnResetFirstRun;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.comboBoxSsids = new System.Windows.Forms.ComboBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.labelPassword = new System.Windows.Forms.Label();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.groupBoxAppearance = new System.Windows.Forms.GroupBox();
            this.checkBoxDarkMode = new System.Windows.Forms.CheckBox();
            this.groupBoxNotifications = new System.Windows.Forms.GroupBox();
            this.checkBoxNotifyReconnect = new System.Windows.Forms.CheckBox();
            this.checkBoxNotifySuccess = new System.Windows.Forms.CheckBox();
            this.checkBoxNotifyFailure = new System.Windows.Forms.CheckBox();
            this.groupBoxTiming = new System.Windows.Forms.GroupBox();
            this.labelCheckInterval = new System.Windows.Forms.Label();
            this.numericCheckInterval = new System.Windows.Forms.NumericUpDown();
            this.labelRecheckDelay = new System.Windows.Forms.Label();
            this.numericRecheckDelay = new System.Windows.Forms.NumericUpDown();
            this.labelFailCount = new System.Windows.Forms.Label();
            this.numericFailCount = new System.Windows.Forms.NumericUpDown();
            this.groupBoxPing = new System.Windows.Forms.GroupBox();
            this.labelGatewayPing = new System.Windows.Forms.Label();
            this.textBoxGatewayPing = new System.Windows.Forms.TextBox();
            this.labelInternetPing = new System.Windows.Forms.Label();
            this.textBoxInternetPing = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip();
            this.groupBoxStartup = new System.Windows.Forms.GroupBox();
            this.radioAutoRunNone = new System.Windows.Forms.RadioButton();
            this.radioAutoRunRegistry = new System.Windows.Forms.RadioButton();
            this.radioAutoRunTask = new System.Windows.Forms.RadioButton();
            this.btnResetFirstRun = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // comboBoxSsids
            // 
            this.comboBoxSsids.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSsids.FormattingEnabled = true;
            this.comboBoxSsids.Location = new System.Drawing.Point(12, 12);
            this.comboBoxSsids.Name = "comboBoxSsids";
            this.comboBoxSsids.Size = new System.Drawing.Size(260, 21);
            this.comboBoxSsids.TabIndex = 0;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(116, 480);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 28);
            this.btnSave.TabIndex = 20;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(197, 480);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 28);
            this.btnCancel.TabIndex = 21;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize = true;
            this.labelPassword.Location = new System.Drawing.Point(12, 46);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(56, 13);
            this.labelPassword.TabIndex = 3;
            this.labelPassword.Text = "Password:";
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(74, 43);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.Size = new System.Drawing.Size(198, 20);
            this.textBoxPassword.TabIndex = 4;
            this.textBoxPassword.UseSystemPasswordChar = true;
            // 
            // groupBoxAppearance
            // 
            this.groupBoxAppearance.Controls.Add(this.checkBoxDarkMode);
            this.groupBoxAppearance.Location = new System.Drawing.Point(12, 160);
            this.groupBoxAppearance.Name = "groupBoxAppearance";
            this.groupBoxAppearance.Size = new System.Drawing.Size(260, 45);
            this.groupBoxAppearance.TabIndex = 10;
            this.groupBoxAppearance.TabStop = false;
            this.groupBoxAppearance.Text = "Appearance";
            // 
            // checkBoxDarkMode
            // 
            this.checkBoxDarkMode.AutoSize = true;
            this.checkBoxDarkMode.Location = new System.Drawing.Point(10, 19);
            this.checkBoxDarkMode.Name = "checkBoxDarkMode";
            this.checkBoxDarkMode.Size = new System.Drawing.Size(78, 17);
            this.checkBoxDarkMode.TabIndex = 0;
            this.checkBoxDarkMode.Text = "Dark Mode";
            this.toolTip1.SetToolTip(this.checkBoxDarkMode, "Enable dark mode for the app interface.");
            this.checkBoxDarkMode.CheckedChanged += new System.EventHandler(this.checkBoxDarkMode_CheckedChanged);
            // 
            // groupBoxNotifications
            // 
            this.groupBoxNotifications.Controls.Add(this.checkBoxNotifyReconnect);
            this.groupBoxNotifications.Controls.Add(this.checkBoxNotifySuccess);
            this.groupBoxNotifications.Controls.Add(this.checkBoxNotifyFailure);
            this.groupBoxNotifications.Location = new System.Drawing.Point(12, 210);
            this.groupBoxNotifications.Name = "groupBoxNotifications";
            this.groupBoxNotifications.Size = new System.Drawing.Size(260, 70);
            this.groupBoxNotifications.TabIndex = 11;
            this.groupBoxNotifications.TabStop = false;
            this.groupBoxNotifications.Text = "Notifications";
            // 
            // checkBoxNotifyReconnect
            // 
            this.checkBoxNotifyReconnect.AutoSize = true;
            this.checkBoxNotifyReconnect.Location = new System.Drawing.Point(10, 19);
            this.checkBoxNotifyReconnect.Name = "checkBoxNotifyReconnect";
            this.checkBoxNotifyReconnect.Size = new System.Drawing.Size(180, 17);
            this.checkBoxNotifyReconnect.TabIndex = 0;
            this.checkBoxNotifyReconnect.Text = "Show reconnect attempt alerts";
            this.toolTip1.SetToolTip(this.checkBoxNotifyReconnect, "Show a notification when a reconnect is attempted.");
            // 
            // checkBoxNotifySuccess
            // 
            this.checkBoxNotifySuccess.AutoSize = true;
            this.checkBoxNotifySuccess.Location = new System.Drawing.Point(10, 36);
            this.checkBoxNotifySuccess.Name = "checkBoxNotifySuccess";
            this.checkBoxNotifySuccess.Size = new System.Drawing.Size(200, 17);
            this.checkBoxNotifySuccess.TabIndex = 1;
            this.checkBoxNotifySuccess.Text = "Show successful reconnect notifications";
            this.toolTip1.SetToolTip(this.checkBoxNotifySuccess, "Show a notification when Wi-Fi is reconnected successfully.");
            // 
            // checkBoxNotifyFailure
            // 
            this.checkBoxNotifyFailure.AutoSize = true;
            this.checkBoxNotifyFailure.Location = new System.Drawing.Point(10, 53);
            this.checkBoxNotifyFailure.Name = "checkBoxNotifyFailure";
            this.checkBoxNotifyFailure.Size = new System.Drawing.Size(180, 17);
            this.checkBoxNotifyFailure.TabIndex = 2;
            this.checkBoxNotifyFailure.Text = "Show reconnect failure alerts";
            this.toolTip1.SetToolTip(this.checkBoxNotifyFailure, "Show a notification when a reconnect fails.");
            // 
            // groupBoxTiming
            // 
            this.groupBoxTiming.Controls.Add(this.labelCheckInterval);
            this.groupBoxTiming.Controls.Add(this.numericCheckInterval);
            this.groupBoxTiming.Controls.Add(this.labelRecheckDelay);
            this.groupBoxTiming.Controls.Add(this.numericRecheckDelay);
            this.groupBoxTiming.Controls.Add(this.labelFailCount);
            this.groupBoxTiming.Controls.Add(this.numericFailCount);
            this.groupBoxTiming.Location = new System.Drawing.Point(12, 285);
            this.groupBoxTiming.Name = "groupBoxTiming";
            this.groupBoxTiming.Size = new System.Drawing.Size(260, 100);
            this.groupBoxTiming.TabIndex = 12;
            this.groupBoxTiming.TabStop = false;
            this.groupBoxTiming.Text = "Timing Settings";
            // 
            // labelCheckInterval
            // 
            this.labelCheckInterval.AutoSize = true;
            this.labelCheckInterval.Location = new System.Drawing.Point(10, 22);
            this.labelCheckInterval.Name = "labelCheckInterval";
            this.labelCheckInterval.Size = new System.Drawing.Size(120, 13);
            this.labelCheckInterval.TabIndex = 0;
            this.labelCheckInterval.Text = "Main check interval (ms):";
            this.toolTip1.SetToolTip(this.labelCheckInterval, "How often to check Wi-Fi status.");
            // 
            // numericCheckInterval
            // 
            this.numericCheckInterval.Location = new System.Drawing.Point(170, 20);
            this.numericCheckInterval.Maximum = 60000;
            this.numericCheckInterval.Minimum = 500;
            this.numericCheckInterval.Increment = 100;
            this.numericCheckInterval.Name = "numericCheckInterval";
            this.numericCheckInterval.Size = new System.Drawing.Size(80, 20);
            this.numericCheckInterval.TabIndex = 1;
            // 
            // labelRecheckDelay
            // 
            this.labelRecheckDelay.AutoSize = true;
            this.labelRecheckDelay.Location = new System.Drawing.Point(10, 47);
            this.labelRecheckDelay.Name = "labelRecheckDelay";
            this.labelRecheckDelay.Size = new System.Drawing.Size(120, 13);
            this.labelRecheckDelay.TabIndex = 2;
            this.labelRecheckDelay.Text = "Recheck delay (ms):";
            this.toolTip1.SetToolTip(this.labelRecheckDelay, "How long to wait before a second SSID check.");
            // 
            // numericRecheckDelay
            // 
            this.numericRecheckDelay.Location = new System.Drawing.Point(170, 45);
            this.numericRecheckDelay.Maximum = 10000;
            this.numericRecheckDelay.Minimum = 500;
            this.numericRecheckDelay.Increment = 100;
            this.numericRecheckDelay.Name = "numericRecheckDelay";
            this.numericRecheckDelay.Size = new System.Drawing.Size(80, 20);
            this.numericRecheckDelay.TabIndex = 3;
            // 
            // labelFailCount
            // 
            this.labelFailCount.AutoSize = true;
            this.labelFailCount.Location = new System.Drawing.Point(10, 72);
            this.labelFailCount.Name = "labelFailCount";
            this.labelFailCount.Size = new System.Drawing.Size(154, 13);
            this.labelFailCount.TabIndex = 4;
            this.labelFailCount.Text = "Failed checks before reconnect:";
            this.toolTip1.SetToolTip(this.labelFailCount, "How many failed checks before attempting a reconnect.");
            // 
            // numericFailCount
            // 
            this.numericFailCount.Location = new System.Drawing.Point(170, 70);
            this.numericFailCount.Maximum = 10;
            this.numericFailCount.Minimum = 1;
            this.numericFailCount.Name = "numericFailCount";
            this.numericFailCount.Size = new System.Drawing.Size(80, 20);
            this.numericFailCount.TabIndex = 5;
            // 
            // groupBoxPing
            // 
            this.groupBoxPing.Controls.Add(this.labelGatewayPing);
            this.groupBoxPing.Controls.Add(this.textBoxGatewayPing);
            this.groupBoxPing.Controls.Add(this.labelInternetPing);
            this.groupBoxPing.Controls.Add(this.textBoxInternetPing);
            this.groupBoxPing.Location = new System.Drawing.Point(12, 390);
            this.groupBoxPing.Name = "groupBoxPing";
            this.groupBoxPing.Size = new System.Drawing.Size(260, 75);
            this.groupBoxPing.TabIndex = 13;
            this.groupBoxPing.TabStop = false;
            this.groupBoxPing.Text = "Ping Targets";
            // 
            // labelGatewayPing
            // 
            this.labelGatewayPing.AutoSize = true;
            this.labelGatewayPing.Location = new System.Drawing.Point(10, 22);
            this.labelGatewayPing.Name = "labelGatewayPing";
            this.labelGatewayPing.Size = new System.Drawing.Size(120, 13);
            this.labelGatewayPing.TabIndex = 0;
            this.labelGatewayPing.Text = "Gateway ping target (IP):";
            this.toolTip1.SetToolTip(this.labelGatewayPing, "Leave blank for auto-detect.");
            // 
            // textBoxGatewayPing
            // 
            this.textBoxGatewayPing.Location = new System.Drawing.Point(170, 19);
            this.textBoxGatewayPing.Name = "textBoxGatewayPing";
            this.textBoxGatewayPing.Size = new System.Drawing.Size(80, 20);
            this.textBoxGatewayPing.TabIndex = 1;
            // 
            // labelInternetPing
            // 
            this.labelInternetPing.AutoSize = true;
            this.labelInternetPing.Location = new System.Drawing.Point(10, 47);
            this.labelInternetPing.Name = "labelInternetPing";
            this.labelInternetPing.Size = new System.Drawing.Size(120, 13);
            this.labelInternetPing.TabIndex = 2;
            this.labelInternetPing.Text = "Internet ping target (host):";
            this.toolTip1.SetToolTip(this.labelInternetPing, "Host or IP to test internet connectivity.");
            // 
            // textBoxInternetPing
            // 
            this.textBoxInternetPing.Location = new System.Drawing.Point(170, 44);
            this.textBoxInternetPing.Name = "textBoxInternetPing";
            this.textBoxInternetPing.Size = new System.Drawing.Size(80, 20);
            this.textBoxInternetPing.TabIndex = 3;
            // 
            // groupBoxStartup
            // 
            this.groupBoxStartup.Controls.Add(this.radioAutoRunNone);
            this.groupBoxStartup.Controls.Add(this.radioAutoRunRegistry);
            this.groupBoxStartup.Controls.Add(this.radioAutoRunTask);
            this.groupBoxStartup.Location = new System.Drawing.Point(12, 70);
            this.groupBoxStartup.Name = "groupBoxStartup";
            this.groupBoxStartup.Size = new System.Drawing.Size(260, 80);
            this.groupBoxStartup.TabIndex = 6;
            this.groupBoxStartup.TabStop = false;
            this.groupBoxStartup.Text = "Startup Options";
            // 
            // radioAutoRunNone
            // 
            this.radioAutoRunNone.AutoSize = true;
            this.radioAutoRunNone.Location = new System.Drawing.Point(10, 20);
            this.radioAutoRunNone.Name = "radioAutoRunNone";
            this.radioAutoRunNone.Size = new System.Drawing.Size(51, 17);
            this.radioAutoRunNone.TabIndex = 0;
            this.radioAutoRunNone.TabStop = true;
            this.radioAutoRunNone.Text = "None";
            this.radioAutoRunNone.UseVisualStyleBackColor = true;
            // 
            // radioAutoRunRegistry
            // 
            this.radioAutoRunRegistry.AutoSize = true;
            this.radioAutoRunRegistry.Location = new System.Drawing.Point(10, 40);
            this.radioAutoRunRegistry.Name = "radioAutoRunRegistry";
            this.radioAutoRunRegistry.Size = new System.Drawing.Size(140, 17);
            this.radioAutoRunRegistry.TabIndex = 1;
            this.radioAutoRunRegistry.TabStop = true;
            this.radioAutoRunRegistry.Text = "Registry (user, UAC on run)";
            this.radioAutoRunRegistry.UseVisualStyleBackColor = true;
            // 
            // radioAutoRunTask
            // 
            this.radioAutoRunTask.AutoSize = true;
            this.radioAutoRunTask.Location = new System.Drawing.Point(10, 60);
            this.radioAutoRunTask.Name = "radioAutoRunTask";
            this.radioAutoRunTask.Size = new System.Drawing.Size(210, 17);
            this.radioAutoRunTask.TabIndex = 2;
            this.radioAutoRunTask.TabStop = true;
            this.radioAutoRunTask.Text = "Scheduled Task (admin, no UAC at login)";
            this.radioAutoRunTask.UseVisualStyleBackColor = true;
            // 
            // btnResetFirstRun
            // 
            this.btnResetFirstRun.Location = new System.Drawing.Point(12, 510);
            this.btnResetFirstRun.Name = "btnResetFirstRun";
            this.btnResetFirstRun.Size = new System.Drawing.Size(260, 28);
            this.btnResetFirstRun.TabIndex = 30;
            this.btnResetFirstRun.Text = "Reset First-Run State (Testing Only)";
            this.btnResetFirstRun.UseVisualStyleBackColor = true;
            this.btnResetFirstRun.Click += new System.EventHandler(this.btnResetFirstRun_Click);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 550);
            this.Controls.Add(this.btnResetFirstRun);
            this.Controls.Add(this.groupBoxPing);
            this.Controls.Add(this.groupBoxTiming);
            this.Controls.Add(this.groupBoxNotifications);
            this.Controls.Add(this.groupBoxAppearance);
            this.Controls.Add(this.groupBoxStartup);
            this.Controls.Add(this.textBoxPassword);
            this.Controls.Add(this.labelPassword);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.comboBoxSsids);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Wi-Fi Watchdog Settings";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
