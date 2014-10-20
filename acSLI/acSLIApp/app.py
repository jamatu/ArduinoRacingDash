import ac
import acsys
import acSLI
from acSLIApp.components import Window, Label, Button, Spinner
from acSLIApp.logger import Logger
from acSLIApp.sim_info import SimInfo as Info
import acSLIApp.loader as Config
import acSLIApp.connection as Connection
import acSLIApp.selector as Selector
import acSLIApp.utils as Utils

#################
Version = "2.0.3"
ArduinoVersion = "2.0.5"
#################

Log = Logger()


class App:

    appWindow = 0
    simInfo = 0
    ticker = 0

    lblPort = 0
    btnUnits = 0
    spnStartPage = 0
    spnIntensity = 0

    maxRPM = 0
    maxFuel = 0

    def __init__(self):
        self.simInfo = Info()
        self.appWindow = Window("acSLI", 250, 230).setBackgroundTexture("apps/python/acSLI/image/backMain.png")

        self.lblPort = Label(self.appWindow.app, "Connected COM Port: {}".format(Config.instance.cfgPort), 15, 40)\
            .setSize(220, 10).setAlign("center").setColor(Utils.rgb(Utils.colours["red"]))
        self.btnUnits = Button(self.appWindow.app, bFunc_SpeedUnits, 160, 20, 45, 90, "Speed Units: {}".format(Config.instance.cfgSpeedUnit))\
                .setAlign("center").hasCustomBackground().setBackgroundTexture("apps/python/acSLI/image/backBtnAuto.png")

        self.spnStartPage = Spinner(self.appWindow.app, sFunc_StartPage, 220, 20, 15, 135, "Startup Page", 0, 7, Config.instance.cfgStartPage)
        self.spnIntensity = Spinner(self.appWindow.app, sFunc_Intensity, 220, 20, 15, 185, "Display Intensity", 0, 7, Config.instance.cfgIntensity)


    def onStart(self):
        global Version
        Selector.Selector()
        Connection.Connection()
        Connection.findConnection().start()
        self.ticker = 0

        Log.info("Loaded Successfully")
        
    def onUpdate(self):
        if self.ticker % Config.instance.cfgTickFreq == 0:
            if Connection.instance.handshake:
                Connection.instance.send(self.compileDataPacket())
            elif Connection.instance.dispSelect:
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

        bSetting = int(deltaNeg << 7) | int(Config.instance.cfgIntensity << 4) | int(Config.instance.cfgStartPage)

        key = bytes([255, bSetting, ac_gear, (ac_speed >> 8 & 0x00FF), (ac_speed & 0x00FF), ((rpms >> 8) & 0x00FF),
                     (rpms & 0x00FF), fuel, shift, engine, lapCount, b1, ((delta >> 8) & 0x00FF), (delta & 0x00FF)])
        return key


def bFunc_SpeedUnits(dummy, variables):
    if Config.instance.cfgSpeedUnit == "MPH":
        Config.instance.cfgSpeedUnit = "KPH"
    elif Config.instance.cfgSpeedUnit == "KPH":
        Config.instance.cfgSpeedUnit = "MPH"

    acSLI.acSLI.btnUnits.setText("Speed Units: {}".format(Config.instance.cfgSpeedUnit))
    Config.instance.rewriteConfig()


def sFunc_StartPage(dummy):
    Config.instance.cfgStartPage = int(acSLI.acSLI.spnStartPage.getValue())
    Config.instance.rewriteConfig()


def sFunc_Intensity(dummy):
    Config.instance.cfgIntensity = int(acSLI.acSLI.spnIntensity.getValue())
    Config.instance.rewriteConfig()