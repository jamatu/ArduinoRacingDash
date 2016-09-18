using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using iRSDKSharp;
using iRacingSdkWrapper;
using iRacingSdkWrapper.Bitfields;
using System.IO;
using System.Runtime.InteropServices;

namespace iRacingSLI
{
    public partial class iRacingSLI : Form
    {
        private SdkWrapper wrapper;
        private connectionHelper connection;
        private configHandler cfg;
        private brakeVibe brk;
        private int ticker;
        private Boolean hasInit;
        private int driverID;
        private String track;
        private String car;
        private double fuelEst;
        private int fuelLaps;
        private int prevLap;
        private double prevFuel;
        private float prevLapTime;
        private Boolean sendTime;
        private Boolean sendTimeReset;

        private String Version = "2.1.5";
        private String ArduinoVersion = "2.1.2";

        [DllImport("kernel32.dll")]
        static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);

        [FlagsAttribute]
        enum EXECUTION_STATE : uint
        {
            ES_AWAYMODE_REQUIRED = 0x00000040,
            ES_CONTINUOUS = 0x80000000,
            ES_DISPLAY_REQUIRED = 0x00000002,
            ES_SYSTEM_REQUIRED = 0x00000001
        }

        public iRacingSLI()
        {
            try
            {
                InitializeComponent();
                this.Text = "iRacing SLI v" + Version;
                cfg = new configHandler(console);
                connection = new connectionHelper(console);
                connection.setupConnection(startConnection, cboPorts, cfg);
                brk = new brakeVibe();

                int top = Convert.ToInt16(cfg.readSetting("Top", "100")) > -3000 ? Convert.ToInt16(cfg.readSetting("Top", "100")) : 100;
                int left = Convert.ToInt16(cfg.readSetting("Left", "100")) > -3000 ? Convert.ToInt16(cfg.readSetting("Left", "100")) : 100;
                this.SetDesktopLocation(top, left);
                this.cboSpdUnit.SelectedIndex = Convert.ToInt16(cfg.readSetting("spdUnit", "0"));
                this.trkIntensity.Value = Convert.ToInt16(cfg.readSetting("intensity", "0"));
                this.chkTelem.Checked = Convert.ToBoolean(cfg.readSetting("telemEnable", "True"));
                this.chkBrake.Checked = Convert.ToBoolean(cfg.readSetting("brakeEnable", "False"));
                this.groupBox1.Enabled = this.chkBrake.Checked;
                this.trkTol.Value = Convert.ToInt16(cfg.readSetting("brakeTol", "35"));
                this.trkSens.Value = Convert.ToInt16(cfg.readSetting("brakeSens", "3"));

                console("Start iRacingSDK Wrapper");
                wrapper = new SdkWrapper();
                wrapper.EventRaiseType = SdkWrapper.EventRaiseTypes.CurrentThread;
                wrapper.TelemetryUpdateFrequency = 20;

                wrapper.Connected += wrapper_Connected;
                wrapper.Disconnected += wrapper_Disconnected;
                wrapper.SessionInfoUpdated += wrapper_SessionInfoUpdated;
                wrapper.TelemetryUpdated += wrapper_TelemetryUpdated;

                wrapper.Start();
                ticker = 39;
                prevLapTime = 0;
            }
            catch (Exception exe)
            {
                LogFile(exe.Message, exe.ToString(), "Constructor", exe.LineNumber(), this.FindForm().Name);
            }
        }

        // Event handler called when the session info is updated
        private void wrapper_SessionInfoUpdated(object sender, SdkWrapper.SessionInfoUpdatedEventArgs e)
        {
            if (!hasInit)
            {
                try
                {
                    hasInit = true;
                    track = e.SessionInfo["WeekendInfo"]["TrackName"].Value;
                    driverID = Convert.ToInt16(e.SessionInfo["DriverInfo"]["DriverCarIdx"].Value);
                    car = e.SessionInfo["DriverInfo"]["Drivers"]["CarIdx", driverID]["CarPath"].Value;
                    fuelEst = Convert.ToDouble(cfg.readSetting(track + "-" + car, "0"));
                    fuelLaps = Convert.ToInt32(cfg.readSetting(track + "-" + car + "-l", "0"));
                }
                catch (Exception exe)
                {
                    LogFile(exe.Message, exe.ToString(), "Update Session Data", exe.LineNumber(), this.FindForm().Name);
                }
            }
        }

