import ac
import acsys
import acSLI
from acSLIApp.components import Window, Label, Button, Spinner
from acSLIApp.logger import Log
from acSLIApp.sim_info import SimInfo as Info
import acSLIApp.loader as Config
import acSLIApp.connection as Connection
import acSLIApp.selector as Selector
import acSLIApp.advSettings as AdvSet
import acSLIApp.utils as Utils

#################
Version = "2.1.5"
ArduinoVersion = "2.1.2"
#################


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
    fuelEstLaps = 0
    prevLap = 0
    prevFuel = 0

    sendTime = 0
    sendTimeReset = 0

    old_ratio = 0
    ratio = 0

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
        AdvSet.AdvSet()
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

            if self.simInfo.graphics.completedLaps > self.prevLap:
                self.prevLap = self.simInfo.graphics.completedLaps
                self.sendTimeReset = 1
                if self.prevFuel != 0:
                    self.sendTime = 1
                self.estimateFuel()
            if self.simInfo.physics.pitLimiterOn or self.simInfo.graphics.isInPit:
                self.prevFuel = 0

        if self.ticker == (Config.instance.cfgTickFreq * 10):
            if self.fuelCache == 0 and self.simInfo.static.track != "" and self.simInfo.static.carModel != "":
                Log.info("Load Fuel Usage Cache")
                self.fuelCache = Utils.Config("apps/python/acSLI/user.cache")
                self.track = self.simInfo.static.track
                self.car = self.simInfo.static.carModel
                self.fuelEst = float(self.fuelCache.getOption(self.track, self.car, True, self.fuelEst))
                self.fuelEstLaps = int(self.fuelCache.getOption(self.track, self.car + "_l", True, self.fuelEstLaps))

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
            engine |= 3 << 1
        else:
            if self.fuelEst != 0:
                fuel = round(current_fuel/self.fuelEst, 2)
                if fuel > 99.9:
                    fuel = round(fuel)
                elif fuel > 9.99:
                    fuel = round(fuel*10)
                    engine |= 1 << 1
                else:
                    fuel = round(fuel*100)
                    engine |= 2 << 1
            else:
                fuel = 0
        if fuel > 999:
            fuel = 999

        lapCount = self.simInfo.graphics.completedLaps + Config.instance.cfgLapOffset
        if lapCount > 199:
            lapCount = 199

        boost = round(ac.getCarState(0, acsys.CS.TurboBoost), 1)
        b1 = int(boost*10)
        if b1 < 0:
            b1 = 0

        if Config.instance.cfgBrakeEnable == 1:
            engine |= (self.calcBrakeVibe() << 3)


        delta = 0
        deltaNeg = 0
        mins = 0
        if self.prevFuel != 0:
            engine |= (1 << 4)
        if self.sendTimeReset == 1:
            self.sendTimeReset = 0
            engine |= (1 << 5)
        if self.sendTime == 1:
            self.sendTime = 0
            engine |= (1 << 6)
            time = self.simInfo.graphics.lastTime.split(":")
            delta = (int(time[1]) << 9) | int(time[2])
            mins = int(time[0])
            if mins > 99:
                mins = 99
        else:
            delta = ac.getCarState(0, acsys.CS.PerformanceMeter)
            deltaNeg = 0
            if delta <= 0:
                deltaNeg = 1
            delta = int(abs(delta) * 1000)
            if delta > 9999:
                delta = 9999


        bSetting = int(deltaNeg << 7) | int(Config.instance.cfgIntensity << 4) | int(Config.instance.cfgStartPage)

        key = bytes([255, bSetting, ac_gear, (ac_speed >> 8 & 0x00FF), (ac_speed & 0x00FF), ((rpms >> 8) & 0x00FF),
                     (rpms & 0x00FF), ((fuel >> 8) & 0x00FF), (fuel & 0x00FF), shift, engine, lapCount, b1,
                     ((delta >> 8) & 0x00FF), (delta & 0x00FF), mins])
        return key

    def estimateFuel(self):
        if self.prevFuel != 0 and self.fuelCache != 0:
            fuelUsg = self.prevFuel - self.simInfo.physics.fuel
            tmp = (self.fuelEst*self.fuelEstLaps) + fuelUsg
            self.fuelEstLaps += 1
            self.fuelEst = tmp/self.fuelEstLaps
            self.fuelCache.updateOption(self.track, self.car, self.fuelEst, True)
            self.fuelCache.updateOption(self.track, self.car + "_l", self.fuelEstLaps, True)
            #Log.log("Recalculate Fuel Usage Per Lap at %s in %s to %s" % (self.track, self.car, self.fuelEst))

        self.prevFuel = self.simInfo.physics.fuel

    #Thanks to Stephane Turpin for locking algorithm
    def calcBrakeVibe(self):
            speed = ac.getCarState(0, acsys.CS.SpeedMS)
            rpm = ac.getCarState(0, acsys.CS.RPM)
            brake = ac.getCarState(0, acsys.CS.Brake)

            #Calculates the ratio between the RPM and car speed
            #If RPM is minimum and the car is still moving -> we probably have locked tires
            if speed > 1:
                self.old_ratio = self.ratio
                self.ratio = rpm / speed
            else:
                self.old_ratio = 0
                self.ratio = 0
                return 0

            #Check if we are pressing the brakes and the ratio is increasing (same RPM and less speed = higher ratio)
            send = 0
            thresh = Config.instance.cfgBrakeTol / 100
            sens = 0.010 - (Config.instance.cfgBrakeSens / 1000)

            if (brake > thresh) and (self.ratio > 0):
                if (self.ratio > self.old_ratio) or (abs((self.ratio - self.old_ratio) / self.ratio) > sens):
                    send = 1

            return send


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