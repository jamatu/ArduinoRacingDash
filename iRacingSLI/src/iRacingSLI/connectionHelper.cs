using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using System.IO.Ports;

namespace iRacingSLI
{
    class connectionHelper
    {

        static SerialPort SP;
        Action<String> console;
        Boolean open;
        Boolean fake;

        public connectionHelper(Action<String> callConsole)
        {
            console = callConsole;
            open = false;
            fake = false;
        }

        public void setupConnection(Action<String> startMethod, System.Windows.Forms.ComboBox cbo, configHandler cfg)
        {
            Object[] arrayPorts = null;
            int startPort = -1;
            String setting = cfg.readSetting("Port", "*");
            using (var searcher = new ManagementObjectSearcher("SELECT * FROM WIN32_SerialPort"))
            {
                string[] portnames = SerialPort.GetPortNames();
                var ports = searcher.Get().Cast<ManagementBaseObject>().ToList();
                arrayPorts = (from n in portnames join p in ports on n equals p["DeviceID"].ToString() select p["Caption"]).ToArray();
            }
            cbo.Items.Add("Local");
            cbo.Items.AddRange(arrayPorts);

            if (setting != "*")
            {
                if (setting == "Local")
                {
                    startMethod(setting);
                }
                else
                {
                    for (int j = 0; j < SerialPort.GetPortNames().Length; j++)
                    {
                        if (SerialPort.GetPortNames()[j].Equals(setting))
                        {
                            try
                            {
                                startMethod(setting);
                                startPort = j;
                            }
                            catch
                            {
                                cfg.writeSetting("Port", "*");
                            }
                        }
                    }
                    if (startPort == -1)
                        cfg.writeSetting("Port", "*");
                }  
            }
            else
            {
                for (int i = 0; i < cbo.Items.Count; i++)
                {
                    if (cbo.Items[i].ToString().Contains("Arduino"))
                    {
                        String port = Regex.Match(cbo.Items[i].ToString(), @"\(([^)]*)\)").Groups[1].Value;
                        console("Found Arduino On Port: " + port);
                        startMethod(port);

                        for (int j = 0; j < SerialPort.GetPortNames().Length; j++)
                        {
                            if (SerialPort.GetPortNames()[j].Equals(port))
                                startPort = j;
                        }

                        break;
                    }
                }
            }
            cbo.SelectedIndex = startPort + 1;
        }

        public Boolean openSerial(String port, String arduinoVer)
        {
            if (SP != null && SP.IsOpen)
                SP.Close();
            if (port == "")
                return false;

            console("Opening Serial Port on: " + port);
            SP = new SerialPort(port, 9600, Parity.None, 8);
            SP.Open();
            open = true;

            //handshake
            SP.ReadTimeout = 5000;
            SP.Write("1");

            String s = "";
            int t = 0;
            while (s.Length < 4 && t < 500)
            {
                System.Threading.Thread.Sleep(10);
                s = SP.ReadExisting();

                if (!s.Contains("."))
                    s = "";
                else
                    s = s.Split('.')[1];

                SP.Write("1");
                t += 1;
            } 
            String av = arduinoVer.Replace(@".", string.Empty);
            if (av.Length < 4)
            {
                av = av[0] + "" + av[1] + "0" + av[2];
            }

            if (s == "")
                console("No Responce from Arduino, going ahead with connection but errors may occour");    
            else
            {
                s = s.Substring(s.Length - 4, 4);
                if (Convert.ToInt16(s) < Convert.ToInt16(av))
                {
                    console("Arduino Code Outdated(v" + s[0] + "." + s[1] + "." + s[2] + s[3] + "). Please Update Arduino to at least v" + arduinoVer + " and then Retry");
                    return false;
                }

                console("Handshake Sucsessful, Connected to Arduino Running: v" + s[0] + "." + s[1] + "." + s[2] + s[3]);
                iRacingSLI.currArduinoVersion = s[0] + "." + s[1] + "." + s[2] + s[3];
            }
            return true;
        }

        public void openFake()
        {
            fake = true;
            open = true;
            console("Opening Fake Port");
        }

        public void closeSerial()
        {
            if (!this.isFake())
                SP.Close();
            console("Closing Serial Port");
            open = false;
            fake = false;
        }

        public Boolean isOpen()
        {
            return open;
        }

        public Boolean isFake()
        {
            return fake;
        }

        public void send(Byte[] data)
        {
            if (!fake)
                SP.Write(data, 0, 16);
        }
    }
}
