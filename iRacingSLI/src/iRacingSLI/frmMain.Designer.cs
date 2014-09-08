namespace iRacingSLI
{
    partial class frmMain
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
            this.components = new System.ComponentModel.Container();
            this.tmr = new System.Windows.Forms.Timer(this.components);
            this.cmbSerial = new System.Windows.Forms.Button();
            this.cboPorts = new System.Windows.Forms.ComboBox();
            this.lblColor = new System.Windows.Forms.Label();
            this.lblConn = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.trkBoost = new System.Windows.Forms.TrackBar();
            this.label5 = new System.Windows.Forms.Label();
            this.trkLap = new System.Windows.Forms.TrackBar();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.chkPit = new System.Windows.Forms.CheckBox();
            this.trkFuel = new System.Windows.Forms.TrackBar();
            this.trkGear = new System.Windows.Forms.TrackBar();
            this.trkShift = new System.Windows.Forms.TrackBar();
            this.chkDebug = new System.Windows.Forms.CheckBox();
            this.chkSpeedUnits = new System.Windows.Forms.CheckBox();
            this.trkIntensity = new System.Windows.Forms.TrackBar();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.trkDelta = new System.Windows.Forms.TrackBar();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkBoost)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkLap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkFuel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkGear)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkShift)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkIntensity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkDelta)).BeginInit();
            this.SuspendLayout();
            // 
            // tmr
            // 
            this.tmr.Interval = 50;
            this.tmr.Tick += new System.EventHandler(this.tmr_Tick);
            // 
            // cmbSerial
            // 
            this.cmbSerial.Location = new System.Drawing.Point(11, 60);
            this.cmbSerial.Name = "cmbSerial";
            this.cmbSerial.Size = new System.Drawing.Size(192, 43);
            this.cmbSerial.TabIndex = 0;
            this.cmbSerial.Text = "Start serial port";
            this.cmbSerial.UseVisualStyleBackColor = true;
            this.cmbSerial.Click += new System.EventHandler(this.cmbSerial_Click);
            // 
            // cboPorts
            // 
            this.cboPorts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPorts.FormattingEnabled = true;
            this.cboPorts.Location = new System.Drawing.Point(11, 33);
            this.cboPorts.Name = "cboPorts";
            this.cboPorts.Size = new System.Drawing.Size(192, 21);
            this.cboPorts.TabIndex = 5;
            // 
            // lblColor
            // 
            this.lblColor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblColor.Location = new System.Drawing.Point(11, 110);
            this.lblColor.Name = "lblColor";
            this.lblColor.Size = new System.Drawing.Size(18, 19);
            this.lblColor.TabIndex = 6;
            // 
            // lblConn
            // 
            this.lblConn.AutoSize = true;
            this.lblConn.Location = new System.Drawing.Point(32, 113);
            this.lblConn.Name = "lblConn";
            this.lblConn.Size = new System.Drawing.Size(0, 13);
            this.lblConn.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(150, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Select your Arduino COM port:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.trkDelta);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.trkBoost);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.trkLap);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.chkPit);
            this.groupBox1.Controls.Add(this.trkFuel);
            this.groupBox1.Controls.Add(this.trkGear);
            this.groupBox1.Controls.Add(this.trkShift);
            this.groupBox1.Location = new System.Drawing.Point(11, 217);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(192, 372);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Debug mode";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 231);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "Boost";
            // 
            // trkBoost
            // 
            this.trkBoost.LargeChange = 2;
            this.trkBoost.Location = new System.Drawing.Point(15, 250);
            this.trkBoost.Maximum = 37;
            this.trkBoost.Name = "trkBoost";
            this.trkBoost.Size = new System.Drawing.Size(171, 45);
            this.trkBoost.TabIndex = 14;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 177);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Lap Number";
            // 
            // trkLap
            // 
            this.trkLap.Location = new System.Drawing.Point(15, 199);
            this.trkLap.Maximum = 200;
            this.trkLap.Name = "trkLap";
            this.trkLap.Size = new System.Drawing.Size(171, 45);
            this.trkLap.SmallChange = 3;
            this.trkLap.TabIndex = 12;
            this.trkLap.TickFrequency = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 126);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Fuel Left";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Current Gear";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "RPM Lights";
            // 
            // chkPit
            // 
            this.chkPit.AutoSize = true;
            this.chkPit.Location = new System.Drawing.Point(12, 347);
            this.chkPit.Name = "chkPit";
            this.chkPit.Size = new System.Drawing.Size(67, 17);
            this.chkPit.TabIndex = 8;
            this.chkPit.Text = "Pit limiter";
            this.chkPit.UseVisualStyleBackColor = true;
            // 
            // trkFuel
            // 
            this.trkFuel.Location = new System.Drawing.Point(15, 145);
            this.trkFuel.Maximum = 100;
            this.trkFuel.Name = "trkFuel";
            this.trkFuel.Size = new System.Drawing.Size(171, 45);
            this.trkFuel.SmallChange = 3;
            this.trkFuel.TabIndex = 7;
            this.trkFuel.TickFrequency = 5;
            // 
            // trkGear
            // 
            this.trkGear.Location = new System.Drawing.Point(12, 94);
            this.trkGear.Maximum = 7;
            this.trkGear.Minimum = -1;
            this.trkGear.Name = "trkGear";
            this.trkGear.Size = new System.Drawing.Size(174, 45);
            this.trkGear.TabIndex = 6;
            // 
            // trkShift
            // 
            this.trkShift.Location = new System.Drawing.Point(15, 43);
            this.trkShift.Maximum = 100;
            this.trkShift.Name = "trkShift";
            this.trkShift.Size = new System.Drawing.Size(171, 45);
            this.trkShift.SmallChange = 5;
            this.trkShift.TabIndex = 5;
            this.trkShift.TickFrequency = 10;
            // 
            // chkDebug
            // 
            this.chkDebug.AutoSize = true;
            this.chkDebug.Enabled = false;
            this.chkDebug.Location = new System.Drawing.Point(11, 191);
            this.chkDebug.Name = "chkDebug";
            this.chkDebug.Size = new System.Drawing.Size(87, 17);
            this.chkDebug.TabIndex = 10;
            this.chkDebug.Text = "Debug mode";
            this.chkDebug.UseVisualStyleBackColor = true;
            this.chkDebug.CheckedChanged += new System.EventHandler(this.chkDebug_CheckedChanged);
            // 
            // chkSpeedUnits
            // 
            this.chkSpeedUnits.AutoSize = true;
            this.chkSpeedUnits.Enabled = true;
            this.chkSpeedUnits.Location = new System.Drawing.Point(116, 191);
            this.chkSpeedUnits.Name = "chkSpeedUnits";
            this.chkSpeedUnits.Size = new System.Drawing.Size(93, 17);
            this.chkSpeedUnits.TabIndex = 11;
            this.chkSpeedUnits.Text = "Speed in KPH";
            this.chkSpeedUnits.UseVisualStyleBackColor = true;
            // 
            // trkIntensity
            // 
            this.trkIntensity.LargeChange = 1;
            this.trkIntensity.Location = new System.Drawing.Point(11, 155);
            this.trkIntensity.Maximum = 7;
            this.trkIntensity.Name = "trkIntensity";
            this.trkIntensity.Size = new System.Drawing.Size(192, 45);
            this.trkIntensity.TabIndex = 12;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(20, 139);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(46, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "Intensity";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 282);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(32, 13);
            this.label8.TabIndex = 17;
            this.label8.Text = "Delta";
            // 
            // trkDelta
            // 
            this.trkDelta.AllowDrop = true;
            this.trkDelta.LargeChange = 10;
            this.trkDelta.Location = new System.Drawing.Point(12, 301);
            this.trkDelta.Maximum = 999;
            this.trkDelta.Minimum = -999;
            this.trkDelta.Name = "trkDelta";
            this.trkDelta.Size = new System.Drawing.Size(162, 45);
            this.trkDelta.TabIndex = 16;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(215, 217);
            this.Controls.Add(this.chkSpeedUnits);
            this.Controls.Add(this.chkDebug);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.trkIntensity);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblConn);
            this.Controls.Add(this.lblColor);
            this.Controls.Add(this.cboPorts);
            this.Controls.Add(this.cmbSerial);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.Text = "iRacing DX SLI";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkBoost)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkLap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkFuel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkGear)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkShift)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkIntensity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkDelta)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer tmr;
        private System.Windows.Forms.Button cmbSerial;
        private System.Windows.Forms.ComboBox cboPorts;
        private System.Windows.Forms.Label lblColor;
        private System.Windows.Forms.Label lblConn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkPit;
        private System.Windows.Forms.TrackBar trkFuel;
        private System.Windows.Forms.TrackBar trkGear;
        private System.Windows.Forms.TrackBar trkShift;
        private System.Windows.Forms.CheckBox chkDebug;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TrackBar trkLap;
        private System.Windows.Forms.TrackBar trkBoost;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chkSpeedUnits;
        private System.Windows.Forms.TrackBar trkIntensity;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TrackBar trkDelta;
    }
}

