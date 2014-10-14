import serial
import serial.tools.list_ports
import re
import acSLI
from app.logger import Logger
import app.loader as Config
import app.error as Error
import app.selector as Selector

Log = Logger()
instance = 0


class Connection:
    ser = 0
    port = 0
    handshake = False

    def __init__(self):
        global instance
        instance = self

        self.handshake = False
        self.findConnection(True)

    def findConnection(self, init):
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
                self.port = "----"
                Log.warning("No Response From Arduino. Please Ensure Arduino is running at least v" +
                            acSLI.App.ArduinoVersion)
                if init:
                    Selector.instance.open(self, "No Response from Arduino")
            else:
                aV = re.findall(r"\'(.+?)\'", str(arduinoVer))[0]
                if "".join(acSLI.App.ArduinoVersion.split(".")) > aV:
                    self.port = "----"
                    Log.warning("Arduino Code Outdated. Please Update Arduino to at least v" +
                                acSLI.App.ArduinoVersion)
                    Error.ErrorBox("Arduino Code Outdated. Please Update Arduino to at least v" +
                                   acSLI.App.ArduinoVersion + " and then Reconnect")
                else:
                    self.handshake = True
                    Log.info("Connected to Arduino running v"
                             + aV[0] + '.' + aV[1] + '.' + aV[2] + " on port " + self.port)
        else:
            self.port = "----"
            if Config.instance.cfgPort == "AUTO":
                Log.warning("No Arduino Detected")
                if init:
                    Selector.instance.open(self, "No Arduino Detected")
            else:
                Log.warning("Invalid COM Port Configured")
                if init:
                    Selector.instance.open(self, "Invalid COM Port Configured")

                # ac.setText(lbConnectedPort, "Connected COM Port: {}".format(port))
        return self.handshake

    def send(self, msg):
        self.ser.write(msg)

    def close(self):
        self.ser.close()