        // Event handler called when the telemetry is updated
        private void wrapper_TelemetryUpdated(object sender, SdkWrapper.TelemetryUpdatedEventArgs e)
        {
            try
            {
                if (connection.isOpen())
                {
                    dataPacket data = new dataPacket(console);
                    data.fetch(e.TelemetryInfo, wrapper.Sdk, fuelEst, chkBrake.Checked ? brk.getBrakeVibe(e.TelemetryInfo, trkTol.Value, trkSens.Value) : 0,
                        sendTimeReset, sendTime, prevFuel);
                    connection.send(data.compile(this.cboSpdUnit.SelectedIndex == 0, this.trkIntensity.Value));
                    sendTime = false;
                    sendTimeReset = false;

                    float ll = Convert.ToSingle(wrapper.Sdk.GetData("LapLastLapTime"));

                    if (ll != prevLapTime)
                    {
                        if (prevFuel != 0 && ll > 0)
                        {
                            sendTime = true;
                            prevLapTime = ll;
                        }
                    }

                    if (e.TelemetryInfo.Lap.Value > prevLap)
                    { 
                        sendTimeReset = true;                    
                        estimateFuel(e.TelemetryInfo);
                        prevLap = e.TelemetryInfo.Lap.Value;
                    }

                    if (wrapper.GetTelemetryValue<Boolean[]>("CarIdxOnPitRoad").Value[driverID])
                        prevFuel = 0;
                }

                if (ticker % 5 == 0)
                {
                    printTelemInfo(e.TelemetryInfo);              
                    ticker += 1;
                }

                if (ticker == 40)
                {
                    ticker = 0;
                }
                else
                    ticker += 1;
            }
            catch (Exception exe)
            {
                LogFile(exe.Message, exe.ToString(), "Update Arduino", exe.LineNumber(), this.FindForm().Name);
            }
        }

        private void estimateFuel(TelemetryInfo telem)
        {
            if (prevFuel != 0)
            {
                double usg = prevFuel - telem.FuelLevel.Value;
                double tmp = (fuelEst * fuelLaps) + usg;
                fuelLaps += 1;
                fuelEst = tmp / fuelLaps;
                cfg.writeSetting(track + "-" + car, Convert.ToString(fuelEst));
                cfg.writeSetting(track + "-" + car + "-l", Convert.ToString(fuelLaps));
                //console("Recalculate Fuel Usage per Lap to: " + fuelEst);
            }
            prevFuel = telem.FuelLevel.Value;
        }

        private void printTelemInfo(TelemetryInfo telem)
        {
            try
            {
                if (this.chkTelem.Checked) {
                    telemTextBox.Clear();

                    String gr = "";
                    if (telem.Gear.Value == -1)
                        gr = "R";
                    else if (telem.Gear.Value == 0)
                        gr = "N";
                    else
                        gr = Convert.ToString(telem.Gear.Value);

                    telemPrint("Car: " + car);
                    telemPrint("Track: " + track);
                    telemPrint("");
                    telemPrint("Gear: " + gr);
                    telemPrint("RPM: " + Math.Round(telem.RPM.Value));
                    telemPrint("Speed: " + (this.cboSpdUnit.SelectedIndex == 0 ? Math.Round(telem.Speed.Value * 2.23693629, 1) + "MPH" : Math.Round(telem.Speed.Value * (2.23693629 * 1.609344), 1) + "KPH"));
                    telemPrint("Lap: " + telem.Lap.Value);
                    telemPrint("Total Flying Laps Completed: " + fuelLaps);
                    telemPrint("Fuel PCT: " + telem.FuelLevelPct.Value * 100);
                    telemPrint("Fuel Lvl (L): " + telem.FuelLevel.Value);
                    telemPrint("Fuel Use on Current Lap (L): " + ((this.prevFuel - telem.FuelLevel.Value)>0 ? Math.Round(this.prevFuel - telem.FuelLevel.Value, 3) : 0));
                    telemPrint("Fuel Use Per Lap Avg(L): " + Math.Round(fuelEst, 5));
                    telemPrint("Laps Left EST: " + Math.Round(telem.FuelLevel.Value / fuelEst, 2));
                    telemPrint(""); 
                }
            }
            catch (Exception exe)
            {
                LogFile(exe.Message, exe.ToString(), "Print Telemetry", exe.LineNumber(), this.FindForm().Name);
            }
        }

