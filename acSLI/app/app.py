import ac
import acsys
from app.components import Window, Label, Button
from app.logger import Logger
from app.sim_info import SimInfo as Info
import app.loader as Config
import app.connection as Connection
import app.selector as Selector

#################
Version = "1.9.0"
ArduinoVersion = "1.9.0"
#################

Log = Logger()


class App:

    appWindow = 0
    simInfo = 0
    ticker = 0

    maxRPM = 0
    maxFuel = 0

    def __init__(self):
        global Version

        self.simInfo = Info()
        self.appWindow = Window("acSLI " + Version, 250, 244)

    def onStart(self):
        Selector.Selector()
        Connection.Connection()
        Connection.findConnection().start()
        self.ticker = 0

        Log.info("Loaded Successfully")
        
    def onUpdate(self):
        if self.ticker % 3 == 0:
            if Connection.instance.handshake:
                Connection.instance.send(self.compileDataPacket())
            else:
                if Connection.instance.dispSelect:
                    Selector.instance.open(Connection.instance.dispSelectMsg)
                    Connection.instance.dispSelect = False

        if self.ticker == 30:
            self.ticker = 0
        else:
            self.ticker += 1

    @staticmethod
    def onClose():
        Connection.instance.close

    def compileDataPacket(self):

        ac_gear = ac.getCarState(0, acsys.CS.Gear)
        ac_speed = int(round(ac.getCarState(0, acsys.CS.SpeedMPH)) if Config.instance.cfgSpeedUnit == "MPH" else
                       round(ac.getCarState(0, acsys.CS.SpeedKMH)))

        rpms = int(ac.getCarState(0, acsys.CS.RPM))
        self.maxRPM = self.simInfo.static.maxRpm if self.maxRPM == 0 else self.maxRPM

        shift = 0
        if self.maxRPM > 0:
            thresh = self.maxRPM*0.65
            if rpms >= thresh:
                shift = round(((rpms-thresh)/(self.maxRPM-thresh))*16)

        current_fuel = self.simInfo.physics.fuel
        self.maxFuel = self.simInfo.static.maxFuel if self.maxFuel == 0 else self.maxFuel
        fuel = int((current_fuel/self.maxFuel)*100)

        lapCount = self.simInfo.graphics.completedLaps
        if lapCount > 199:
            lapCount = 199

        engine = 0x00
        if self.simInfo.physics.pitLimiterOn and not self.simInfo.graphics.isInPit:
            engine = 0x10

        boost = round(ac.getCarState(0, acsys.CS.TurboBoost), 1)
        b1 = int(boost*10)

        delta = ac.getCarState(0, acsys.CS.PerformanceMeter)
        deltaNeg = 0
        if delta <= 0:
            deltaNeg = 1
        delta = int(abs(delta) * 1000)
        if delta > 9999:
            delta = 9999

        bSetting = int(deltaNeg << 7) | int(1 << 4) | int(Config.instance.cfgStartPage)

        key = bytes([255, bSetting, ac_gear, (ac_speed >> 8 & 0x00FF), (ac_speed & 0x00FF), ((rpms >> 8) & 0x00FF),
                     (rpms & 0x00FF), fuel, shift, engine, lapCount, b1, ((delta >> 8) & 0x00FF), (delta & 0x00FF)])
        return key
