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

        public static String Version = "2.2.3";
        public static String ArduinoVersion = "2.1.2";
        public static String currArduinoVersion = "-";

        public iRacingSLI()
        {
            try
            {
                InitializeComponent();
                this.Text = "iRacing SLI v" + Version;
                cfg = new configHandler(console);
                connection = new connectionHelper(console);
                connection.setupConnection(startConnection, cboPorts, cfg);

                int top = Convert.ToInt16(cfg.readSetting("Top", "100")) > -3000 ? Convert.ToInt16(cfg.readSetting("Top", "100")) : 100;
                int left = Convert.ToInt16(cfg.readSetting("Left", "100")) > -3000 ? Convert.ToInt16(cfg.readSetting("Left", "100")) : 100;
                this.SetDesktopLocation(top, left);
                this.cboSpdUnit.SelectedIndex = Convert.ToInt16(cfg.readSetting("spdUnit", "0"));
                this.trkIntensity.Value = Convert.ToInt16(cfg.readSetting("intensity", "0"));
                this.chkTelem.Checked = Convert.ToBoolean(cfg.readSetting("telemEnable", "True"));

                console("Start iRacingSDK Wrapper");
                wrapper = new SdkWrapper();
                wrapper.EventRaiseType = SdkWrapper.EventRaiseTypes.CurrentThread;
                wrapper.TelemetryUpdateFrequency = 20;
                //wrapper.ConnectSleepTime = 1;

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
                ExceptionHelper.writeToLogFile(exe.Message, exe.ToString(), "Constructor", exe.LineNumber(), this.FindForm().Name);
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
                    console("Load data for " + car + " at " + track);
                }
                catch (Exception exe)
                {
                    ExceptionHelper.writeToLogFile(exe.Message, exe.ToString(), "Update Session Data", exe.LineNumber(), this.FindForm().Name);
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
                    if (!connection.isFake())
                    {
                        dataPacket data = new dataPacket(console);
                        data.fetch(e.TelemetryInfo, wrapper.Sdk, fuelEst, sendTimeReset, sendTime, prevFuel);
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
                    }


                    if (e.TelemetryInfo.Lap.Value > prevLap)
                    {
                        estimateFuel(e.TelemetryInfo);                   
                        prevLap = e.TelemetryInfo.Lap.Value;
                        sendTimeReset = true;
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
                ExceptionHelper.writeToLogFile(exe.Message, exe.ToString(), "Update Arduino", exe.LineNumber(), this.FindForm().Name);
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
                    telemPrint("");
                    telemPrint("Lap: " + telem.Lap.Value);
                    telemPrint("Total Flying Laps Completed: " + fuelLaps);
                    telemPrint("");
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
                ExceptionHelper.writeToLogFile(exe.Message, exe.ToString(), "Print Telemetry", exe.LineNumber(), this.FindForm().Name);
            }
        }

        private void wrapper_Connected(object sender, EventArgs e)
        {
            statusLabel.Text = "Status: connected!";
        }

        private void wrapper_Disconnected(object sender, EventArgs e)
        {
            statusLabel.Text = "Status: disconnected.";
            telemTextBox.Clear();
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
            {
                if (cboPorts.Text == "Local")
                    startConnection(cboPorts.Text);
                else
                    startConnection(Regex.Match(cboPorts.Text, @"\(([^)]*)\)").Groups[1].Value);
            }
        }

        private void btnDefault_Click(object sender, EventArgs e)
        {
            if (Regex.Match(cboPorts.Text, @"\(([^)]*)\)").Groups[1].Value == cfg.readSetting("Port", "*") || cboPorts.Text == cfg.readSetting("Port", "*"))
                cfg.writeSetting("Port", "*");
            else
            {
                if (cboPorts.Text == "Local")
                    cfg.writeSetting("Port", cboPorts.Text);
                else
                    cfg.writeSetting("Port", Regex.Match(cboPorts.Text, @"\(([^)]*)\)").Groups[1].Value);
            }

            if (Regex.Match(cboPorts.Text, @"\(([^)]*)\)").Groups[1].Value == cfg.readSetting("Port", "*") || cboPorts.Text == cfg.readSetting("Port", "*"))
                btnDefault.Text = "Remove Default";
            else
                btnDefault.Text = "Set Default";
        }

        private void cboPorts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Regex.Match(cboPorts.Text, @"\(([^)]*)\)").Groups[1].Value == cfg.readSetting("Port", "*") || cboPorts.Text == cfg.readSetting("Port", "*"))
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

        public void startConnection(String port)
        {
            startButton.Text = "Stop";
            cboPorts.Enabled = false;
            if (port == "Local")
                connection.openFake();
            else
                if (!connection.openSerial(port, ArduinoVersion)) stopConnection();
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
    }
}
