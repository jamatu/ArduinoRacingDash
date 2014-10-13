from app.components import Window, Label, Button
from app.logger import Logger
from app.sim_info import SimInfo as Info
import app.connection as Connection

#################
Version = "1.9.0"
ArduinoVersion = "1.9.0"
#################

Log = Logger()

appWindow = 0
simInfo = 0


class App:

    def __init__(self):
        global Version

        self.simInfo = Info()
        self.appWindow = Window("acSLI " + Version, 250, 244)

    def onStart(self):
        Connection.Connection()

        if Connection.instance.handshake:
            Log.info("Loaded Successfully")
        else:
            Log.warning("Loaded with Errors")
        
    def onUpdate(self):
        #Log.msg(str(self.simInfo.graphics.lastTime))
        pass
        
    def onClose(self):
        pass

