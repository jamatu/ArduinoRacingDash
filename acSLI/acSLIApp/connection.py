import serial as serial
import serial.tools.list_ports
import re
import time
import acSLI
import threading
from acSLIApp.logger import Log
import acSLIApp.loader as Config
import acSLIApp.error as Error
import acSLIApp.utils as Utils

instance = 0


class Connection:
    ser = 0
    port = 0
    handshake = False
    dispSelect = False
    dispSelectMsg = 0

    def __init__(self):
        global instance
        instance = self

    def send(self, msg):
        self.ser.write(msg)

    def close(self):
        self.ser.close()


def _findConnect():
    global instance
    portValid = False
    instance.handshake = False
    instance.dispSelect = False

    for sPort, desc, hwid in sorted(serial.tools.list_ports.comports()):
        Log.log("%s: %s [%s]" % (sPort, desc, hwid))

        if Config.instance.cfgPort == "AUTO":
            if "Arduino" in desc:
                instance.port = sPort
                portValid = True
        else:
            if Config.instance.cfgPort == sPort:
                instance.port = sPort
                portValid = True

        if portValid:
            break

    if portValid:
        try:
            instance.ser = serial.Serial(instance.port, 9600, timeout=5)
            time.sleep(2)
            instance.ser.write(bytes([1, 1]))
            arduinoVer = instance.ser.read(5)

            if str(arduinoVer) == "b''":
                instance.port = "----"
                Log.warning("No Response From Arduino. Please Ensure Arduino is running at least v" +
                            acSLI.App.ArduinoVersion)
                instance.dispSelect = True
                instance.dispSelectMsg = "No Response from Arduino"
                instance.ser.close()
            else:
                aV = re.findall(r"\'(.+?)\'", str(arduinoVer))[0].split('.')[1]
                if len(aV) < 4:
                    aV = aV[0] + aV[1] + "0" + aV[2]

                aLV = "".join(acSLI.App.ArduinoVersion.split("."))
                if len(aLV) < 4:
                    aLV = aLV[0] + aLV[1] + "0" + aLV[2]

                if aLV > aV:
                    instance.port = "----"
                    instance.ser.close()
                    Log.warning("Arduino Code Outdated(v%s). Please Update Arduino to at least v%s and then Restart AC" % (arduinoVer, acSLI.App.ArduinoVersion))
                    Error.ErrorBox("Arduino Code Outdated. Please Update Arduino to at least v" +
                                   acSLI.App.ArduinoVersion + " and then Restart AC")
                else:
                    instance.handshake = True
                    Log.info("Connected to Arduino running v" + aV[0] + '.' + aV[1] + '.' + aV[2] + aV[3] + " on port " + instance.port)
        except Exception as e:
            Log.error("On Open Port: %s" % e)
    else:
        instance.port = "----"
        if Config.instance.cfgPort == "AUTO":
            Log.warning("No Arduino Detected")
            instance.dispSelect = True
            instance.dispSelectMsg = "No Arduino Detected"
        else:
            Log.warning("Invalid COM Port Configured")
            instance.dispSelect = True
            instance.dispSelectMsg = "Invalid COM Port Configured"

    if instance.port != "----":
        acSLI.acSLI.lblPort.setText("Connected COM Port: {}".format(instance.port)).setColor(Utils.rgb(Utils.colours["green"]))
    else:
        acSLI.acSLI.lblPort.setText("Connected COM Port: {}".format(instance.port)).setColor(Utils.rgb(Utils.colours["red"]))


class findConnection (threading.Thread):

    def __init__(self):
        threading.Thread.__init__(self)

    def run(self):
        _findConnect()