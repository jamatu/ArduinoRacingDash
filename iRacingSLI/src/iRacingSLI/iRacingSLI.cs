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

        public iRacingSLI()
        {
            InitializeComponent();
            console("Start iRacingSDK Wrapper");
            wrapper = new SdkWrapper();
            wrapper.EventRaiseType = SdkWrapper.EventRaiseTypes.CurrentThread;
            wrapper.TelemetryUpdateFrequency = 20;

            wrapper.Connected += wrapper_Connected;
            wrapper.Disconnected += wrapper_Disconnected;
            wrapper.SessionInfoUpdated += wrapper_SessionInfoUpdated;
            wrapper.TelemetryUpdated += wrapper_TelemetryUpdated;

            connection = new connectionHelper(console);
            connection.setupConnection(startConnection, cboPorts);
            cfg = new configHandler(console);

            wrapper.Start();
            ticker = 39;
        }

        // Event handler called when the session info is updated
        private void wrapper_SessionInfoUpdated(object sender, SdkWrapper.SessionInfoUpdatedEventArgs e)
        {
            if (!hasInit)
            {
                hasInit = true;
                track = e.SessionInfo["WeekendInfo"]["TrackName"].Value;
                driverID = Convert.ToInt16(e.SessionInfo["DriverInfo"]["DriverCarIdx"].Value);
                car = e.SessionInfo["DriverInfo"]["Drivers"]["CarIdx", driverID]["CarPath"].Value;
                fuelEst = Convert.ToDouble(cfg.readSetting(track + "-" + car));
                fuelLaps = Convert.ToInt32(cfg.readSetting(track + "-" + car + "-l"));
            }
        }

        // Event handler called when the telemetry is updated
        private void wrapper_TelemetryUpdated(object sender, SdkWrapper.TelemetryUpdatedEventArgs e)
        {
            if (connection.isOpen())
            {
                dataPacket data = new dataPacket(console);
                data.fetch(e.TelemetryInfo, wrapper.Sdk, fuelEst);
                connection.send(data.compile(false, 0));
            }

            if (ticker == 40)
            {
                if (e.TelemetryInfo.Lap.Value > prevLap)
                {
                    estimateFuel(e.TelemetryInfo);
                    prevLap = e.TelemetryInfo.Lap.Value;
                }

                if (wrapper.GetTelemetryValue<Boolean[]>("CarIdxOnPitRoad").Value[driverID])
                    prevFuel = 0;

                ticker = 0;
            }
            if (ticker % 5 == 0)
            {
                printTelemInfo(e.TelemetryInfo);
                ticker += 1;
            }
            else
                ticker += 1;
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
                console("Recalculate Fuel Usage per Lap to: " + fuelEst);
            }
            prevFuel = telem.FuelLevel.Value;
        }

        private void printTelemInfo(TelemetryInfo telem)
        {
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
            telemPrint("Speed: " + Math.Round(telem.Speed.Value * 2.23693629, 1) + "MPH");
            telemPrint("Fuel PCT: " + telem.FuelLevelPct.Value * 100);
            telemPrint("Fuel Lvl (L): " + telem.FuelLevel.Value);
            telemPrint("Fuel Use Per Lap (L): " + Math.Round(fuelEst, 5));
            telemPrint("Laps Left EST: " + Math.Round(telem.FuelLevel.Value / fuelEst, 2));
            telemPrint("");
            telemPrint("Lap: " + telem.Lap.Value);
            telemPrint("prevFuelLvl: " + prevFuel);     
            telemPrint("onPitRoad: " + wrapper.GetTelemetryValue<Boolean[]>("CarIdxOnPitRoad").Value[driverID]);

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
                }
                else
                {
                    statusLabel.Text = "Status: disconnected.";
                    telemTextBox.Clear();
                }
            }
            else
            {
                if (wrapper.IsRunning)
                {
                    statusLabel.Text = "Status: disconnected, waiting for sim...";
                    telemTextBox.Clear();
                }
                else
                {
                    statusLabel.Text = "Status: disconnected";
                    telemTextBox.Clear();
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
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            if (startButton.Text == "Stop")
                stopConnection();
            else
                startConnection(Regex.Match(cboPorts.Text, @"\(([^)]*)\)").Groups[1].Value);

            this.StatusChanged();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            wrapper.Stop();
            Application.Exit();
        }

        public void startConnection(String port)
        {
            //wrapper.Start();
            startButton.Text = "Stop";
            connection.openSerial(port);
        }

        public void stopConnection()
        {
            //wrapper.Stop();
            startButton.Text = "Start";
            connection.closeSerial();
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
