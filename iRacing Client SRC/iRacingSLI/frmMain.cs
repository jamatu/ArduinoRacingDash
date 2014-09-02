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
        short iRPM, iFuel, iShift;
        byte Engine;
        byte[] serialdata = new byte[11];
        byte[] shiftlights = new byte[16];
        String Boost;

        System.Random randnum = new System.Random();

        public frmMain(string[] args)
        {
            InitializeComponent();

            startPort = 0;

            if (args.Length > 0 && args[0] != null){
                if (SerialPort.GetPortNames().Contains(args[0])){
                    SP = new SerialPort(args[0], 9600, Parity.None, 8);
                    SP.Open();
                    tmr.Enabled = true;
                    cmbSerial.Text = "Stop serial port";
                    chkDebug.Enabled = true;

                    for (int i = 0; i < SerialPort.GetPortNames().Length; i++){
                        if (SerialPort.GetPortNames()[i].Equals(args[0])){
                            startPort = i;
                        }
                    }
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

                iRPM = Convert.ToInt16(RPM);
                iFuel = Convert.ToByte(Math.Round(Fuel));
                iShift = Convert.ToByte(Math.Round((Shift * 16) / 100));

                serialdata[0] = 255;
                serialdata[1] = Convert.ToByte(Gear + 1);
                serialdata[2] = Convert.ToByte(Speed);
                serialdata[3] = Convert.ToByte((iRPM >> 8) & 0x00FF);
                serialdata[4] = Convert.ToByte(iRPM & 0x00FF);
                serialdata[5] = Convert.ToByte(iFuel);
                serialdata[6] = Convert.ToByte(iShift);
                serialdata[7] = Engine;
                serialdata[8] = Convert.ToByte(Lap);
                serialdata[9] = Convert.ToByte(Boost[0]);
                serialdata[10] = Convert.ToByte(Boost[1]);

                SP.Write(serialdata, 0, 11);

            } else {
                if (sdk.IsConnected()) {
                    lblConn.Text = "Connected to iRacing API";
                    lblColor.BackColor = Color.FromArgb(0, 200, 0);

                    Gear = Convert.ToInt32(sdk.GetData("Gear"));
                    Speed = Convert.ToDouble(sdk.GetData("Speed")) * 2.23693629;
                    RPM = Convert.ToDouble(sdk.GetData("RPM"));
                    Fuel = Convert.ToDouble(sdk.GetData("FuelLevelPct"));
                    Shift = Convert.ToDouble(sdk.GetData("ShiftIndicatorPct"));
                    Engine = Convert.ToByte(sdk.GetData("EngineWarnings"));
                    Lap = Convert.ToInt32(sdk.GetData("Lap"));

                    this.Text = Shift.ToString();

                    iRPM = Convert.ToInt16(RPM);
                    iFuel = Convert.ToByte(Math.Round(Fuel * 100));
                    iShift = Convert.ToByte(Math.Round((Shift * 100 * 16) / 100));

                    serialdata[0] = 255;
                    serialdata[1] = Convert.ToByte(Gear + 1);
                    serialdata[2] = Convert.ToByte(Speed);
                    serialdata[3] = Convert.ToByte((iRPM >> 8) & 0x00FF);
                    serialdata[4] = Convert.ToByte(iRPM & 0x00FF);
                    serialdata[5] = Convert.ToByte(iFuel);
                    serialdata[6] = Convert.ToByte(iShift);
                    serialdata[7] = Engine;
                    serialdata[8] = Convert.ToByte(Lap);
                    serialdata[9] = 0;
                    serialdata[10] = 0;

                    SP.Write(serialdata, 0, 11);
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
                this.Height = 545;
            } else {
                this.Height = 205;
            }

        }
    }
}