        private void StatusChanged()
        {
            if (wrapper.IsConnected)
            {
                if (wrapper.IsRunning)
                {
                    statusLabel.Text = "Status: connected!";
                    ticker = 0;
                    hasInit = false;
                    SetThreadExecutionState(EXECUTION_STATE.ES_DISPLAY_REQUIRED | EXECUTION_STATE.ES_CONTINUOUS);
                }
                else
                {
                    statusLabel.Text = "Status: disconnected.";
                    telemTextBox.Clear();
                    SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS);
                }
            }
            else
            {
                if (wrapper.IsRunning)
                {
                    statusLabel.Text = "Status: disconnected, waiting for sim...";
                    telemTextBox.Clear();
                    SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS);
                }
                else
                {
                    statusLabel.Text = "Status: disconnected";
                    telemTextBox.Clear();
                    SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS);
                }
            }
        }

        private void wrapper_Connected(object sender, EventArgs e)
        {
            this.StatusChanged();
        }

        private void wrapper_Disconnected(object sender, EventArgs e)
        {
            this.StatusChanged();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            wrapper.Stop();
            cfg.writeSetting("Top", Convert.ToString(this.Location.X));
            cfg.writeSetting("Left", Convert.ToString(this.Location.Y));
            Application.Exit();
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            if (startButton.Text == "Stop")
                stopConnection();
            else
                startConnection(Regex.Match(cboPorts.Text, @"\(([^)]*)\)").Groups[1].Value);

            this.StatusChanged();
        }

        private void btnDefault_Click(object sender, EventArgs e)
        {
            if (Regex.Match(cboPorts.Text, @"\(([^)]*)\)").Groups[1].Value == cfg.readSetting("Port", "*"))
                cfg.writeSetting("Port", "*");
            else
                cfg.writeSetting("Port", Regex.Match(cboPorts.Text, @"\(([^)]*)\)").Groups[1].Value);

            if (Regex.Match(cboPorts.Text, @"\(([^)]*)\)").Groups[1].Value == cfg.readSetting("Port", "*"))
                btnDefault.Text = "Remove Default";
            else
                btnDefault.Text = "Set Default";
        }

        private void cboPorts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Regex.Match(cboPorts.Text, @"\(([^)]*)\)").Groups[1].Value == cfg.readSetting("Port", "*"))
                btnDefault.Text = "Remove Default";
            else
                btnDefault.Text = "Set Default";
        }

        private void cboSpdUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            cfg.writeSetting("spdUnit", Convert.ToString(this.cboSpdUnit.SelectedIndex));
        }

        private void trkIntensity_ValueChanged(object sender, EventArgs e)
        {
            cfg.writeSetting("intensity", Convert.ToString(this.trkIntensity.Value));
        }

        private void chkTelem_CheckedChanged(object sender, EventArgs e)
        {
            cfg.writeSetting("telemEnable", Convert.ToString(chkTelem.Checked));
            telemTextBox.Clear();
        }

        private void chkBrake_CheckedChanged(object sender, EventArgs e)
        {
            this.groupBox1.Enabled = this.chkBrake.Checked;
            cfg.writeSetting("brakeEnable", Convert.ToString(chkBrake.Checked));
        }

        private void trkTol_ValueChanged(object sender, EventArgs e)
        {
            lblTol.Text = "(" + trkTol.Value + "%)";
            cfg.writeSetting("brakeTol", Convert.ToString(trkTol.Value));
        }

        private void trkSens_ValueChanged(object sender, EventArgs e)
        {
            lblSens.Text = "(" + trkSens.Value + ")";
            cfg.writeSetting("brakeSens", Convert.ToString(trkSens.Value));
        }

        public void startConnection(String port)
        {
            startButton.Text = "Stop";
            cboPorts.Enabled = false;
            if (!connection.openSerial(port, ArduinoVersion))
                stopConnection();
        }

        public void stopConnection()
        {
            connection.closeSerial();
            startButton.Text = "Start";
            cboPorts.Enabled = true;
        }

        public void console(String str)
        {
            if (consoleTextBox.Text.Length > 0)
                consoleTextBox.AppendText("\r\n" + str);
            else
                consoleTextBox.AppendText(str);
        }

        public void telemPrint(String str)
        {
            if (telemTextBox.Text.Length > 0)
                telemTextBox.AppendText("\r\n" + str);
            else
                telemTextBox.AppendText(str);
        }

        public void LogFile(string sExceptionName, string sEventName, string sControlName, int nErrorLineNo, string sFormName)
        {

            StreamWriter log;

            if (!File.Exists("logfile.txt"))
            {
                log = new StreamWriter("logfile.txt");
            }
            else
            {
                log = File.AppendText("logfile.txt");
            }

            log.WriteLine("Data Time:" + DateTime.Now);
            log.WriteLine("Exception Name:" + sExceptionName);
            log.WriteLine("Event Name:" + sEventName);
            log.WriteLine("Control Name:" + sControlName);
            log.WriteLine("Error Line No.:" + nErrorLineNo);
            log.WriteLine("Form Name:" + sFormName);

            log.Close();
        }
    }

    public static class ExceptionHelper
    {
        public static int LineNumber(this Exception e)
        {
            int linenum = 0;

            try
            {
                linenum = Convert.ToInt32(e.StackTrace.Substring(e.StackTrace.LastIndexOf(":line") + 5));
            }
            catch{}
            return linenum;
        }
    }
}
