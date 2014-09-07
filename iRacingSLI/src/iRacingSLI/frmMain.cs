using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using iRSDKSharp;

namespace iRacingSLI {
    public partial class frmMain : Form {

        static SerialPort SP;
        iRacingSDK sdk = new iRacingSDK();

        int Gear, Lap, startPort;
        double Speed, RPM, Fuel, Shift;
        short iRPM, iFuel, iShift, iSpeed;
        byte Engine;
        byte[] serialdata = new byte[12];
        byte[] shiftlights = new byte[16];
        String Boost;
        Boolean unitKPH;

        System.Random randnum = new System.Random();

        public frmMain(string[] args)
        {
            InitializeComponent();

            startPort = 0;
            this.unitKPH = false;
            chkSpeedUnits.Enabled = true;

            for (int i = 0; i < 6; i = i + 2)
                if (args.Length > i+1 && args[i] != null)
                {
                    if (args[i] == "--Port")
                    {
                        this.processPortArgs(args[i+1]);
                    }
                    if (args[i] == "--Unit")
                    {
                        this.processUnitArgs(args[i + 1]);
                    }
                    if (args[i] == "--Intensity")
                    {
                        trkIntensity.Value = Convert.ToInt16(args[i + 1]);
                    }
                }
        }

        private void tmr_Tick(object sender, EventArgs e) {
            if (chkDebug.Checked == true) {
                lblConn.Text = "Debug mode active!";
                lblColor.BackColor = Color.FromArgb(255, 129, 0);

                Gear = trkGear.Value;
                Speed = randnum.Next(0, 255);
                RPM = randnum.Next(4253, 17954);
                Fuel = trkFuel.Value;
                Shift = trkShift.Value;
                Lap = trkLap.Value;
                Boost = Convert.ToString(trkBoost.Value);

                if (Boost.Length < 2)
                    Boost = String.Concat("0", Boost);

                if (chkPit.Checked == true)
                    Engine = 0x10;
                else
                    Engine = 0x00;

                
                iSpeed = Convert.ToInt16(Speed);
                iRPM = Convert.ToInt16(RPM);
                iFuel = Convert.ToByte(Math.Round(Fuel));
                iShift = Convert.ToByte(Math.Round((Shift * 16) / 100));
                if (Lap > 199) { Lap = 199; }

                serialdata[0] = 255;
                serialdata[1] = Convert.ToByte((trkIntensity.Value << 4) | 0);
                serialdata[2] = Convert.ToByte(Gear + 1);
                serialdata[3] = Convert.ToByte((iSpeed >> 8) & 0x00FF);
                serialdata[4] = Convert.ToByte(iSpeed & 0x00FF);
                serialdata[5] = Convert.ToByte((iRPM >> 8) & 0x00FF);
                serialdata[6] = Convert.ToByte(iRPM & 0x00FF);
                serialdata[7] = Convert.ToByte(iFuel);
                serialdata[8] = Convert.ToByte(iShift);
                serialdata[9] = Engine;
                serialdata[10] = Convert.ToByte(Lap);
                serialdata[11] = Convert.ToByte(Boost);

                //lblConn.Text = "s1: " + serialdata[1] + " : " + (serialdata[1] >> 4);
                SP.Write(serialdata, 0, 12);

            } else {
                if (sdk.IsConnected()) {
                    lblConn.Text = "Connected to iRacing API";
                    lblColor.BackColor = Color.FromArgb(0, 200, 0);

                    if (this.unitKPH) {
                        Speed = Convert.ToDouble(sdk.GetData("Speed")) * (2.23693629 * 1.609344); //KPH
                    } else {
                        Speed = Convert.ToDouble(sdk.GetData("Speed")) * 2.23693629; //MPH
                    }

                    Gear = Convert.ToInt32(sdk.GetData("Gear"));
                    RPM = Convert.ToDouble(sdk.GetData("RPM"));
                    Fuel = Convert.ToDouble(sdk.GetData("FuelLevelPct"));
                    Shift = Convert.ToDouble(sdk.GetData("ShiftIndicatorPct"));
                    Engine = Convert.ToByte(sdk.GetData("EngineWarnings"));
                    Lap = Convert.ToInt32(sdk.GetData("Lap"));

                    this.Text = Shift.ToString();
                    iSpeed = Convert.ToInt16(Speed);
                    iRPM = Convert.ToInt16(RPM);
                    iFuel = Convert.ToByte(Math.Round(Fuel * 100));
                    iShift = Convert.ToByte(Math.Round((Shift * 100 * 16) / 100));
                    if (Lap > 199) { Lap = 199; }

                    serialdata[0] = 255;
                    serialdata[1] = Convert.ToByte((trkIntensity.Value << 4) | 0);
                    serialdata[2] = Convert.ToByte(Gear + 1);
                    serialdata[3] = Convert.ToByte((iSpeed >> 8) & 0x00FF);
                    serialdata[4] = Convert.ToByte(iSpeed & 0x00FF);
                    serialdata[5] = Convert.ToByte((iRPM >> 8) & 0x00FF);
                    serialdata[6] = Convert.ToByte(iRPM & 0x00FF);
                    serialdata[7] = Convert.ToByte(iFuel);
                    serialdata[8] = Convert.ToByte(iShift);
                    serialdata[9] = Engine;
                    serialdata[10] = Convert.ToByte(Lap);
                    serialdata[11] = 0;

                    SP.Write(serialdata, 0, 12);
                } else if (sdk.IsInitialized) {
                    lblConn.Text = "No connection with iRacing API";
                    lblColor.BackColor = Color.FromArgb(200, 0, 0);

                    sdk.Shutdown();
                } else {
                    lblConn.Text = "No connection with iRacing API";
                    lblColor.BackColor = Color.FromArgb(200, 0, 0);

                    sdk.Startup();
                }
            }
        }

