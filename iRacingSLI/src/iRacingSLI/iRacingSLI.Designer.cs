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
            this.consoleLabel = new System.Windows.Forms.Label();
            this.settingsLabel = new System.Windows.Forms.Label();
            this.telemetryLabel = new System.Windows.Forms.Label();
            this.statusLabel = new System.Windows.Forms.Label();
            this.cboPorts = new System.Windows.Forms.ComboBox();
            this.closeButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
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
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(712, 12);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(107, 23);
            this.closeButton.TabIndex = 6;
            this.closeButton.Text = "Close App";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // iRacingSLI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(831, 307);
            this.ControlBox = false;
            this.Controls.Add(this.closeButton);
            this.Controls.Add(this.cboPorts);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.startButton);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "iRacingSLI";
            this.Text = "iRacing SLI";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
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
        private System.Windows.Forms.Button closeButton;
    }

}

