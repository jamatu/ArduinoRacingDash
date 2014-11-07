namespace iRacingSLI
{
    partial class iRacingSLI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.startButton = new System.Windows.Forms.Button();
            this.consoleTextBox = new System.Windows.Forms.TextBox();
            this.telemTextBox = new System.Windows.Forms.TextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lblIntensity = new System.Windows.Forms.Label();
            this.trkIntensity = new System.Windows.Forms.TrackBar();
            this.lblSpdUnit = new System.Windows.Forms.Label();
            this.cboSpdUnit = new System.Windows.Forms.ComboBox();
            this.consoleLabel = new System.Windows.Forms.Label();
            this.settingsLabel = new System.Windows.Forms.Label();
            this.telemetryLabel = new System.Windows.Forms.Label();
            this.statusLabel = new System.Windows.Forms.Label();
            this.cboPorts = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkIntensity)).BeginInit();
            this.SuspendLayout();
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(12, 12);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(107, 23);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // consoleTextBox
            // 
            this.consoleTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.consoleTextBox.Location = new System.Drawing.Point(0, 188);
            this.consoleTextBox.Multiline = true;
            this.consoleTextBox.Name = "consoleTextBox";
            this.consoleTextBox.ReadOnly = true;
            this.consoleTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.consoleTextBox.Size = new System.Drawing.Size(397, 65);
            this.consoleTextBox.TabIndex = 0;
            // 
            // telemTextBox
            // 
            this.telemTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.telemTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.telemTextBox.Location = new System.Drawing.Point(0, 13);
            this.telemTextBox.Multiline = true;
            this.telemTextBox.Name = "telemTextBox";
            this.telemTextBox.ReadOnly = true;
            this.telemTextBox.Size = new System.Drawing.Size(406, 240);
            this.telemTextBox.TabIndex = 2;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(12, 41);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lblIntensity);
            this.splitContainer1.Panel1.Controls.Add(this.trkIntensity);
            this.splitContainer1.Panel1.Controls.Add(this.lblSpdUnit);
            this.splitContainer1.Panel1.Controls.Add(this.cboSpdUnit);
            this.splitContainer1.Panel1.Controls.Add(this.consoleLabel);
            this.splitContainer1.Panel1.Controls.Add(this.consoleTextBox);
            this.splitContainer1.Panel1.Controls.Add(this.settingsLabel);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.telemTextBox);
            this.splitContainer1.Panel2.Controls.Add(this.telemetryLabel);
            this.splitContainer1.Size = new System.Drawing.Size(807, 253);
            this.splitContainer1.SplitterDistance = 397;
            this.splitContainer1.TabIndex = 3;
            // 
            // lblIntensity
            // 
            this.lblIntensity.AutoSize = true;
            this.lblIntensity.Location = new System.Drawing.Point(32, 66);
            this.lblIntensity.Name = "lblIntensity";
            this.lblIntensity.Size = new System.Drawing.Size(49, 13);
            this.lblIntensity.TabIndex = 6;
            this.lblIntensity.Text = "Intensity:";
            // 
            // trkIntensity
            // 
            this.trkIntensity.Location = new System.Drawing.Point(87, 61);
            this.trkIntensity.Maximum = 7;
            this.trkIntensity.Name = "trkIntensity";
            this.trkIntensity.Size = new System.Drawing.Size(104, 45);
            this.trkIntensity.TabIndex = 5;
            this.trkIntensity.ValueChanged += new System.EventHandler(this.trkIntensity_ValueChanged);
            // 
            // lblSpdUnit
            // 
            this.lblSpdUnit.AutoSize = true;
            this.lblSpdUnit.Location = new System.Drawing.Point(30, 28);
            this.lblSpdUnit.Name = "lblSpdUnit";
            this.lblSpdUnit.Size = new System.Drawing.Size(60, 13);
            this.lblSpdUnit.TabIndex = 4;
            this.lblSpdUnit.Text = "SpeedUnit:";
            // 
            // cboSpdUnit
            // 
            this.cboSpdUnit.FormattingEnabled = true;
            this.cboSpdUnit.Items.AddRange(new object[] {
            "MPH",
            "KPH"});
            this.cboSpdUnit.Location = new System.Drawing.Point(96, 25);
            this.cboSpdUnit.Name = "cboSpdUnit";
            this.cboSpdUnit.Size = new System.Drawing.Size(59, 21);
            this.cboSpdUnit.TabIndex = 3;
            this.cboSpdUnit.SelectedIndexChanged += new System.EventHandler(this.cboSpdUnit_SelectedIndexChanged);
            // 
            // consoleLabel
            // 
            this.consoleLabel.AutoSize = true;
            this.consoleLabel.Location = new System.Drawing.Point(0, 172);
            this.consoleLabel.Name = "consoleLabel";
            this.consoleLabel.Size = new System.Drawing.Size(45, 13);
            this.consoleLabel.TabIndex = 2;
            this.consoleLabel.Text = "Console";
            // 
            // settingsLabel
            // 
            this.settingsLabel.AutoSize = true;
            this.settingsLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.settingsLabel.Location = new System.Drawing.Point(0, 0);
            this.settingsLabel.Name = "settingsLabel";
            this.settingsLabel.Size = new System.Drawing.Size(45, 13);
            this.settingsLabel.TabIndex = 1;
            this.settingsLabel.Text = "Settings";
            // 
            // telemetryLabel
            // 
            this.telemetryLabel.AutoSize = true;
            this.telemetryLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.telemetryLabel.Location = new System.Drawing.Point(0, 0);
            this.telemetryLabel.Name = "telemetryLabel";
            this.telemetryLabel.Size = new System.Drawing.Size(53, 13);
            this.telemetryLabel.TabIndex = 3;
            this.telemetryLabel.Text = "Telemetry";
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Location = new System.Drawing.Point(500, 17);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(40, 13);
            this.statusLabel.TabIndex = 4;
            this.statusLabel.Text = "Status:";
            // 
            // cboPorts
            // 
            this.cboPorts.FormattingEnabled = true;
            this.cboPorts.Location = new System.Drawing.Point(147, 13);
            this.cboPorts.Name = "cboPorts";
            this.cboPorts.Size = new System.Drawing.Size(200, 21);
            this.cboPorts.TabIndex = 5;
            // 
            // iRacingSLI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(831, 307);
            this.Controls.Add(this.cboPorts);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.startButton);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "iRacingSLI";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "iRacing SLI";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trkIntensity)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.TextBox consoleTextBox;
        private System.Windows.Forms.TextBox telemTextBox;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label settingsLabel;
        private System.Windows.Forms.Label telemetryLabel;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Label consoleLabel;
        private System.Windows.Forms.ComboBox cboPorts;
        private System.Windows.Forms.Label lblSpdUnit;
        private System.Windows.Forms.ComboBox cboSpdUnit;
        private System.Windows.Forms.Label lblIntensity;
        private System.Windows.Forms.TrackBar trkIntensity;
    }

}

