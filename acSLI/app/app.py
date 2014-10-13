import ac
import serial
from app.logger import Logger
from app.sim_info import SimInfo as Info


Log = Logger()

appWindow = 0
simInfo = 0


class App:

    def __init__(self, Version):

        self.simInfo = Info()
        self.appWindow = ac.newApp("AC SLI " + Version)
        
        ac.setSize(self.appWindow, 250, 244)
        ac.drawBorder(self.appWindow, 0)
        ac.setBackgroundOpacity(self.appWindow, 0)

    def onStart(self):
        Log.info("acSLI Loaded")
        
    def onUpdate(self):
        #Log.msg(str(self.simInfo.graphics.lastTime))
        pass
        
    def onClose(self):
        pass