        private void frmMain_Load(object sender, EventArgs e) {
            String[] ports = SerialPort.GetPortNames();
            cboPorts.Items.AddRange(ports);
            cboPorts.SelectedIndex = startPort;

            lblConn.Text = "No connection with iRacing API";
        }

        private void cmbSerial_Click(object sender, EventArgs e) {
            if (cmbSerial.Text == "Start serial port") {
                SP = new SerialPort(cboPorts.Text, 9600, Parity.None, 8);
                SP.Open();
                tmr.Enabled = true;
                cmbSerial.Text = "Stop serial port";
                chkDebug.Enabled = true;
            } else {
                SP.Close();
                tmr.Enabled = false;
                cmbSerial.Text = "Start serial port";
                chkDebug.Enabled = false;
            }
        }

        private void chkDebug_CheckedChanged(object sender, EventArgs e) {
            if (chkDebug.Checked == true) {
                this.Height = 590;
            } else {
                this.Height = 245;
            }

        }

        private void chkSpeedUnits_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSpeedUnits.Checked == true)
            {
                this.unitKPH = true;
            }
            else
            {
                this.unitKPH = false;
            }

        }


        private void processPortArgs(String port)
        {
            if (SerialPort.GetPortNames().Contains(port))
            {
                SP = new SerialPort(port, 9600, Parity.None, 8);
                SP.Open();
                tmr.Enabled = true;
                cmbSerial.Text = "Stop serial port";
                chkDebug.Enabled = true;

                for (int i = 0; i < SerialPort.GetPortNames().Length; i++)
                {
                    if (SerialPort.GetPortNames()[i].Equals(port))
                    {
                        startPort = i;
                    }
                }
            }
        }

        private void processUnitArgs(String unit)
        {
            if (unit == "MPH")
            {
                this.unitKPH = false;
                this.chkSpeedUnits.Checked = false;
            }
            else if (unit == "KPH")
            {
                this.unitKPH = true;
                this.chkSpeedUnits.Checked = true;
            }
            else if (unit == "KMH")
            {
                this.unitKPH = true;
                this.chkSpeedUnits.Checked = true;
            }
        }
    }
}
