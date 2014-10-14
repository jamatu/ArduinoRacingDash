import serial
import serial.tools.list_ports
import re
import acSLI
from app.logger import Logger
import app.loader as Config

Log = Logger()
instance = 0


class Connection:

    ser = 0
    port = 0
    handshake = False

    def __init__(self):
        global instance

        self.handshake = False
        self._findConnection()
        instance = self

    def _findConnection(self):
        portValid = False
        for sPort, desc, hwid in sorted(serial.tools.list_ports.comports()):
            Log.info("%s: %s [%s]" % (sPort, desc, hwid))

            if Config.instance.cfgPort == "AUTO":
                if "Arduino" in desc:
                    self.port = sPort
                    portValid = True
            else:
                if Config.instance.cfgPort == sPort:
                    self.port = sPort
                    portValid = True

            if portValid:
                break

        if portValid:
            self.ser = serial.Serial(self.port, 9600, timeout=5)
            arduinoVer = self.ser.read(3)

            if str(arduinoVer) == "b''":
                Log.warning("No Response From Arduino")
            else:
                aV = re.findall(r"\'(.+?)\'", str(arduinoVer))[0]
                if "".join(acSLI.App.ArduinoVersion.split(".")) > aV:
                    Log.warning("Arduino Code Outdated, Please Update to a Compatible Version")
                else:
                    self.handshake = True
                    Log.info("Connected to Arduino running v"
                             + aV[0] + '.' + aV[1] + '.' + aV[2] + " on port " + self.port)
        else:
            self.port = "----"
            if Config.instance.cfgPort == "AUTO":
                Log.info("No Arduino Detected")
            else:
                Log.info("Invalid COM Port")

        #ac.setText(lbConnectedPort, "Connected COM Port: {}".format(port))

    def send(self, msg):
        self.ser.write(msg)

    def close(self):
        self.ser.close()