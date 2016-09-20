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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblSens = new System.Windows.Forms.Label();
            this.lblTol = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.trkSens = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.trkTol = new System.Windows.Forms.TrackBar();
            this.lblIntensity = new System.Windows.Forms.Label();
            this.trkIntensity = new System.Windows.Forms.TrackBar();
            this.lblSpdUnit = new System.Windows.Forms.Label();
            this.cboSpdUnit = new System.Windows.Forms.ComboBox();
            this.consoleLabel = new System.Windows.Forms.Label();
            this.settingsLabel = new System.Windows.Forms.Label();
            this.chkTelem = new System.Windows.Forms.CheckBox();
            this.telemetryLabel = new System.Windows.Forms.Label();
            this.statusLabel = new System.Windows.Forms.Label();
            this.cboPorts = new System.Windows.Forms.ComboBox();
            this.btnDefault = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkSens)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkTol)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkIntensity)).BeginInit();
            this.SuspendLayout();
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(12, 12);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(116, 23);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // consoleTextBox
            // 
            this.consoleTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.consoleTextBox.Location = new System.Drawing.Point(0, 308);
            this.consoleTextBox.Multiline = true;
            this.consoleTextBox.Name = "consoleTextBox";
            this.consoleTextBox.ReadOnly = true;
            this.consoleTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.consoleTextBox.Size = new System.Drawing.Size(397, 99);
            this.consoleTextBox.TabIndex = 0;
            // 
            // telemTextBox
            // 
            this.telemTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.telemTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.telemTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.telemTextBox.Location = new System.Drawing.Point(0, 13);
            this.telemTextBox.Multiline = true;
            this.telemTextBox.Name = "telemTextBox";
            this.telemTextBox.ReadOnly = true;
            this.telemTextBox.Size = new System.Drawing.Size(406, 394);
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
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
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
            this.splitContainer1.Panel2.Controls.Add(this.chkTelem);
            this.splitContainer1.Panel2.Controls.Add(this.telemTextBox);
            this.splitContainer1.Panel2.Controls.Add(this.telemetryLabel);
            this.splitContainer1.Size = new System.Drawing.Size(807, 407);
            this.splitContainer1.SplitterDistance = 397;
            this.splitContainer1.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblSens);
            this.groupBox1.Controls.Add(this.lblTol);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.trkSens);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.trkTol);
            this.groupBox1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBox1.Location = new System.Drawing.Point(24, 76);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(297, 100);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Brake Vibe Settings";
            this.groupBox1.Visible = false;
            // 
            // lblSens
            // 
            this.lblSens.AutoSize = true;
            this.lblSens.Location = new System.Drawing.Point(15, 74);
            this.lblSens.Name = "lblSens";
            this.lblSens.Size = new System.Drawing.Size(19, 13);
            this.lblSens.TabIndex = 17;
            this.lblSens.Text = "(3)";
            // 
            // lblTol
            // 
            this.lblTol.AutoSize = true;
            this.lblTol.Location = new System.Drawing.Point(15, 37);
            this.lblTol.Name = "lblTol";
            this.lblTol.Size = new System.Drawing.Size(33, 13);
            this.lblTol.TabIndex = 16;
            this.lblTol.Text = "(30%)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "Sensitivity";
            // 
            // trkSens
            // 
            this.trkSens.AutoSize = false;
            this.trkSens.Location = new System.Drawing.Point(103, 57);
            this.trkSens.Name = "trkSens";
            this.trkSens.Size = new System.Drawing.Size(179, 37);
            this.trkSens.TabIndex = 14;
            this.trkSens.Tag = "";
            this.trkSens.Value = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Brake Tolerance";
            // 
            // trkTol
            // 
            this.trkTol.Location = new System.Drawing.Point(103, 19);
            this.trkTol.Maximum = 100;
            this.trkTol.Name = "trkTol";
            this.trkTol.Size = new System.Drawing.Size(179, 45);
            this.trkTol.TabIndex = 12;
            this.trkTol.Tag = "";
            this.trkTol.TickFrequency = 10;
            this.trkTol.Value = 30;
            // 
            // lblIntensity
            // 
            this.lblIntensity.AutoSize = true;
            this.lblIntensity.Location = new System.Drawing.Point(196, 30);
            this.lblIntensity.Name = "lblIntensity";
            this.lblIntensity.Size = new System.Drawing.Size(49, 13);
            this.lblIntensity.TabIndex = 6;
            this.lblIntensity.Text = "Intensity:";
            // 
            // trkIntensity
            // 
            this.trkIntensity.Location = new System.Drawing.Point(251, 25);
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
            this.consoleLabel.Location = new System.Drawing.Point(0, 295);
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
            // chkTelem
            // 
            this.chkTelem.AutoSize = true;
            this.chkTelem.Location = new System.Drawing.Point(388, 16);
            this.chkTelem.Name = "chkTelem";
            this.chkTelem.Size = new System.Drawing.Size(15, 14);
            this.chkTelem.TabIndex = 15;
            this.chkTelem.UseVisualStyleBackColor = true;
            this.chkTelem.CheckedChanged += new System.EventHandler(this.chkTelem_CheckedChanged);
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
            this.statusLabel.Location = new System.Drawing.Point(549, 17);
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
            this.cboPorts.SelectedIndexChanged += new System.EventHandler(this.cboPorts_SelectedIndexChanged);
            // 
            // btnDefault
            // 
            this.btnDefault.Location = new System.Drawing.Point(353, 11);
            this.btnDefault.Name = "btnDefault";
            this.btnDefault.Size = new System.Drawing.Size(71, 23);
            this.btnDefault.TabIndex = 6;
            this.btnDefault.Text = "Set Defualt";
            this.btnDefault.UseVisualStyleBackColor = true;
            this.btnDefault.Click += new System.EventHandler(this.btnDefault_Click);
            // 
            // iRacingSLI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(831, 461);
            this.Controls.Add(this.btnDefault);
            this.Controls.Add(this.cboPorts);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.startButton);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "iRacingSLI";
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
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkSens)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkTol)).EndInit();
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
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Label consoleLabel;
        private System.Windows.Forms.ComboBox cboPorts;
        private System.Windows.Forms.Label lblSpdUnit;
        private System.Windows.Forms.ComboBox cboSpdUnit;
        private System.Windows.Forms.Label lblIntensity;
        private System.Windows.Forms.TrackBar trkIntensity;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblSens;
        private System.Windows.Forms.Label lblTol;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TrackBar trkSens;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar trkTol;
        private System.Windows.Forms.CheckBox chkTelem;
        private System.Windows.Forms.Button btnDefault;
        private System.Windows.Forms.Label telemetryLabel;
    }

}

