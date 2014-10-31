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
Version = "2.0.13"
ArduinoVersion = "2.0.13"
#################

Log = Logger()


class App:

    appWindow = 0
    simInfo = 0
    ticker = 0
    fuelCache = 0
    track = 0
    car = 0

    lblPort = 0
    btnUnits = 0
    btnFuel = 0
    spnStartPage = 0
    spnIntensity = 0

    maxRPM = 0
    maxFuel = 0

    fuelEst = 0
    prevLap = 0
    prevFuel = 0

    def __init__(self):
        self.simInfo = Info()
        self.appWindow = Window("acSLI", 250, 260).setBackgroundTexture("apps/python/acSLI/image/backMain.png")

        self.lblPort = Label(self.appWindow.app, "Connected COM Port: {}".format(Config.instance.cfgPort), 15, 40)\
            .setSize(220, 10).setAlign("center").setColor(Utils.rgb(Utils.colours["red"]))
        self.btnUnits = Button(self.appWindow.app, bFunc_SpeedUnits, 200, 20, 25, 90, "Speed Units: {}".format(Config.instance.cfgSpeedUnit))\
                .setAlign("center").hasCustomBackground().setBackgroundTexture("apps/python/acSLI/image/backBtnLarge.png")
        self.btnFuel = Button(self.appWindow.app, bFunc_FuelDisp, 200, 20, 25, 120, "Fuel Display: {}".format(Config.instance.cfgFuelDisp))\
                .setAlign("center").hasCustomBackground().setBackgroundTexture("apps/python/acSLI/image/backBtnLarge.png")

        self.spnStartPage = Spinner(self.appWindow.app, sFunc_StartPage, 220, 20, 15, 165, "Startup Page", 0, 7, Config.instance.cfgStartPage)
        self.spnIntensity = Spinner(self.appWindow.app, sFunc_Intensity, 220, 20, 15, 215, "Display Intensity", 0, 7, Config.instance.cfgIntensity)


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

        if self.ticker == 60:
            if self.fuelCache == 0 and self.simInfo.static.track != "" and self.simInfo.static.carModel != "":
                Log.info("Load Fuel Usage Cache")
                self.fuelCache = Utils.Config("apps/python/acSLI/user.cache")
                self.track = self.simInfo.static.track
                self.car = self.simInfo.static.carModel
                self.fuelEst = float(self.fuelCache.getOption(self.track, self.car, True, self.fuelEst))

            if self.simInfo.graphics.completedLaps > self.prevLap:
                self.prevLap = self.simInfo.graphics.completedLaps
                self.estimateFuel()
                Log.info("new lap detected")

            self.ticker = 0
        else:
            self.ticker += 1

    def onClose(self):
        Connection.instance.close

    def compileDataPacket(self):
        ac_gear = ac.getCarState(0, acsys.CS.Gear)
        ac_speed = int(round(ac.getCarState(0, acsys.CS.SpeedMPH)) if Config.instance.cfgSpeedUnit == "MPH" else
                       round(ac.getCarState(0, acsys.CS.SpeedKMH)))

        rpms = int(ac.getCarState(0, acsys.CS.RPM))
        self.maxRPM = self.simInfo.static.maxRpm if self.maxRPM == 0 else self.maxRPM #blank the if statement if sim-info issue where max rpm doesn't change is fixed
        shift = 0
        if self.maxRPM > 0:
            thresh = self.maxRPM*0.65
            if rpms >= thresh:
                shift = round(((rpms-thresh)/(self.maxRPM-thresh))*16)
                if shift > 16:
                    shift = 16

        engine = 0
        if self.simInfo.physics.pitLimiterOn and not self.simInfo.graphics.isInPit:
            engine = 1

        current_fuel = self.simInfo.physics.fuel
        fuel = 0
        if Config.instance.cfgFuelDisp == "PERCENTAGE":
            self.maxFuel = self.simInfo.static.maxFuel if self.maxFuel == 0 else self.maxFuel
            fuel = int((current_fuel/self.maxFuel)*100)
        else:
            fuel = round(current_fuel/self.fuelEst, 1)
            if fuel > 9.9:
                fuel = round(fuel)
            else:
                fuel = round(fuel*10)
                engine |= 1 << 1
        if fuel > 99:
            fuel = 99

        lapCount = self.simInfo.graphics.completedLaps + Config.instance.cfgLapOffset
        if lapCount > 199:
            lapCount = 199

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

    def estimateFuel(self):
        if self.prevFuel != 0 and self.fuelCache != 0:
            fuelUsg = self.prevFuel - self.simInfo.physics.fuel
            if self.fuelEst == 0:
                self.fuelEst = fuelUsg
            else:
                self.fuelEst = (self.fuelEst + fuelUsg)/2
            self.fuelCache.updateOption(self.track, self.car, self.fuelEst, True)

        self.prevFuel = self.simInfo.physics.fuel
        Log.info(self.simInfo.physics.fuel)
        Log.info(self.fuelEst)


def bFunc_SpeedUnits(dummy, variables):
    if Config.instance.cfgSpeedUnit == "MPH":
        Config.instance.cfgSpeedUnit = "KPH"
    elif Config.instance.cfgSpeedUnit == "KPH":
        Config.instance.cfgSpeedUnit = "MPH"

    acSLI.acSLI.btnUnits.setText("Speed Units: {}".format(Config.instance.cfgSpeedUnit))
    Config.instance.rewriteConfig()


def bFunc_FuelDisp(dummy, variables):
    if Config.instance.cfgFuelDisp == "LAP ESTIMATE":
        Config.instance.cfgFuelDisp = "PERCENTAGE"
    elif Config.instance.cfgFuelDisp == "PERCENTAGE":
        Config.instance.cfgFuelDisp = "LAP ESTIMATE"

    acSLI.acSLI.btnFuel.setText("Fuel Display: {}".format(Config.instance.cfgFuelDisp))
    Config.instance.rewriteConfig()


def sFunc_StartPage(dummy):
    Config.instance.cfgStartPage = int(acSLI.acSLI.spnStartPage.getValue())
    Config.instance.rewriteConfig()


def sFunc_Intensity(dummy):
    Config.instance.cfgIntensity = int(acSLI.acSLI.spnIntensity.getValue())
    Config.instance.rewriteConfig()