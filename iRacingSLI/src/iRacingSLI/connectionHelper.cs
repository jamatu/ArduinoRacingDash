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

        public connectionHelper(Action<String> callConsole)
        {
            console = callConsole;
            open = false;
        }

        public void setupConnection(Action<String> startMethod, System.Windows.Forms.ComboBox cbo)
        {
            Object[] arrayPorts = null;
            int startPort = 0;
            using (var searcher = new ManagementObjectSearcher("SELECT * FROM WIN32_SerialPort"))
            {
                string[] portnames = SerialPort.GetPortNames();
                var ports = searcher.Get().Cast<ManagementBaseObject>().ToList();
                arrayPorts = (from n in portnames join p in ports on n equals p["DeviceID"].ToString() select p["Caption"]).ToArray();
            }
            cbo.Items.AddRange(arrayPorts);

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
            cbo.SelectedIndex = startPort;
        }

        public void openSerial(String port)
        {
            if (SP != null && SP.IsOpen)
                SP.Close();
            console("Opening Serial Port on: " + port);
            SP = new SerialPort(port, 9600, Parity.None, 8);
            SP.Open();
            open = true;
        }

        public void closeSerial()
        {
            SP.Close();
            console("Closing Serial Port");
            open = false;
        }

        public Boolean isOpen()
        {
            return open;
        }

        public void send(Byte[] data)
        {
            SP.Write(data, 0, 15);
        }
    }
}
